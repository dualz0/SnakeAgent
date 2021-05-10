using UnityEngine;
using UnityEngine.UI;

public class SnakeController : MonoBehaviour
{
    public GameObject BodyPrefab;
    public GameObject head;
    public GameObject canvas;
    public int length;
    public int score;
    private Vector3 up = new Vector3(0, 0, 1);
    private Vector3 down = new Vector3(0, 0, -1);
    private Vector3 right = new Vector3(1, 0, 0);
    private Vector3 left = new Vector3(-1, 0, 0);
    private Vector3 direction;
    private Vector3 movingDirection;
 
    public bool moving;

    void Start()
    {
        restart();
    }

    public void restart() {
        Debug.Log("Restart!!!");

        score = 0;

        int randomInt = Random.Range(0, 4);
        switch(randomInt) {
            case 0:
                direction = up;
                break;
            case 1:
                direction = down;
                break;
            case 2:
                direction = left;
                break;
            case 3:
                direction = right;
                break;
        }
        movingDirection = direction;


        // 删除所有子物体
        for (int i = transform.childCount - 1; i >= 0; i--) {
            Destroy(transform.GetChild(i).gameObject);
        }
        length = 2;
        float x = Random.Range(2, 7) + 0.1f;
        float z = Random.Range(3, 8) - 0.1f;
        GameObject.Find("Head").transform.position = new Vector3(x, 0, z);
        for (int i = 0; i < length; i++) {
            GameObject body = Instantiate(BodyPrefab, transform);
            body.transform.position = new Vector3(head.transform.position.x, head.transform.position.y, head.transform.position.z - (i + 1));

            // body.GetComponent<GameOver>().canvas = canvas.gameObject;
        }
    }
    
    void Update()
    {
        canvas.transform.GetChild(1).GetComponent<Text>().text = "Score: " + score;
    }

    // 让苹果来调用这个方法
    public void getApple() {
        Debug.Log("GetApple!!");
        GameObject body = Instantiate(BodyPrefab, transform);

        if (transform.childCount != 0) {
            body.transform.position = transform.GetChild(length - 1).position;
        }
        // body.GetComponent<GameOver>().canvas = canvas.gameObject;
        length++;
        score++;
    }
}
