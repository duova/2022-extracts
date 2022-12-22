using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Speed : MonoBehaviour
{
    public GameObject gameManager;

    void Update()
    {
        GetComponent<TextMeshPro>().text = "Speed: " + (int)(gameManager.GetComponent<GameManager>().gameSpeedMultiplier * 100f) + "%";
    }
}
