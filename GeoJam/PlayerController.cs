using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public bool inverted;
    public float maxHorizontalVelocity;
    public float horizontalAcceleration;
    public float horizontalDecceleration;
    public float jumpForce;

    void Update()
    {
        if (inverted)
        {
            GetComponent<Rigidbody2D>().gravityScale = -1;
        }
        else
        {
            GetComponent<Rigidbody2D>().gravityScale = 1;
        }

        if (transform.position.x > 0)
        {
            inverted = true;
        }
        else
        {
            inverted = false;
        }

        //strafe
        if (Input.GetKey(KeyCode.LeftArrow) && GetComponent<Rigidbody2D>().velocity.x > -maxHorizontalVelocity)
        {
            if (GetComponent<Rigidbody2D>().velocity.x > 0)
            {
                StopStrafe();
            }
            else
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector3(-horizontalAcceleration * Time.deltaTime, 0));
            }
        }

        if (Input.GetKey(KeyCode.RightArrow) && GetComponent<Rigidbody2D>().velocity.x < maxHorizontalVelocity)
        {
            if (GetComponent<Rigidbody2D>().velocity.x < 0)
            {
                StopStrafe();
            }
            else
            {
                if (maxHorizontalVelocity - GetComponent<Rigidbody2D>().velocity.x < horizontalAcceleration * Time.deltaTime)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector3(maxHorizontalVelocity, GetComponent<Rigidbody2D>().velocity.y);
                }
                else
                {
                    GetComponent<Rigidbody2D>().AddForce(new Vector3(horizontalAcceleration * Time.deltaTime, 0));
                }
            }
        }

        if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            StopStrafe();
        }

        //jump
        if (!inverted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GetComponent<Collider2D>().enabled = false;
                if (Physics2D.BoxCast(transform.position, transform.GetChild(0).GetComponent<SpriteRenderer>().bounds.size, 0, Vector3.down, 0.5f))
                {
                    GetComponent<Rigidbody2D>().AddForce(new Vector3(0, jumpForce));
                }
                GetComponent<Collider2D>().enabled = true;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector3(0, -jumpForce * 1.5f));
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GetComponent<Collider2D>().enabled = false;
                if (Physics2D.BoxCast(transform.position, transform.GetChild(0).GetComponent<SpriteRenderer>().bounds.size, 0, Vector3.up, 0.5f))
                {
                    GetComponent<Rigidbody2D>().AddForce(new Vector3(0, -jumpForce));
                }
                GetComponent<Collider2D>().enabled = true;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector3(0, jumpForce * 1.5f));
            }
        }

        //ends of screen
        if (transform.position.x > 9)
        {
            transform.position = new Vector3(-9, transform.position.y);
        }
        if (transform.position.x < -9)
        {
            transform.position = new Vector3(9, transform.position.y);
        }

        //loss condition
        if (transform.position.y > 5 || transform.position.y < -5)
        {
            SceneManager.LoadScene(3);
        }
    }

    void StopStrafe()
    {
        if (GetComponent<Rigidbody2D>().velocity.x > 0)
        {
            if (GetComponent<Rigidbody2D>().velocity.x < horizontalDecceleration * Time.deltaTime)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector3(0, GetComponent<Rigidbody2D>().velocity.y);
            }
            else
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector3(-horizontalDecceleration * Time.deltaTime, 0));
            }
        }

        if (GetComponent<Rigidbody2D>().velocity.x < 0)
        {
            if (GetComponent<Rigidbody2D>().velocity.x > -horizontalDecceleration * Time.deltaTime)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector3(0, GetComponent<Rigidbody2D>().velocity.y);
            }
            else
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector3(horizontalDecceleration * Time.deltaTime, 0));
            }
        }
    }
}
