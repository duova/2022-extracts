using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float gameSpeedMultiplier = 1;
    public enum BallColor { Red, Blue, Green, Magenta, Cyan, Yellow, White, Null };
    public BallColor[] currentColorPattern;
    public float gameSpeedIncrease;
    public int bounceCombo;
    public int bouncesForReset;
    //Use bounceNumber on mainBall to trigger spawns;
    public GameObject mainBall;
    public int bouncesTillWin;
    public GameObject[] scheduledSpawnPrefabs;
    public Vector3[] scheduledSpawnLocations;
    public int[] scheduledSpawnOnBounceNumbers;
    private int checkedScheduledSpawn;
    GameObject newBall;
    DuplicateBall newDBallComponent;
    SpecialBall newSBallComponent;
    public float maxGameSpeed;
    public int level;

    void Update()
    {

        if (bounceCombo >= bouncesForReset)
        {
            bounceCombo = 0;
            gameSpeedMultiplier = 1;
        }

        if (mainBall.GetComponent<Ball>().bounceNumber >= bouncesTillWin)
        {
            Win();
        }

        if (scheduledSpawnOnBounceNumbers.GetLength(0) > checkedScheduledSpawn)
        {
            if (mainBall.GetComponent<Ball>().bounceNumber == scheduledSpawnOnBounceNumbers[checkedScheduledSpawn])
            {
                newBall = Instantiate(scheduledSpawnPrefabs[checkedScheduledSpawn], scheduledSpawnLocations[checkedScheduledSpawn], Quaternion.identity);
                if (newBall.TryGetComponent<DuplicateBall>(out newDBallComponent))
                {
                    newDBallComponent.gameManager = gameObject;
                }
                if (newBall.TryGetComponent<SpecialBall>(out newSBallComponent))
                {
                    newSBallComponent.gameManager = gameObject;
                }
                checkedScheduledSpawn++;
            }
        }

        if (gameSpeedMultiplier > maxGameSpeed)
        {
            Lose();
        }
    }

    public BallColor GetNextColor(BallColor currentColor)
    {
        for (int i = 0; i < currentColorPattern.GetLength(0); i++)
        {
            if (currentColorPattern[i] == currentColor)
            {
                if (currentColorPattern.GetLength(0) - 1 < i + 1)
                {
                    return currentColorPattern[0];
                }
                else
                {
                    return currentColorPattern[i + 1];
                }
            }
        }
        return BallColor.Null;
    }

    public void Lose()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //impl
    }

    public void Win()
    {
        //Load next scene in order
        if (PlayerPrefs.HasKey("highestLevel"))
        {
            if (PlayerPrefs.GetInt("highestLevel") > level)
            {
                return;
            }
            PlayerPrefs.SetInt("highestLevel", level);
        }
        else
        {
            PlayerPrefs.SetInt("highestLevel", level);
        }
        PlayerPrefs.Save();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
