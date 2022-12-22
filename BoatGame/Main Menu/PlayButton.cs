using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton: MonoBehaviour
{
    public int scene;

    void Start()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 1;
    }

    void Update()
    {
        if (GetComponent<Button>().keyDown)
        {
            if (scene == 0)
            {
                SceneManager.LoadScene(1);
            }
            else if (scene == 20)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                SceneManager.LoadScene(scene);
            }
        }
    }
}
