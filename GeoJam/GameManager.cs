using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject leftGroup;
    public GameObject rightGroup;
    public GameObject player;
    public GameObject winBlock;

    [SerializeField]
    float blockVelocityMultiplier;

    void Update()
    {
        ApplyGroupVelocity();
        if (Vector3.Magnitude(winBlock.transform.position - player.transform.position) < 1)
        {
            SceneManager.LoadScene(2);
        }
    }

    void ApplyGroupVelocity()
    {
        leftGroup.transform.position += new Vector3(0, -blockVelocityMultiplier * Time.deltaTime);
        rightGroup.transform.position += new Vector3(0, blockVelocityMultiplier * Time.deltaTime);
    }
}
