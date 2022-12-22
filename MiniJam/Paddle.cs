using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public GameObject gameManager;
    /// <summary>
    /// 0 is a and d, 1 is mouse0 and mouse1, 2 is shift and space.
    /// </summary>
    public int controlSet;
    public int velocity;
    public GameManager.BallColor color;
    public float paddleXLimit = 9;
    public KeyCode leftButton;
    public KeyCode rightButton;


    void Update()
    {
       
        Controller(leftButton, rightButton);

    }

    void Controller(KeyCode keyLeft, KeyCode keyRight)
    {
        if (Input.GetKey(keyLeft) && Input.GetKey(keyRight))
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            return;
        }
        if (Input.GetKey(keyLeft))
        {
            if (transform.position.x < -paddleXLimit)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(-velocity * gameManager.GetComponent<GameManager>().gameSpeedMultiplier, 0);
            }
        }
        if (Input.GetKey(keyRight))
        {
            if (transform.position.x > paddleXLimit)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(velocity * gameManager.GetComponent<GameManager>().gameSpeedMultiplier, 0);
            }
        }
        if (!Input.GetKey(keyRight) && !Input.GetKey(keyLeft))
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
}
