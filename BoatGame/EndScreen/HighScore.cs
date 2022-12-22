using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScore : MonoBehaviour
{
    // Update is called once per frame
    void Start()
    {

        if (PlayerPrefs.HasKey("Best")) GetComponent<TextMesh>().text = "Most Rounds Survived On Record: " + PlayerPrefs.GetInt("Best");
        if (!PlayerPrefs.HasKey("Best"))
        {
            PlayerPrefs.SetInt("Best", PlayerPrefs.GetInt("Last"));
            PlayerPrefs.Save();
        }
        else if (PlayerPrefs.GetInt("Last") > PlayerPrefs.GetInt("Best"))
        {
            PlayerPrefs.SetInt("Best", PlayerPrefs.GetInt("Last"));
            PlayerPrefs.Save();
        }
    }
}
