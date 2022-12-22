using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public bool hover;
    public bool keyDown;
    public bool keyUp;
    public bool keyHeld;
    public bool fadeOnHover = true;
    GameObject cursor;

    void Start()
    {
        cursor = GameObject.Find("Cursor");
    }

    void Update()
    {
        if (cursor.GetComponent<SpriteRenderer>().bounds.Intersects(GetComponent<SpriteRenderer>().bounds))
        {
            hover = true;
            if (fadeOnHover == true)
            {
                GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 0.5f);
            }
        }
        else
        {
            hover = false;
            if (fadeOnHover == true)
            {
                GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 1);
            }
        }

        if (cursor.GetComponent<SpriteRenderer>().bounds.Intersects(GetComponent<SpriteRenderer>().bounds) && Input.GetKeyDown(KeyCode.Mouse0))
        {
            keyDown = true;
        }
        else
        {
            keyDown = false;
        }

        if (cursor.GetComponent<SpriteRenderer>().bounds.Intersects(GetComponent<SpriteRenderer>().bounds) && Input.GetKeyUp(KeyCode.Mouse0))
        {
            keyUp = true;
        }
        else
        {
            keyUp = false;
        }

        if (cursor.GetComponent<SpriteRenderer>().bounds.Intersects(GetComponent<SpriteRenderer>().bounds) && Input.GetKey(KeyCode.Mouse0))
        {
            keyHeld = true;
        }
        else
        {
            keyHeld = false;
        }
    }
}
