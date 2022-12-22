using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    float timer = 100;
    public void Play(Vector3 pos)
    {
        timer = 0;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Animator>().SetBool("Play", true);
        transform.position = pos;
    }

    void Update()
    {
        if (timer <= 1.83f)
        {
            timer += Time.deltaTime;
        }
        else
        {
            GetComponent<Animator>().SetBool("Play", false);
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
