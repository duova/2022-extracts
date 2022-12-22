using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool inverted;
    public bool resettingLevel;
    public int levelResetCounter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (resettingLevel == true)
        {
            levelResetCounter++;
            if (levelResetCounter >= 10)
            {
                resettingLevel = false;
                levelResetCounter = 0;
            }
        }
    }


    /// <summary>
    /// only called by HealthRespawn class, ignore otherwise
    /// </summary>
    public void ResetLevel()
    {
        resettingLevel = true;
    }
}
