using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatCollider : MonoBehaviour
{
    void Start()
    {
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
    }

    public bool IsColliding(GameObject obj)
    {
        if (obj != null)
        {
            if (obj.GetComponent<SpriteRenderer>().bounds.Intersects(GetComponent<SpriteRenderer>().bounds))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else return false;
    }
}
