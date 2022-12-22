using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public GameObject gameManager;
    public List<float> bounceXVelocity;
    public float bounceYVelocity;
    public int bounceNumber;
    public GameManager.BallColor currentColor;
    public float minY;
    float bounceCD;
    public GameObject spriteRendererChild;

    Paddle paddle;
    Paddle raycastedPaddle;
    Patch patch;

    protected virtual void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, bounceYVelocity * gameManager.GetComponent<GameManager>().gameSpeedMultiplier);
    }

    protected virtual void Update()
    {
        if (bounceXVelocity.Count > bounceNumber)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(bounceXVelocity[bounceNumber] * gameManager.GetComponent<GameManager>().gameSpeedMultiplier, GetComponent<Rigidbody2D>().velocity.y);
        }

        if (transform.position.y < minY)
        {
            gameManager.GetComponent<GameManager>().Lose();
        }

        bounceCD -= Time.deltaTime;

        //sped up gravity
        GetComponent<Rigidbody2D>().velocity += new Vector2(0, -4f * (gameManager.GetComponent<GameManager>().gameSpeedMultiplier - 1) * Time.deltaTime);

        //implement color changes
        switch (currentColor)
        {
            case GameManager.BallColor.Red:
                spriteRendererChild.GetComponent<SpriteRenderer>().color = new Color(167f / 255f , 29 / 255f, 32 / 255f, 1);
                break;
            case GameManager.BallColor.Green:
                spriteRendererChild.GetComponent<SpriteRenderer>().color = new Color(55f / 255f, 174 / 255f, 80 / 255f, 1);
                break;
            case GameManager.BallColor.Blue:
                spriteRendererChild.GetComponent<SpriteRenderer>().color = new Color(115f / 255f, 124f / 255f, 247f / 255f, 1);
                break;
            case GameManager.BallColor.Yellow:
                spriteRendererChild.GetComponent<SpriteRenderer>().color = new Color(253f / 255f, 255f / 255f, 98f / 255f, 1);
                break;
            case GameManager.BallColor.Magenta:
                spriteRendererChild.GetComponent<SpriteRenderer>().color = new Color(225f / 255f, 98f / 255f, 255f / 255f, 1);
                break;
            case GameManager.BallColor.Cyan:
                spriteRendererChild.GetComponent<SpriteRenderer>().color = new Color(94f / 255f, 255f / 255f, 227f / 255f, 1);
                break;
            case GameManager.BallColor.White:
                spriteRendererChild.GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 1);
                break;
            default:
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //On collision with paddle for all balls
        if (col.TryGetComponent<Paddle>( out paddle) && bounceCD < 0)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, bounceYVelocity * gameManager.GetComponent<GameManager>().gameSpeedMultiplier);
            bounceCD = 0.1f;

            RaycastHit2D[] rays = Physics2D.BoxCastAll(transform.position, GetComponent<Collider2D>().bounds.size, 0, Vector2.down, 0.2f);

            GameManager.BallColor mixedColor = DetectMixedColorFromBoxcast(rays);

            if (mixedColor == currentColor && gameManager.GetComponent<GameManager>().gameSpeedMultiplier > 1.1f)
            {
                gameManager.GetComponent<GameManager>().bounceCombo++;
            }
            OnBounce(mixedColor);
            bounceNumber++;
        }
        else if (col.gameObject.TryGetComponent<Patch>(out patch))
        {
            //impl ball hit patch effect
            ChangeToNextColor();
        }
    }

    //Override collision logic for ball types
    protected virtual void OnBounce(GameManager.BallColor mixedColor)
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x * gameManager.GetComponent<GameManager>().gameSpeedMultiplier, bounceYVelocity);

        if (mixedColor != currentColor)
        {
            gameManager.GetComponent<GameManager>().gameSpeedMultiplier += gameManager.GetComponent<GameManager>().gameSpeedIncrease;
            gameManager.GetComponent<GameManager>().bounceCombo = 0;
        }

        transform.GetChild(0).gameObject.GetComponent<Animator>().Play("Bounce");

        ChangeToNextColor();
    }

    void ChangeToNextColor()
    {
        currentColor = gameManager.GetComponent<GameManager>().GetNextColor(currentColor);
    }

    public GameManager.BallColor DetectMixedColorFromBoxcast(RaycastHit2D[] rays)
    {
        List<GameManager.BallColor> colors = new List<GameManager.BallColor>();
        GameManager.BallColor mixedColor;

        foreach (RaycastHit2D ray in rays)
        {
            if (ray.collider.TryGetComponent<Paddle>(out raycastedPaddle))
            {
                colors.Add(raycastedPaddle.color);
            }
        }

        switch (colors.Count)
        {
            case 0:
                mixedColor = GameManager.BallColor.Null;
                break;
            case 1:
                mixedColor = colors[0];
                break;
            case 2:
                if (colors.Contains(GameManager.BallColor.Red) && colors.Contains(GameManager.BallColor.Green))
                {
                    mixedColor = GameManager.BallColor.Yellow;
                }
                else if (colors.Contains(GameManager.BallColor.Blue) && colors.Contains(GameManager.BallColor.Green))
                {
                    mixedColor = GameManager.BallColor.Cyan;
                }
                else if (colors.Contains(GameManager.BallColor.Blue) && colors.Contains(GameManager.BallColor.Red))
                {
                    mixedColor = GameManager.BallColor.Magenta;
                }
                else
                {
                    mixedColor = GameManager.BallColor.Null;
                }
                break;
            case 3:
                mixedColor = GameManager.BallColor.White;
                break;
            default:
                mixedColor = GameManager.BallColor.Null;
                break;
        }

        return mixedColor;
    }
}
