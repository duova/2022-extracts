using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public GameObject l1;
    public GameObject l2;
    public GameObject r1;
    public GameObject r2;

    public float speed;

    void Update()
    {
        Left(l1);
        Left(l2);
        Right(r1);
        Right(r2);
    }

    void Left(GameObject tile)
    {
        if (tile.transform.position.y <= -19.2f)
        {
            ReturnToY(tile, 19.1f);
        }
        tile.transform.position += new Vector3(0, -speed * Time.deltaTime);
    }

    void Right(GameObject tile)
    {
        if (tile.transform.position.y >= 19.2f)
        {
            ReturnToY(tile, -19.1f);
        }
        tile.transform.position += new Vector3(0, speed * Time.deltaTime);
    }

    void ReturnToY(GameObject tile, float y)
    {
        tile.transform.position = new Vector3(tile.transform.position.x, y);
    }
}
