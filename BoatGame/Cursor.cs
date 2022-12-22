using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    public GameObject healthbar;
    public GameObject healthbarBackground;

    public bool healthbarEnabled;


    //Vector3 lastSafePos;

    void Start()
    {
        UnityEngine.Cursor.visible = false;
    }

    void Update()
    {

        transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

        if (healthbar != null && healthbarBackground != null)
        {
            if (healthbarEnabled)
            {
                healthbar.GetComponent<SpriteRenderer>().enabled = true;
                healthbarBackground.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                healthbar.GetComponent<SpriteRenderer>().enabled = false;
                healthbarBackground.GetComponent<SpriteRenderer>().enabled = false;
            }
            healthbarEnabled = false;
        }
    }
}
