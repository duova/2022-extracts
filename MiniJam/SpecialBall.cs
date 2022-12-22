using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBall : Ball
{
    public int bouncesUntilDestroy = 2;

    protected override void Start()
    {
        base.Start();
        //spawn special effect
        currentColor = GameManager.BallColor.White;
    }

    protected override void OnBounce(GameManager.BallColor mixedColor)
    {
        if (bounceNumber == 0)
        {
            currentColor = mixedColor;
            //disable special effect
        }

        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x * gameManager.GetComponent<GameManager>().gameSpeedMultiplier, 2f * bounceYVelocity);

        if (mixedColor != currentColor && bounceNumber >= 1)
        {
            gameManager.GetComponent<GameManager>().gameSpeedMultiplier += gameManager.GetComponent<GameManager>().gameSpeedIncrease;
            gameManager.GetComponent<GameManager>().bounceCombo = 0;
        }

        if (bouncesUntilDestroy <= bounceNumber && GetComponent<Rigidbody2D>().velocity.y < 0.1f)
        {
            //impl effects
            Destroy(gameObject);
        }
    }
}
