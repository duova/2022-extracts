using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject wave1;
    public GameObject wave2;
    public GameObject wave3;
    public GameObject wave4;
    public GameObject wave5;
    public GameObject wave6;
    public GameObject wave7;
    public Vector3 positiveBounds;
    public Vector3 negativeBounds;
    float timer;
    int nextWave;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            PlayNextWave(new Vector3(Random.Range(negativeBounds.x, positiveBounds.x), Random.Range(negativeBounds.y, positiveBounds.y)));
            timer = Random.Range(0.3f, 2f);
        }
    }

    void PlayNextWave(Vector3 pos)
    {
        switch(nextWave)
        {
            case 0:
                wave1.GetComponent<Wave>().Play(pos);
                nextWave++;
                break;
            case 1:
                wave2.GetComponent<Wave>().Play(pos);
                nextWave++;
                break;
            case 2:
                wave3.GetComponent<Wave>().Play(pos);
                nextWave++;
                break;
            case 3:
                wave4.GetComponent<Wave>().Play(pos);
                nextWave++;
                break;
            case 4:
                wave5.GetComponent<Wave>().Play(pos);
                nextWave++;
                break;
            case 5:
                wave6.GetComponent<Wave>().Play(pos);
                nextWave++;
                break;
            case 6:
                wave7.GetComponent<Wave>().Play(pos);
                nextWave = 0;
                break;
            default:
                nextWave = 0;
                break;
        }
    }
}
