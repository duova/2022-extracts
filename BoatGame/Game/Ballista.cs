using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballista : MonoBehaviour
{
    public GameObject leftBallista;
    public GameObject rightBallista;

    GameObject line;

    GameObject gameManager;

    float animationTimer;

    /// <summary>
    /// Minus 1 divided by 2
    /// </summary>
    public int volleyWidth = 8;

    bool firing;

    bool shot;

    bool loadedArrowEnhanced;

    public GameObject arrowPrefab;
    public GameObject arrowEnhancedPrefab;

    void Start()
    {
        if (rightBallista != null)
        {
            rightBallista.GetComponent<Ballista>().leftBallista = gameObject;
        }
    }

    void Update()
    {
        if (line == null)
        {
            line = GameObject.Find("Line");
        }

        if (gameManager == null)
        {
            gameManager = GameObject.Find("GameManager");
        }
        
        if (line.GetComponent<SpriteRenderer>().bounds.Intersects(GetComponent<SpriteRenderer>().bounds))
        {
            if (gameManager.GetComponent<GameManager>().ballistaFireRequest == true)
            {
                FireVolley(gameManager.GetComponent<GameManager>().ballistaEnhancedArrow);
                gameManager.GetComponent<GameManager>().ballistaFireRequest = false;
            }
        }

        if (firing)
        {
            animationTimer += Time.deltaTime;
            if (animationTimer >= 0.8f && shot == true)
            {
                if (!loadedArrowEnhanced)
                {
                    Instantiate(arrowPrefab, transform.position, Quaternion.identity);

                }
                else
                {
                    Instantiate(arrowEnhancedPrefab, transform.position, Quaternion.identity);
                }
                shot = false;
            }
            if (animationTimer >= 1f)
            {
                GetComponent<Animator>().SetBool("Play", false);
                animationTimer = 0;
                firing = false;
            }
        }
    }

    public void FireVolley(bool enhancedArrow)
    {
        Fire(enhancedArrow);
        LeftChainFire(volleyWidth, enhancedArrow);
        RightChainFire(volleyWidth, enhancedArrow);
    }

    public void LeftChainFire(int remaining, bool enhancedArrow)
    {
        if (leftBallista != null)
        {
            leftBallista.GetComponent<Ballista>().Fire(enhancedArrow);
            if (remaining > 1)
            {
                leftBallista.GetComponent<Ballista>().LeftChainFire(remaining - 1, enhancedArrow);
            }
        }
    }

    public void RightChainFire(int remaining, bool enhancedArrow)
    {
        if (rightBallista != null)
        {
            rightBallista.GetComponent<Ballista>().Fire(enhancedArrow);
            if (remaining > 1)
            {
                rightBallista.GetComponent<Ballista>().RightChainFire(remaining - 1, enhancedArrow);
            }
        }
    }

    public void Fire(bool enhancedArrow)
    {
        //impl arrow instanciation and animation
        GetComponent<Animator>().SetBool("Play", true);
        firing = true;
        shot = true;
        loadedArrowEnhanced = enhancedArrow;
    }
}
