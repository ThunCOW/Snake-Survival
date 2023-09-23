using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float turnSpeed;

    SnakeManager snakeManager;
    InputManager inputManager;

    bool firstTouch;

    private bool canMoveTail = true;

    void Start()
    {
        snakeManager = GetComponent<SnakeManager>();
        inputManager = GetComponent<InputManager>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            firstTouch = true;
    }
    void FixedUpdate()
    {
        SnakeMovement();
    }

    private void SnakeMovement()
    {
        snakeManager.SnakeBody[0].GetComponent<Rigidbody>().velocity = snakeManager.SnakeBody[0].transform.forward * speed * Time.deltaTime;
        if (firstTouch)
        {
            Vector3 distance = inputManager.AimTarget.transform.position - snakeManager.SnakeBody[0].transform.position;
            Quaternion rotation = Quaternion.LookRotation(distance);
            snakeManager.SnakeBody[0].transform.rotation = Quaternion.Lerp(snakeManager.SnakeBody[0].transform.rotation, rotation, turnSpeed * Time.deltaTime);
        }
        //if (Input.GetAxis("Horizontal") != 0)
        //  snakeManager.snakeBody[0].transform.Rotate(new Vector3(0, turnSpeed * Time.deltaTime * Input.GetAxis("Horizontal"), 0));

        if (snakeManager.SnakeBody.Count > 1)
        {
            for (int i = 1; i < snakeManager.SnakeBody.Count; i++)
            {
                SnakePartManager snakePartM = snakeManager.SnakeBody[i - 1].GetComponent<SnakePartManager>();
                if (i == snakeManager.SnakeBody.Count -1)
                {
                    if (canMoveTail)
                    {
                        snakeManager.SnakeBody[i].transform.position = snakePartM.SnakeParts[0].position;
                        snakeManager.SnakeBody[i].transform.rotation = snakePartM.SnakeParts[0].rotation;
                        snakePartM.SnakeParts.RemoveAt(0);
                    }
                }
                else
                {
                    snakeManager.SnakeBody[i].transform.position = snakePartM.SnakeParts[0].position;
                    snakeManager.SnakeBody[i].transform.rotation = snakePartM.SnakeParts[0].rotation;
                    snakePartM.SnakeParts.RemoveAt(0);
                }
            }
        }
    }

    public IEnumerator IncreaseBodySize(float waitFor)
    {
        canMoveTail = false;

        yield return new WaitForSeconds(waitFor);

        canMoveTail = true;
    }
}