using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
public class SnakeAgent : Agent
{
    Rigidbody rBody;
    private Vector3 up = new Vector3(0, 0, 1);
    private Vector3 down = new Vector3(0, 0, -1);
    private Vector3 right = new Vector3(1, 0, 0);
    private Vector3 left = new Vector3(-1, 0, 0);
    const int k_Up = 0; 
    const int k_Down = 1;
    const int k_Left = 2;
    const int k_Right = 3;

    private Vector3 direction;
    private Vector3 movingDirection;
    public float threshold = 0.5f;
    private float timer;
    void Start () {
        rBody = GetComponent<Rigidbody>();
    }
    public int length;
    public bool maskActions = true;
    public GameObject BodyPrefab;

    public Transform Target;
    public GameObject canvas;
    public GameObject GameManager;
    public float timeBetweenDecisionsAtInference;

    float m_TimeSinceDecision;

    [Tooltip("Because we want an observation right before making a decision, we can force " +
        "a camera to render before making a decision. Place the agentCam here if using " +
        "RenderTexture as observations.")]
    public Camera renderCamera;


    public override void OnEpisodeBegin()
    {
        Debug.Log("OnEpisodeBegin!!!");
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(Target.position);
        sensor.AddObservation(this.transform.position);
        
        Vector3 relativeposition = Target.position - this.transform.position;
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Get the action index for 
        int movement = actionBuffers.DiscreteActions[0];
        int directionX = 0, directionY = 0, directionZ = 0;
        float preDistanceToTarget = Vector3.Distance(this.transform.position, Target.position);

        // Look up the index in the movement action list:
        switch (movement)
        {
            case k_Up:
                directionZ = 1;
                break;
            case k_Down:
                directionZ = -1;
                break;
            case k_Left:
                directionX = -1;
                break;
            case k_Right:
                directionX = 1;
                break;
        }
        movingDirection = new Vector3(directionX, directionY, directionZ);

        if (timer > threshold)
        {
            int length = GameObject.Find("GameManager").GetComponent<SnakeController>().length;
            Vector3 saveCurPos = this.transform.position;
            this.transform.position += movingDirection;
            for (int i = length - 1; i > 0; i--)
            {
                GameManager.transform.GetChild(i).transform.position = GameManager.transform.GetChild(i - 1).transform.position;
            }
            
            GameManager.transform.GetChild(0).transform.position = saveCurPos;

            float curDistanceToTarget = Vector3.Distance(this.transform.position, Target.position);

            float DistanceReward = 1.0f / curDistanceToTarget * 0.25f;

            AddReward(DistanceReward);
            AddReward(-0.05f);

            timer = 0;
        }
        timer += Time.deltaTime;     
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Wall") {
            Debug.Log("wall");
            AddReward(-1f);
            GameObject.Find("GameManager").GetComponent<SnakeController>().restart();
            EndEpisode();
        }

        else if (other.tag == "Apple") {
            Debug.Log("Apple");
            // GameObject.Find("GameManager").GetComponent<SnakeController>().getApple();
            AddReward(+1.0f);
            float x = (float)Random.Range(0, 9) + 0.5f;
            float z = (float)Random.Range(0, 9) + 0.5f;
            GameObject.Find("Apple").transform.position = new Vector3(x, 0, z);
            if (threshold > 0.1f) {
                threshold -= 0.05f;
            }
        }
        else if (other.tag == "Body") {
            Debug.Log("body");
            AddReward(-1f);
            GameObject.Find("GameManager").GetComponent<SnakeController>().restart();
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

        switch (Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"))) {
            case -1: discreteActions[0] = 2; break;
            case  0:                         break;
            case +1: discreteActions[0] = 3; break;
        }
        switch (Mathf.RoundToInt(Input.GetAxisRaw("Vertical"))) {
            case -1: discreteActions[0] = 1; break;
            case  0:                         break;
            case +1: discreteActions[0] = 0; break;
        }
    }
}