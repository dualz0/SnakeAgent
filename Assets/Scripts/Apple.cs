using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public GameObject snake;
    public GameObject head;

    // 触发器，不会产生物理碰撞
    private void OnTriggerEnter(Collider col)
    {
        if (col.name == "Head")
        {
            GameObject.Find("GameManager").GetComponent<SnakeController>().getApple();
            int x = Random.Range(0, 9);
            int z = Random.Range(0, 9);
            transform.position = new Vector3(x + 0.5f, 0, z + 0.5f);
        }

        if (col.name == "Body(Clone)")
        {
            int x = Random.Range(0, 9);
            int z = Random.Range(0, 9);
            transform.position = new Vector3(x + 0.5f, 0, z + 0.5f);
        }
    }
}
