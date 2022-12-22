using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionsButton: MonoBehaviour
{
    void Start()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 1;
    }

    void Update()
    {
        if (GetComponent<Button>().keyDown)
        {
            SceneManager.LoadScene(2);
        }
    }
}
