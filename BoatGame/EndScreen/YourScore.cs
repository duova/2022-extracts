using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YourScore : MonoBehaviour
{
    // Update is called once per frame
    void Start()
    {
        GetComponent<TextMesh>().text = "Rounds Survived: " + PlayerPrefs.GetInt("Last");
    }
}
