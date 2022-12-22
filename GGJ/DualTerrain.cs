using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualTerrain : MonoBehaviour
{
    //game manager needs to implement inverting.
    public GameObject refToGameManager;
    public bool inverted;

    // Start is called before the first frame update
    void Start()
    {
        if (inverted)
        {
            enabled = false;
        }
        else
        {
            enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (refToGameManager.GetComponent<GameManager>().inverted == true)
        {
            if (inverted)
            {
                enabled = true;
            }
            else
            {
                enabled = false;
            }
        }
        else
        {
            if (inverted)
            {
                enabled = false;
            }
            else
            {
                enabled = true;
            }
        }
    }
}
