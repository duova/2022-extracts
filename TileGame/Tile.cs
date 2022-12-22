using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int pos;

    public Color initialColor;

    public GameObject gameManager;

    public bool selected;

    int savedPos;

    Color savedColor;

    public Vector3 V1 = new Vector3(3.5f, 2.5f);
    public Vector3 V2 = new Vector3(4.5f, 2.5f);
    public Vector3 V3 = new Vector3(5.5f, 2.5f);
    public Vector3 V4 = new Vector3(6.5f, 2.5f);
    public Vector3 V5 = new Vector3(3.5f, 1.5f);
    public Vector3 V6 = new Vector3(4.5f, 1.5f);
    public Vector3 V7 = new Vector3(5.5f, 1.5f);
    public Vector3 V8 = new Vector3(6.5f, 1.5f);
    public Vector3 V9 = new Vector3(3.5f, 0.5f);
    public Vector3 V10 = new Vector3(4.5f, 0.5f);
    public Vector3 V11 = new Vector3(5.5f, 0.5f);
    public Vector3 V12 = new Vector3(6.5f, 0.5f);
    public Vector3 V13 = new Vector3(3.5f, -0.5f);
    public Vector3 V14 = new Vector3(4.5f, -0.5f);
    public Vector3 V15 = new Vector3(5.5f, -0.5f);
    public Vector3 V16 = new Vector3(6.5f, -0.5f);

    public Vector3 V17 = new Vector3(-6.5f, 2.5f);
    public Vector3 V18 = new Vector3(-5.5f, 2.5f);
    public Vector3 V19 = new Vector3(-4.5f, 2.5f);
    public Vector3 V20 = new Vector3(-3.5f, 2.5f);
    public Vector3 V21 = new Vector3(-6.5f, 1.5f);
    public Vector3 V22 = new Vector3(-5.5f, 1.5f);
    public Vector3 V23 = new Vector3(-4.5f, 1.5f);
    public Vector3 V24 = new Vector3(-3.5f, 1.5f);
    public Vector3 V25 = new Vector3(-6.5f, 0.5f);
    public Vector3 V26 = new Vector3(-5.5f, 0.5f);
    public Vector3 V27 = new Vector3(-4.5f, 0.5f);
    public Vector3 V28 = new Vector3(-3.5f, 0.5f);
    public Vector3 V29 = new Vector3(-6.5f, -0.5f);
    public Vector3 V30 = new Vector3(-5.5f, -0.5f);
    public Vector3 V31 = new Vector3(-4.5f, -0.5f);
    public Vector3 V32 = new Vector3(-3.5f, -0.5f);

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        initialColor = GetComponent<SpriteRenderer>().color;
    }

    //on update put tile into pos
    void Update()
    {
        if (pos == 1)
        {
            transform.position = V1;
        }
        if (pos == 2)
        {
            transform.position = V2;
        }
        if (pos == 3)
        {
            transform.position = V3;
        }
        if (pos == 4)
        {
            transform.position = V4;
        }
        if (pos == 5)
        {
            transform.position = V5;
        }
        if (pos == 6)
        {
            transform.position = V6;
        }
        if (pos == 7)
        {
            transform.position = V7;
        }
        if (pos == 8)
        {
            transform.position = V8;
        }
        if (pos == 9)
        {
            transform.position = V9;
        }
        if (pos == 10)
        {
            transform.position = V10;
        }
        if (pos == 11)
        {
            transform.position = V11;
        }
        if (pos == 12)
        {
            transform.position = V12;
        }
        if (pos == 13)
        {
            transform.position = V13;
        }
        if (pos == 14)
        {
            transform.position = V14;
        }
        if (pos == 15)
        {
            transform.position = V15;
        }
        if (pos == 16)
        {
            transform.position = V16;
        }

        if (pos == 17)
        {
            transform.position = V17;
        }
        if (pos == 18)
        {
            transform.position = V18;
        }
        if (pos == 19)
        {
            transform.position = V19;
        }
        if (pos == 20)
        {
            transform.position = V20;
        }
        if (pos == 21)
        {
            transform.position = V21;
        }
        if (pos == 22)
        {
            transform.position = V22;
        }
        if (pos == 23)
        {
            transform.position = V23;
        }
        if (pos == 24)
        {
            transform.position = V24;
        }
        if (pos == 25)
        {
            transform.position = V25;
        }
        if (pos == 26)
        {
            transform.position = V26;
        }
        if (pos == 27)
        {
            transform.position = V27;
        }
        if (pos == 28)
        {
            transform.position = V28;
        }
        if (pos == 29)
        {
            transform.position = V29;
        }
        if (pos == 30)
        {
            transform.position = V30;
        }
        if (pos == 31)
        {
            transform.position = V31;
        }
        if (pos == 32)
        {
            transform.position = V32;
        }

        if (selected && gameManager.GetComponent<GameManager>().LocateTileOnPos(pos - 4) != null && gameManager.GetComponent<GameManager>().LocateTileOnPos(pos - 4).GetComponent<Tile>().selected == true)
        {
            savedColor = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = gameManager.GetComponent<GameManager>().LocateTileOnPos(pos - 4).GetComponent<SpriteRenderer>().color;
            gameManager.GetComponent<GameManager>().LocateTileOnPos(pos - 4).GetComponent<SpriteRenderer>().color = savedColor;
            savedPos = pos;
            GameObject interactingTile = gameManager.GetComponent<GameManager>().LocateTileOnPos(pos - 4);
            pos = gameManager.GetComponent<GameManager>().LocateTileOnPos(pos - 4).GetComponent<Tile>().pos;
            interactingTile.GetComponent<Tile>().pos = savedPos;
            gameManager.GetComponent<GameManager>().ResetSelected();
        }
        else if (selected && gameManager.GetComponent<GameManager>().LocateTileOnPos(pos + 1) != null && gameManager.GetComponent<GameManager>().LocateTileOnPos(pos + 1).GetComponent<Tile>().selected == true)
        {
            savedColor = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = gameManager.GetComponent<GameManager>().LocateTileOnPos(pos + 1).GetComponent<SpriteRenderer>().color;
            gameManager.GetComponent<GameManager>().LocateTileOnPos(pos + 1).GetComponent<SpriteRenderer>().color = savedColor;
            savedPos = pos;
            GameObject interactingTile = gameManager.GetComponent<GameManager>().LocateTileOnPos(pos + 1);
            pos = gameManager.GetComponent<GameManager>().LocateTileOnPos(pos + 1).GetComponent<Tile>().pos;
            interactingTile.GetComponent<Tile>().pos = savedPos;
            gameManager.GetComponent<GameManager>().ResetSelected();
        }

        if (selected == true && gameManager.GetComponent<GameManager>().Mouse.GetComponent<SpriteRenderer>().bounds.Intersects(gameObject.GetComponent<SpriteRenderer>().bounds) && Input.GetKeyDown(KeyCode.Mouse0))
        {
            selected = false;
        }
        else if (gameManager.GetComponent<GameManager>().Mouse.GetComponent<SpriteRenderer>().bounds.Intersects(gameObject.GetComponent<SpriteRenderer>().bounds) && Input.GetKeyDown(KeyCode.Mouse0) && pos > 16)
        {
            selected = true;
        }

        if (selected == true)
        {
            GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 0.1f);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 1f);
        }
    }

    public void RevertColor()
    {
        gameObject.GetComponent<SpriteRenderer>().color = initialColor;
        selected = false;
    }
}
