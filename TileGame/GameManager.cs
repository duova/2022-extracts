using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject TileRed1;
    public GameObject TileRed2;
    public GameObject TileRed3;
    public GameObject TileRed4;
    public GameObject TileBlue1;
    public GameObject TileBlue2;
    public GameObject TileBlue3;
    public GameObject TileBlue4;
    public GameObject TileGreen1;
    public GameObject TileGreen2;
    public GameObject TileGreen3;
    public GameObject TileGreen4;
    public GameObject TileYellow1;
    public GameObject TileYellow2;
    public GameObject TileYellow3;
    public GameObject TileYellow4;

    public GameObject ITileRed1;
    public GameObject ITileRed2;
    public GameObject ITileRed3;
    public GameObject ITileRed4;
    public GameObject ITileBlue1;
    public GameObject ITileBlue2;
    public GameObject ITileBlue3;
    public GameObject ITileBlue4;
    public GameObject ITileGreen1;
    public GameObject ITileGreen2;
    public GameObject ITileGreen3;
    public GameObject ITileGreen4;
    public GameObject ITileYellow1;
    public GameObject ITileYellow2;
    public GameObject ITileYellow3;
    public GameObject ITileYellow4;

    public GameObject Mouse;

    public float time;

    public GameObject MainText;

    // Start is called before the first frame update
    void Start()
    {
        Randomize();
        GenerateStartBlock();
    }

    // Update is called once per frame
    void Update()
    {
        Mouse.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);

        time += Time.deltaTime;

        if (time > 12f)
        {
            ITileRed1.GetComponent<Tile>().RevertColor();
            ITileRed2.GetComponent<Tile>().RevertColor();
            ITileRed3.GetComponent<Tile>().RevertColor();
            ITileRed4.GetComponent<Tile>().RevertColor();
            ITileBlue1.GetComponent<Tile>().RevertColor();
            ITileBlue2.GetComponent<Tile>().RevertColor();
            ITileBlue3.GetComponent<Tile>().RevertColor();
            ITileBlue4.GetComponent<Tile>().RevertColor();
            ITileGreen1.GetComponent<Tile>().RevertColor();
            ITileGreen2.GetComponent<Tile>().RevertColor();
            ITileGreen3.GetComponent<Tile>().RevertColor();
            ITileGreen4.GetComponent<Tile>().RevertColor();
            ITileYellow1.GetComponent<Tile>().RevertColor();
            ITileYellow2.GetComponent<Tile>().RevertColor();
            ITileYellow3.GetComponent<Tile>().RevertColor();
            ITileYellow4.GetComponent<Tile>().RevertColor();

            if (
            LocateTileOnPos(TileRed1.GetComponent<Tile>().pos + 16).GetComponent<Tile>().initialColor == Color.red &&
            LocateTileOnPos(TileRed2.GetComponent<Tile>().pos + 16).GetComponent<Tile>().initialColor == Color.red &&
            LocateTileOnPos(TileRed3.GetComponent<Tile>().pos + 16).GetComponent<Tile>().initialColor == Color.red &&
            LocateTileOnPos(TileRed4.GetComponent<Tile>().pos + 16).GetComponent<Tile>().initialColor == Color.red &&
            LocateTileOnPos(TileBlue1.GetComponent<Tile>().pos + 16).GetComponent<Tile>().initialColor == Color.blue &&
            LocateTileOnPos(TileBlue2.GetComponent<Tile>().pos + 16).GetComponent<Tile>().initialColor == Color.blue &&
            LocateTileOnPos(TileBlue3.GetComponent<Tile>().pos + 16).GetComponent<Tile>().initialColor == Color.blue &&
            LocateTileOnPos(TileBlue4.GetComponent<Tile>().pos + 16).GetComponent<Tile>().initialColor == Color.blue &&
            LocateTileOnPos(TileGreen1.GetComponent<Tile>().pos + 16).GetComponent<Tile>().initialColor == Color.green &&
            LocateTileOnPos(TileGreen2.GetComponent<Tile>().pos + 16).GetComponent<Tile>().initialColor == Color.green &&
            LocateTileOnPos(TileGreen3.GetComponent<Tile>().pos + 16).GetComponent<Tile>().initialColor == Color.green &&
            LocateTileOnPos(TileGreen4.GetComponent<Tile>().pos + 16).GetComponent<Tile>().initialColor == Color.green &&
            LocateTileOnPos(TileYellow1.GetComponent<Tile>().pos + 16).GetComponent<Tile>().initialColor == new Color(1, 1, 0, 1) &&
            LocateTileOnPos(TileYellow2.GetComponent<Tile>().pos + 16).GetComponent<Tile>().initialColor == new Color(1, 1, 0, 1) &&
            LocateTileOnPos(TileYellow3.GetComponent<Tile>().pos + 16).GetComponent<Tile>().initialColor == new Color(1, 1, 0, 1) &&
            LocateTileOnPos(TileYellow4.GetComponent<Tile>().pos + 16).GetComponent<Tile>().initialColor == new Color(1, 1, 0, 1)
                )
            {
                MainText.GetComponent<TextMesh>().text = "You Win!";
            }
            else
            {
                MainText.GetComponent<TextMesh>().text = "You Lose!";
            }
        }
        else
        {
            MainText.GetComponent<TextMesh>().text = ((int)(12f - time)).ToString();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TileRed1.GetComponent<Tile>().pos = 0;
            TileRed2.GetComponent<Tile>().pos = 0;
            TileRed3.GetComponent<Tile>().pos = 0;
            TileRed4.GetComponent<Tile>().pos = 0;
            TileBlue1.GetComponent<Tile>().pos = 0;
            TileBlue2.GetComponent<Tile>().pos = 0;
            TileBlue3.GetComponent<Tile>().pos = 0;
            TileBlue4.GetComponent<Tile>().pos = 0;
            TileGreen1.GetComponent<Tile>().pos = 0;
            TileGreen2.GetComponent<Tile>().pos = 0;
            TileGreen3.GetComponent<Tile>().pos = 0;
            TileGreen4.GetComponent<Tile>().pos = 0;
            TileYellow1.GetComponent<Tile>().pos = 0;
            TileYellow2.GetComponent<Tile>().pos = 0;
            TileYellow3.GetComponent<Tile>().pos = 0;
            TileYellow4.GetComponent<Tile>().pos = 0;
            ITileRed1.GetComponent<Tile>().RevertColor();
            ITileRed2.GetComponent<Tile>().RevertColor();
            ITileRed3.GetComponent<Tile>().RevertColor();
            ITileRed4.GetComponent<Tile>().RevertColor();
            ITileBlue1.GetComponent<Tile>().RevertColor();
            ITileBlue2.GetComponent<Tile>().RevertColor();
            ITileBlue3.GetComponent<Tile>().RevertColor();
            ITileBlue4.GetComponent<Tile>().RevertColor();
            ITileGreen1.GetComponent<Tile>().RevertColor();
            ITileGreen2.GetComponent<Tile>().RevertColor();
            ITileGreen3.GetComponent<Tile>().RevertColor();
            ITileGreen4.GetComponent<Tile>().RevertColor();
            ITileYellow1.GetComponent<Tile>().RevertColor();
            ITileYellow2.GetComponent<Tile>().RevertColor();
            ITileYellow3.GetComponent<Tile>().RevertColor();
            ITileYellow4.GetComponent<Tile>().RevertColor();
            Randomize();
            GenerateStartBlock();
            time = 0;
        }
    }


    /*
     * Randomizes 4x4 tiles
     * randomly assigns 1-16 positional ints into each result tile
     * 16 int outputs correspond to tile positions from left to right, top down.
     */
    public void Randomize()
    {
        while (TileRed1.GetComponent<Tile>().pos == TileRed2.GetComponent<Tile>().pos ||
                TileRed1.GetComponent<Tile>().pos == TileRed3.GetComponent<Tile>().pos ||
                TileRed1.GetComponent<Tile>().pos == TileRed4.GetComponent<Tile>().pos ||
                TileRed1.GetComponent<Tile>().pos == TileBlue1.GetComponent<Tile>().pos ||
                TileRed1.GetComponent<Tile>().pos == TileBlue2.GetComponent<Tile>().pos ||
                TileRed1.GetComponent<Tile>().pos == TileBlue3.GetComponent<Tile>().pos ||
                TileRed1.GetComponent<Tile>().pos == TileBlue4.GetComponent<Tile>().pos ||
                TileRed1.GetComponent<Tile>().pos == TileGreen1.GetComponent<Tile>().pos ||
                TileRed1.GetComponent<Tile>().pos == TileGreen2.GetComponent<Tile>().pos ||
                TileRed1.GetComponent<Tile>().pos == TileGreen3.GetComponent<Tile>().pos ||
                TileRed1.GetComponent<Tile>().pos == TileGreen4.GetComponent<Tile>().pos ||
                TileRed1.GetComponent<Tile>().pos == TileYellow1.GetComponent<Tile>().pos ||
                TileRed1.GetComponent<Tile>().pos == TileYellow2.GetComponent<Tile>().pos ||
                TileRed1.GetComponent<Tile>().pos == TileYellow3.GetComponent<Tile>().pos ||
                TileRed1.GetComponent<Tile>().pos == TileYellow4.GetComponent<Tile>().pos)
        {
            TileRed1.GetComponent<Tile>().pos = Random.Range(1, 17);
        }

        while (TileRed2.GetComponent<Tile>().pos == TileRed1.GetComponent<Tile>().pos ||
                TileRed2.GetComponent<Tile>().pos == TileRed3.GetComponent<Tile>().pos ||
                TileRed2.GetComponent<Tile>().pos == TileRed4.GetComponent<Tile>().pos ||
                TileRed2.GetComponent<Tile>().pos == TileBlue1.GetComponent<Tile>().pos ||
                TileRed2.GetComponent<Tile>().pos == TileBlue2.GetComponent<Tile>().pos ||
                TileRed2.GetComponent<Tile>().pos == TileBlue3.GetComponent<Tile>().pos ||
                TileRed2.GetComponent<Tile>().pos == TileBlue4.GetComponent<Tile>().pos ||
                TileRed2.GetComponent<Tile>().pos == TileGreen1.GetComponent<Tile>().pos ||
                TileRed2.GetComponent<Tile>().pos == TileGreen2.GetComponent<Tile>().pos ||
                TileRed2.GetComponent<Tile>().pos == TileGreen3.GetComponent<Tile>().pos ||
                TileRed2.GetComponent<Tile>().pos == TileGreen4.GetComponent<Tile>().pos ||
                TileRed2.GetComponent<Tile>().pos == TileYellow1.GetComponent<Tile>().pos ||
                TileRed2.GetComponent<Tile>().pos == TileYellow2.GetComponent<Tile>().pos ||
                TileRed2.GetComponent<Tile>().pos == TileYellow3.GetComponent<Tile>().pos ||
                TileRed2.GetComponent<Tile>().pos == TileYellow4.GetComponent<Tile>().pos)
        {
            TileRed2.GetComponent<Tile>().pos = Random.Range(1, 17);
        }

        while (TileRed3.GetComponent<Tile>().pos == TileRed2.GetComponent<Tile>().pos ||
                TileRed3.GetComponent<Tile>().pos == TileRed1.GetComponent<Tile>().pos ||
                TileRed3.GetComponent<Tile>().pos == TileRed4.GetComponent<Tile>().pos ||
                TileRed3.GetComponent<Tile>().pos == TileBlue1.GetComponent<Tile>().pos ||
                TileRed3.GetComponent<Tile>().pos == TileBlue2.GetComponent<Tile>().pos ||
                TileRed3.GetComponent<Tile>().pos == TileBlue3.GetComponent<Tile>().pos ||
                TileRed3.GetComponent<Tile>().pos == TileBlue4.GetComponent<Tile>().pos ||
                TileRed3.GetComponent<Tile>().pos == TileGreen1.GetComponent<Tile>().pos ||
                TileRed3.GetComponent<Tile>().pos == TileGreen2.GetComponent<Tile>().pos ||
                TileRed3.GetComponent<Tile>().pos == TileGreen3.GetComponent<Tile>().pos ||
                TileRed3.GetComponent<Tile>().pos == TileGreen4.GetComponent<Tile>().pos ||
                TileRed3.GetComponent<Tile>().pos == TileYellow1.GetComponent<Tile>().pos ||
                TileRed3.GetComponent<Tile>().pos == TileYellow2.GetComponent<Tile>().pos ||
                TileRed3.GetComponent<Tile>().pos == TileYellow3.GetComponent<Tile>().pos ||
                TileRed3.GetComponent<Tile>().pos == TileYellow4.GetComponent<Tile>().pos)
        {
            TileRed3.GetComponent<Tile>().pos = Random.Range(1, 17);
        }

        while (TileRed4.GetComponent<Tile>().pos == TileRed2.GetComponent<Tile>().pos ||
                TileRed4.GetComponent<Tile>().pos == TileRed3.GetComponent<Tile>().pos ||
                TileRed4.GetComponent<Tile>().pos == TileRed1.GetComponent<Tile>().pos ||
                TileRed4.GetComponent<Tile>().pos == TileBlue1.GetComponent<Tile>().pos ||
                TileRed4.GetComponent<Tile>().pos == TileBlue2.GetComponent<Tile>().pos ||
                TileRed4.GetComponent<Tile>().pos == TileBlue3.GetComponent<Tile>().pos ||
                TileRed4.GetComponent<Tile>().pos == TileBlue4.GetComponent<Tile>().pos ||
                TileRed4.GetComponent<Tile>().pos == TileGreen1.GetComponent<Tile>().pos ||
                TileRed4.GetComponent<Tile>().pos == TileGreen2.GetComponent<Tile>().pos ||
                TileRed4.GetComponent<Tile>().pos == TileGreen3.GetComponent<Tile>().pos ||
                TileRed4.GetComponent<Tile>().pos == TileGreen4.GetComponent<Tile>().pos ||
                TileRed4.GetComponent<Tile>().pos == TileYellow1.GetComponent<Tile>().pos ||
                TileRed4.GetComponent<Tile>().pos == TileYellow2.GetComponent<Tile>().pos ||
                TileRed4.GetComponent<Tile>().pos == TileYellow3.GetComponent<Tile>().pos ||
                TileRed4.GetComponent<Tile>().pos == TileYellow4.GetComponent<Tile>().pos)
        {
            TileRed4.GetComponent<Tile>().pos = Random.Range(1, 17);
        }

        while (TileBlue1.GetComponent<Tile>().pos == TileRed2.GetComponent<Tile>().pos ||
                TileBlue1.GetComponent<Tile>().pos == TileRed3.GetComponent<Tile>().pos ||
                TileBlue1.GetComponent<Tile>().pos == TileRed4.GetComponent<Tile>().pos ||
                TileBlue1.GetComponent<Tile>().pos == TileRed1.GetComponent<Tile>().pos ||
                TileBlue1.GetComponent<Tile>().pos == TileBlue2.GetComponent<Tile>().pos ||
                TileBlue1.GetComponent<Tile>().pos == TileBlue3.GetComponent<Tile>().pos ||
                TileBlue1.GetComponent<Tile>().pos == TileBlue4.GetComponent<Tile>().pos ||
                TileBlue1.GetComponent<Tile>().pos == TileGreen1.GetComponent<Tile>().pos ||
                TileBlue1.GetComponent<Tile>().pos == TileGreen2.GetComponent<Tile>().pos ||
                TileBlue1.GetComponent<Tile>().pos == TileGreen3.GetComponent<Tile>().pos ||
                TileBlue1.GetComponent<Tile>().pos == TileGreen4.GetComponent<Tile>().pos ||
                TileBlue1.GetComponent<Tile>().pos == TileYellow1.GetComponent<Tile>().pos ||
                TileBlue1.GetComponent<Tile>().pos == TileYellow2.GetComponent<Tile>().pos ||
                TileBlue1.GetComponent<Tile>().pos == TileYellow3.GetComponent<Tile>().pos ||
                TileBlue1.GetComponent<Tile>().pos == TileYellow4.GetComponent<Tile>().pos)
        {
            TileBlue1.GetComponent<Tile>().pos = Random.Range(1, 17);
        }

        while (TileBlue2.GetComponent<Tile>().pos == TileRed2.GetComponent<Tile>().pos ||
                TileBlue2.GetComponent<Tile>().pos == TileRed3.GetComponent<Tile>().pos ||
                TileBlue2.GetComponent<Tile>().pos == TileRed4.GetComponent<Tile>().pos ||
                TileBlue2.GetComponent<Tile>().pos == TileBlue1.GetComponent<Tile>().pos ||
                TileBlue2.GetComponent<Tile>().pos == TileRed1.GetComponent<Tile>().pos ||
                TileBlue2.GetComponent<Tile>().pos == TileBlue3.GetComponent<Tile>().pos ||
                TileBlue2.GetComponent<Tile>().pos == TileBlue4.GetComponent<Tile>().pos ||
                TileBlue2.GetComponent<Tile>().pos == TileGreen1.GetComponent<Tile>().pos ||
                TileBlue2.GetComponent<Tile>().pos == TileGreen2.GetComponent<Tile>().pos ||
                TileBlue2.GetComponent<Tile>().pos == TileGreen3.GetComponent<Tile>().pos ||
                TileBlue2.GetComponent<Tile>().pos == TileGreen4.GetComponent<Tile>().pos ||
                TileBlue2.GetComponent<Tile>().pos == TileYellow1.GetComponent<Tile>().pos ||
                TileBlue2.GetComponent<Tile>().pos == TileYellow2.GetComponent<Tile>().pos ||
                TileBlue2.GetComponent<Tile>().pos == TileYellow3.GetComponent<Tile>().pos ||
                TileBlue2.GetComponent<Tile>().pos == TileYellow4.GetComponent<Tile>().pos)
        {
            TileBlue2.GetComponent<Tile>().pos = Random.Range(1, 17);
        }

        while (TileBlue3.GetComponent<Tile>().pos == TileRed2.GetComponent<Tile>().pos ||
                TileBlue3.GetComponent<Tile>().pos == TileRed3.GetComponent<Tile>().pos ||
                TileBlue3.GetComponent<Tile>().pos == TileRed4.GetComponent<Tile>().pos ||
                TileBlue3.GetComponent<Tile>().pos == TileBlue1.GetComponent<Tile>().pos ||
                TileBlue3.GetComponent<Tile>().pos == TileBlue2.GetComponent<Tile>().pos ||
                TileBlue3.GetComponent<Tile>().pos == TileRed1.GetComponent<Tile>().pos ||
                TileBlue3.GetComponent<Tile>().pos == TileBlue4.GetComponent<Tile>().pos ||
                TileBlue3.GetComponent<Tile>().pos == TileGreen1.GetComponent<Tile>().pos ||
                TileBlue3.GetComponent<Tile>().pos == TileGreen2.GetComponent<Tile>().pos ||
                TileBlue3.GetComponent<Tile>().pos == TileGreen3.GetComponent<Tile>().pos ||
                TileBlue3.GetComponent<Tile>().pos == TileGreen4.GetComponent<Tile>().pos ||
                TileBlue3.GetComponent<Tile>().pos == TileYellow1.GetComponent<Tile>().pos ||
                TileBlue3.GetComponent<Tile>().pos == TileYellow2.GetComponent<Tile>().pos ||
                TileBlue3.GetComponent<Tile>().pos == TileYellow3.GetComponent<Tile>().pos ||
                TileBlue3.GetComponent<Tile>().pos == TileYellow4.GetComponent<Tile>().pos)
        {
            TileBlue3.GetComponent<Tile>().pos = Random.Range(1, 17);
        }

        while (TileBlue4.GetComponent<Tile>().pos == TileRed2.GetComponent<Tile>().pos ||
                TileBlue4.GetComponent<Tile>().pos == TileRed3.GetComponent<Tile>().pos ||
                TileBlue4.GetComponent<Tile>().pos == TileRed4.GetComponent<Tile>().pos ||
                TileBlue4.GetComponent<Tile>().pos == TileBlue1.GetComponent<Tile>().pos ||
                TileBlue4.GetComponent<Tile>().pos == TileBlue2.GetComponent<Tile>().pos ||
                TileBlue4.GetComponent<Tile>().pos == TileBlue3.GetComponent<Tile>().pos ||
                TileBlue4.GetComponent<Tile>().pos == TileRed1.GetComponent<Tile>().pos ||
                TileBlue4.GetComponent<Tile>().pos == TileGreen1.GetComponent<Tile>().pos ||
                TileBlue4.GetComponent<Tile>().pos == TileGreen2.GetComponent<Tile>().pos ||
                TileBlue4.GetComponent<Tile>().pos == TileGreen3.GetComponent<Tile>().pos ||
                TileBlue4.GetComponent<Tile>().pos == TileGreen4.GetComponent<Tile>().pos ||
                TileBlue4.GetComponent<Tile>().pos == TileYellow1.GetComponent<Tile>().pos ||
                TileBlue4.GetComponent<Tile>().pos == TileYellow2.GetComponent<Tile>().pos ||
                TileBlue4.GetComponent<Tile>().pos == TileYellow3.GetComponent<Tile>().pos ||
                TileBlue4.GetComponent<Tile>().pos == TileYellow4.GetComponent<Tile>().pos)
        {
            TileBlue4.GetComponent<Tile>().pos = Random.Range(1, 17);
        }

        while (TileGreen1.GetComponent<Tile>().pos == TileRed2.GetComponent<Tile>().pos ||
                TileGreen1.GetComponent<Tile>().pos == TileRed3.GetComponent<Tile>().pos ||
                TileGreen1.GetComponent<Tile>().pos == TileRed4.GetComponent<Tile>().pos ||
                TileGreen1.GetComponent<Tile>().pos == TileBlue1.GetComponent<Tile>().pos ||
                TileGreen1.GetComponent<Tile>().pos == TileBlue2.GetComponent<Tile>().pos ||
                TileGreen1.GetComponent<Tile>().pos == TileBlue3.GetComponent<Tile>().pos ||
                TileGreen1.GetComponent<Tile>().pos == TileBlue4.GetComponent<Tile>().pos ||
                TileGreen1.GetComponent<Tile>().pos == TileRed1.GetComponent<Tile>().pos ||
                TileGreen1.GetComponent<Tile>().pos == TileGreen2.GetComponent<Tile>().pos ||
                TileGreen1.GetComponent<Tile>().pos == TileGreen3.GetComponent<Tile>().pos ||
                TileGreen1.GetComponent<Tile>().pos == TileGreen4.GetComponent<Tile>().pos ||
                TileGreen1.GetComponent<Tile>().pos == TileYellow1.GetComponent<Tile>().pos ||
                TileGreen1.GetComponent<Tile>().pos == TileYellow2.GetComponent<Tile>().pos ||
                TileGreen1.GetComponent<Tile>().pos == TileYellow3.GetComponent<Tile>().pos ||
                TileGreen1.GetComponent<Tile>().pos == TileYellow4.GetComponent<Tile>().pos)
        {
            TileGreen1.GetComponent<Tile>().pos = Random.Range(1, 17);
        }

        while (TileGreen2.GetComponent<Tile>().pos == TileRed2.GetComponent<Tile>().pos ||
                TileGreen2.GetComponent<Tile>().pos == TileRed3.GetComponent<Tile>().pos ||
                TileGreen2.GetComponent<Tile>().pos == TileRed4.GetComponent<Tile>().pos ||
                TileGreen2.GetComponent<Tile>().pos == TileBlue1.GetComponent<Tile>().pos ||
                TileGreen2.GetComponent<Tile>().pos == TileBlue2.GetComponent<Tile>().pos ||
                TileGreen2.GetComponent<Tile>().pos == TileBlue3.GetComponent<Tile>().pos ||
                TileGreen2.GetComponent<Tile>().pos == TileBlue4.GetComponent<Tile>().pos ||
                TileGreen2.GetComponent<Tile>().pos == TileGreen1.GetComponent<Tile>().pos ||
                TileGreen2.GetComponent<Tile>().pos == TileRed1.GetComponent<Tile>().pos ||
                TileGreen2.GetComponent<Tile>().pos == TileGreen3.GetComponent<Tile>().pos ||
                TileGreen2.GetComponent<Tile>().pos == TileGreen4.GetComponent<Tile>().pos ||
                TileGreen2.GetComponent<Tile>().pos == TileYellow1.GetComponent<Tile>().pos ||
                TileGreen2.GetComponent<Tile>().pos == TileYellow2.GetComponent<Tile>().pos ||
                TileGreen2.GetComponent<Tile>().pos == TileYellow3.GetComponent<Tile>().pos ||
                TileGreen2.GetComponent<Tile>().pos == TileYellow4.GetComponent<Tile>().pos)
        {
            TileGreen2.GetComponent<Tile>().pos = Random.Range(1, 17);
        }

        while (TileGreen3.GetComponent<Tile>().pos == TileRed2.GetComponent<Tile>().pos ||
                TileGreen3.GetComponent<Tile>().pos == TileRed3.GetComponent<Tile>().pos ||
                TileGreen3.GetComponent<Tile>().pos == TileRed4.GetComponent<Tile>().pos ||
                TileGreen3.GetComponent<Tile>().pos == TileBlue1.GetComponent<Tile>().pos ||
                TileGreen3.GetComponent<Tile>().pos == TileBlue2.GetComponent<Tile>().pos ||
                TileGreen3.GetComponent<Tile>().pos == TileBlue3.GetComponent<Tile>().pos ||
                TileGreen3.GetComponent<Tile>().pos == TileBlue4.GetComponent<Tile>().pos ||
                TileGreen3.GetComponent<Tile>().pos == TileGreen1.GetComponent<Tile>().pos ||
                TileGreen3.GetComponent<Tile>().pos == TileGreen2.GetComponent<Tile>().pos ||
                TileGreen3.GetComponent<Tile>().pos == TileRed1.GetComponent<Tile>().pos ||
                TileGreen3.GetComponent<Tile>().pos == TileGreen4.GetComponent<Tile>().pos ||
                TileGreen3.GetComponent<Tile>().pos == TileYellow1.GetComponent<Tile>().pos ||
                TileGreen3.GetComponent<Tile>().pos == TileYellow2.GetComponent<Tile>().pos ||
                TileGreen3.GetComponent<Tile>().pos == TileYellow3.GetComponent<Tile>().pos ||
                TileGreen3.GetComponent<Tile>().pos == TileYellow4.GetComponent<Tile>().pos)
        {
            TileGreen3.GetComponent<Tile>().pos = Random.Range(1, 17);
        }

        while (TileGreen4.GetComponent<Tile>().pos == TileRed2.GetComponent<Tile>().pos ||
                TileGreen4.GetComponent<Tile>().pos == TileRed3.GetComponent<Tile>().pos ||
                TileGreen4.GetComponent<Tile>().pos == TileRed4.GetComponent<Tile>().pos ||
                TileGreen4.GetComponent<Tile>().pos == TileBlue1.GetComponent<Tile>().pos ||
                TileGreen4.GetComponent<Tile>().pos == TileBlue2.GetComponent<Tile>().pos ||
                TileGreen4.GetComponent<Tile>().pos == TileBlue3.GetComponent<Tile>().pos ||
                TileGreen4.GetComponent<Tile>().pos == TileBlue4.GetComponent<Tile>().pos ||
                TileGreen4.GetComponent<Tile>().pos == TileGreen1.GetComponent<Tile>().pos ||
                TileGreen4.GetComponent<Tile>().pos == TileGreen2.GetComponent<Tile>().pos ||
                TileGreen4.GetComponent<Tile>().pos == TileGreen3.GetComponent<Tile>().pos ||
                TileGreen4.GetComponent<Tile>().pos == TileRed1.GetComponent<Tile>().pos ||
                TileGreen4.GetComponent<Tile>().pos == TileYellow1.GetComponent<Tile>().pos ||
                TileGreen4.GetComponent<Tile>().pos == TileYellow2.GetComponent<Tile>().pos ||
                TileGreen4.GetComponent<Tile>().pos == TileYellow3.GetComponent<Tile>().pos ||
                TileGreen4.GetComponent<Tile>().pos == TileYellow4.GetComponent<Tile>().pos)
        {
            TileGreen4.GetComponent<Tile>().pos = Random.Range(1, 17);
        }

        while (TileYellow1.GetComponent<Tile>().pos == TileRed2.GetComponent<Tile>().pos ||
                TileYellow1.GetComponent<Tile>().pos == TileRed3.GetComponent<Tile>().pos ||
                TileYellow1.GetComponent<Tile>().pos == TileRed4.GetComponent<Tile>().pos ||
                TileYellow1.GetComponent<Tile>().pos == TileBlue1.GetComponent<Tile>().pos ||
                TileYellow1.GetComponent<Tile>().pos == TileBlue2.GetComponent<Tile>().pos ||
                TileYellow1.GetComponent<Tile>().pos == TileBlue3.GetComponent<Tile>().pos ||
                TileYellow1.GetComponent<Tile>().pos == TileBlue4.GetComponent<Tile>().pos ||
                TileYellow1.GetComponent<Tile>().pos == TileGreen1.GetComponent<Tile>().pos ||
                TileYellow1.GetComponent<Tile>().pos == TileGreen2.GetComponent<Tile>().pos ||
                TileYellow1.GetComponent<Tile>().pos == TileGreen3.GetComponent<Tile>().pos ||
                TileYellow1.GetComponent<Tile>().pos == TileGreen4.GetComponent<Tile>().pos ||
                TileYellow1.GetComponent<Tile>().pos == TileRed1.GetComponent<Tile>().pos ||
                TileYellow1.GetComponent<Tile>().pos == TileYellow2.GetComponent<Tile>().pos ||
                TileYellow1.GetComponent<Tile>().pos == TileYellow3.GetComponent<Tile>().pos ||
                TileYellow1.GetComponent<Tile>().pos == TileYellow4.GetComponent<Tile>().pos)
        {
            TileYellow1.GetComponent<Tile>().pos = Random.Range(1, 17);
        }

        while (TileYellow2.GetComponent<Tile>().pos == TileRed2.GetComponent<Tile>().pos ||
                TileYellow2.GetComponent<Tile>().pos == TileRed3.GetComponent<Tile>().pos ||
                TileYellow2.GetComponent<Tile>().pos == TileRed4.GetComponent<Tile>().pos ||
                TileYellow2.GetComponent<Tile>().pos == TileBlue1.GetComponent<Tile>().pos ||
                TileYellow2.GetComponent<Tile>().pos == TileBlue2.GetComponent<Tile>().pos ||
                TileYellow2.GetComponent<Tile>().pos == TileBlue3.GetComponent<Tile>().pos ||
                TileYellow2.GetComponent<Tile>().pos == TileBlue4.GetComponent<Tile>().pos ||
                TileYellow2.GetComponent<Tile>().pos == TileGreen1.GetComponent<Tile>().pos ||
                TileYellow2.GetComponent<Tile>().pos == TileGreen2.GetComponent<Tile>().pos ||
                TileYellow2.GetComponent<Tile>().pos == TileGreen3.GetComponent<Tile>().pos ||
                TileYellow2.GetComponent<Tile>().pos == TileGreen4.GetComponent<Tile>().pos ||
                TileYellow2.GetComponent<Tile>().pos == TileYellow1.GetComponent<Tile>().pos ||
                TileYellow2.GetComponent<Tile>().pos == TileRed1.GetComponent<Tile>().pos ||
                TileYellow2.GetComponent<Tile>().pos == TileYellow3.GetComponent<Tile>().pos ||
                TileYellow2.GetComponent<Tile>().pos == TileYellow4.GetComponent<Tile>().pos)
        {
            TileYellow2.GetComponent<Tile>().pos = Random.Range(1, 17);
        }

        while (TileYellow3.GetComponent<Tile>().pos == TileRed2.GetComponent<Tile>().pos ||
                TileYellow3.GetComponent<Tile>().pos == TileRed3.GetComponent<Tile>().pos ||
                TileYellow3.GetComponent<Tile>().pos == TileRed4.GetComponent<Tile>().pos ||
                TileYellow3.GetComponent<Tile>().pos == TileBlue1.GetComponent<Tile>().pos ||
                TileYellow3.GetComponent<Tile>().pos == TileBlue2.GetComponent<Tile>().pos ||
                TileYellow3.GetComponent<Tile>().pos == TileBlue3.GetComponent<Tile>().pos ||
                TileYellow3.GetComponent<Tile>().pos == TileBlue4.GetComponent<Tile>().pos ||
                TileYellow3.GetComponent<Tile>().pos == TileGreen1.GetComponent<Tile>().pos ||
                TileYellow3.GetComponent<Tile>().pos == TileGreen2.GetComponent<Tile>().pos ||
                TileYellow3.GetComponent<Tile>().pos == TileGreen3.GetComponent<Tile>().pos ||
                TileYellow3.GetComponent<Tile>().pos == TileGreen4.GetComponent<Tile>().pos ||
                TileYellow3.GetComponent<Tile>().pos == TileYellow1.GetComponent<Tile>().pos ||
                TileYellow3.GetComponent<Tile>().pos == TileYellow2.GetComponent<Tile>().pos ||
                TileYellow3.GetComponent<Tile>().pos == TileRed1.GetComponent<Tile>().pos ||
                TileYellow3.GetComponent<Tile>().pos == TileYellow4.GetComponent<Tile>().pos)
        {
            TileYellow3.GetComponent<Tile>().pos = Random.Range(1, 17);
        }

        while (TileYellow4.GetComponent<Tile>().pos == TileRed2.GetComponent<Tile>().pos ||
                TileYellow4.GetComponent<Tile>().pos == TileRed3.GetComponent<Tile>().pos ||
                TileYellow4.GetComponent<Tile>().pos == TileRed4.GetComponent<Tile>().pos ||
                TileYellow4.GetComponent<Tile>().pos == TileBlue1.GetComponent<Tile>().pos ||
                TileYellow4.GetComponent<Tile>().pos == TileBlue2.GetComponent<Tile>().pos ||
                TileYellow4.GetComponent<Tile>().pos == TileBlue3.GetComponent<Tile>().pos ||
                TileYellow4.GetComponent<Tile>().pos == TileBlue4.GetComponent<Tile>().pos ||
                TileYellow4.GetComponent<Tile>().pos == TileGreen1.GetComponent<Tile>().pos ||
                TileYellow4.GetComponent<Tile>().pos == TileGreen2.GetComponent<Tile>().pos ||
                TileYellow4.GetComponent<Tile>().pos == TileGreen3.GetComponent<Tile>().pos ||
                TileYellow4.GetComponent<Tile>().pos == TileGreen4.GetComponent<Tile>().pos ||
                TileYellow4.GetComponent<Tile>().pos == TileYellow1.GetComponent<Tile>().pos ||
                TileYellow4.GetComponent<Tile>().pos == TileYellow2.GetComponent<Tile>().pos ||
                TileYellow4.GetComponent<Tile>().pos == TileYellow3.GetComponent<Tile>().pos ||
                TileYellow4.GetComponent<Tile>().pos == TileRed1.GetComponent<Tile>().pos ||
                TileYellow4.GetComponent<Tile>().pos == 0)
        {
            TileYellow4.GetComponent<Tile>().pos = Random.Range(1, 17);
        }

    }

    /*
     * Randomly changes original tile block
     * randomly assigns 17-32 positional ints into each result tile
     * 16 int outputs correspond to tile positions from left to right, top down.
     */
    public void GenerateStartBlock()
    {
        ITileRed1.GetComponent<Tile>().pos = TileRed1.GetComponent<Tile>().pos + 16;
        ITileRed2.GetComponent<Tile>().pos = TileRed2.GetComponent<Tile>().pos + 16;
        ITileRed3.GetComponent<Tile>().pos = TileRed3.GetComponent<Tile>().pos + 16;
        ITileRed4.GetComponent<Tile>().pos = TileRed4.GetComponent<Tile>().pos + 16;
        ITileBlue1.GetComponent<Tile>().pos = TileBlue1.GetComponent<Tile>().pos + 16;
        ITileBlue2.GetComponent<Tile>().pos = TileBlue2.GetComponent<Tile>().pos + 16;
        ITileBlue3.GetComponent<Tile>().pos = TileBlue3.GetComponent<Tile>().pos + 16;
        ITileBlue4.GetComponent<Tile>().pos = TileBlue4.GetComponent<Tile>().pos + 16;
        ITileGreen1.GetComponent<Tile>().pos = TileGreen1.GetComponent<Tile>().pos + 16;
        ITileGreen2.GetComponent<Tile>().pos = TileGreen2.GetComponent<Tile>().pos + 16;
        ITileGreen3.GetComponent<Tile>().pos = TileGreen3.GetComponent<Tile>().pos + 16;
        ITileGreen4.GetComponent<Tile>().pos = TileGreen4.GetComponent<Tile>().pos + 16;
        ITileYellow1.GetComponent<Tile>().pos = TileYellow1.GetComponent<Tile>().pos + 16;
        ITileYellow2.GetComponent<Tile>().pos = TileYellow2.GetComponent<Tile>().pos + 16;
        ITileYellow3.GetComponent<Tile>().pos = TileYellow3.GetComponent<Tile>().pos + 16;
        ITileYellow4.GetComponent<Tile>().pos = TileYellow4.GetComponent<Tile>().pos + 16;

        int doublePos = Random.Range(17, 25);
        int secondDoublePos = Random.Range(25, 33);

        RandomSwitch(Random.Range(17, 33));
        RandomSwitch(Random.Range(17, 33));
        RandomSwitch(doublePos);
        RandomSwitch(doublePos);
        RandomSwitch(Random.Range(17, 33));
        RandomSwitch(Random.Range(17, 33));
        RandomSwitch(secondDoublePos);
        RandomSwitch(secondDoublePos);
    }

    public GameObject LocateTileOnPos(int pos)
    {
        if (TileRed1.GetComponent<Tile>().pos == pos)
        {
            return TileRed1;
        }
        else if (TileRed2.GetComponent<Tile>().pos == pos)
        {
            return TileRed2;
        }
        else if (TileRed3.GetComponent<Tile>().pos == pos)
        {
            return TileRed3;
        }
        else if (TileRed4.GetComponent<Tile>().pos == pos)
        {
            return TileRed4;
        }
        else if (TileBlue1.GetComponent<Tile>().pos == pos)
        {
            return TileBlue1;
        }
        else if (TileBlue2.GetComponent<Tile>().pos == pos)
        {
            return TileBlue2;
        }
        else if (TileBlue3.GetComponent<Tile>().pos == pos)
        {
            return TileBlue3;
        }
        else if (TileBlue4.GetComponent<Tile>().pos == pos)
        {
            return TileBlue4;
        }
        else if (TileGreen1.GetComponent<Tile>().pos == pos)
        {
            return TileGreen1;
        }
        else if (TileGreen2.GetComponent<Tile>().pos == pos)
        {
            return TileGreen2;
        }
        else if (TileGreen3.GetComponent<Tile>().pos == pos)
        {
            return TileGreen3;
        }
        else if (TileGreen4.GetComponent<Tile>().pos == pos)
        {
            return TileGreen4;
        }
        else if (TileYellow1.GetComponent<Tile>().pos == pos)
        {
            return TileYellow1;
        }
        else if (TileYellow2.GetComponent<Tile>().pos == pos)
        {
            return TileYellow2;
        }
        else if (TileYellow3.GetComponent<Tile>().pos == pos)
        {
            return TileYellow3;
        }
        else if (TileYellow4.GetComponent<Tile>().pos == pos)
        {
            return TileYellow4;
        }

        if (ITileRed1.GetComponent<Tile>().pos == pos)
        {
            return ITileRed1;
        }
        else if (ITileRed2.GetComponent<Tile>().pos == pos)
        {
            return ITileRed2;
        }
        else if (ITileRed3.GetComponent<Tile>().pos == pos)
        {
            return ITileRed3;
        }
        else if (ITileRed4.GetComponent<Tile>().pos == pos)
        {
            return ITileRed4;
        }
        else if (ITileBlue1.GetComponent<Tile>().pos == pos)
        {
            return ITileBlue1;
        }
        else if (ITileBlue2.GetComponent<Tile>().pos == pos)
        {
            return ITileBlue2;
        }
        else if (ITileBlue3.GetComponent<Tile>().pos == pos)
        {
            return ITileBlue3;
        }
        else if (ITileBlue4.GetComponent<Tile>().pos == pos)
        {
            return ITileBlue4;
        }
        else if (ITileGreen1.GetComponent<Tile>().pos == pos)
        {
            return ITileGreen1;
        }
        else if (ITileGreen2.GetComponent<Tile>().pos == pos)
        {
            return ITileGreen2;
        }
        else if (ITileGreen3.GetComponent<Tile>().pos == pos)
        {
            return ITileGreen3;
        }
        else if (ITileGreen4.GetComponent<Tile>().pos == pos)
        {
            return ITileGreen4;
        }
        else if (ITileYellow1.GetComponent<Tile>().pos == pos)
        {
            return ITileYellow1;
        }
        else if (ITileYellow2.GetComponent<Tile>().pos == pos)
        {
            return ITileYellow2;
        }
        else if (ITileYellow3.GetComponent<Tile>().pos == pos)
        {
            return ITileYellow3;
        }
        else if (ITileYellow4.GetComponent<Tile>().pos == pos)
        {
            return ITileYellow4;
        }
        else return null;
    }

    public void RandomSwitch(int pos)
    {
        int savedPos = pos;
        GameObject operatingTile = LocateTileOnPos(pos);
        //1 is up, 2 is right, 3 is down, 4 is left
        int randRelativePos;
        if (pos == 2 + 16 || pos == 3 + 16)
        {
            randRelativePos = Random.Range(1, 5);
            while (randRelativePos == 1)
            {
                randRelativePos = Random.Range(1, 5);
            }
        }
        else if (pos == 8 + 16 || pos == 12 + 16)
        {
            randRelativePos = Random.Range(1, 5);
            while (randRelativePos == 2)
            {
                randRelativePos = Random.Range(1, 5);
            }
        }
        else if (pos == 5 + 16 || pos == 9 + 16)
        {
            randRelativePos = Random.Range(1, 5);
            while (randRelativePos == 4)
            {
                randRelativePos = Random.Range(1, 5);
            }
        }
        else if (pos == 14 + 16 || pos == 15 + 16)
        {
            randRelativePos = Random.Range(1, 5);
            while (randRelativePos == 3)
            {
                randRelativePos = Random.Range(1, 5);
            }
        }
        else if (pos == 1 + 16)
        {
            randRelativePos = Random.Range(1, 5);
            while (randRelativePos == 1 || randRelativePos == 4)
            {
                randRelativePos = Random.Range(1, 5);
            }
        }
        else if (pos == 4 + 16)
        {
            randRelativePos = Random.Range(1, 5);
            while (randRelativePos == 1 || randRelativePos == 2)
            {
                randRelativePos = Random.Range(1, 5);
            }
        }
        else if (pos == 13 + 16)
        {
            randRelativePos = Random.Range(1, 5);
            while (randRelativePos == 3 || randRelativePos == 4)
            {
                randRelativePos = Random.Range(1, 5);
            }
        }
        else if (pos == 16 + 16)
        {
            randRelativePos = Random.Range(1, 5);
            while (randRelativePos == 2 || randRelativePos == 3)
            {
                randRelativePos = Random.Range(1, 5);
            }
        }
        else
        {
            randRelativePos = Random.Range(1, 5);
        }

        if (randRelativePos == 1)
        {
            LocateTileOnPos(savedPos - 4).GetComponent<Tile>().pos = savedPos;
            operatingTile.GetComponent<Tile>().pos = savedPos - 4;
        }
        else if (randRelativePos == 2)
        {
            LocateTileOnPos(savedPos + 1).GetComponent<Tile>().pos = savedPos;
            operatingTile.GetComponent<Tile>().pos = savedPos + 1;
        }
        else if (randRelativePos == 3)
        {
            LocateTileOnPos(savedPos + 4).GetComponent<Tile>().pos = savedPos;
            operatingTile.GetComponent<Tile>().pos = savedPos + 4;
        }
        else if (randRelativePos == 4)
        {
            LocateTileOnPos(savedPos - 1).GetComponent<Tile>().pos = savedPos;
            operatingTile.GetComponent<Tile>().pos = savedPos - 1;
        }
    }

    public void ResetSelected()
    {
        ITileRed1.GetComponent<Tile>().selected = false;
        ITileRed2.GetComponent<Tile>().selected = false;
        ITileRed3.GetComponent<Tile>().selected = false;
        ITileRed4.GetComponent<Tile>().selected = false;
        ITileBlue1.GetComponent<Tile>().selected = false;
        ITileBlue2.GetComponent<Tile>().selected = false;
        ITileBlue3.GetComponent<Tile>().selected = false;
        ITileBlue4.GetComponent<Tile>().selected = false;
        ITileGreen1.GetComponent<Tile>().selected = false;
        ITileGreen2.GetComponent<Tile>().selected = false;
        ITileGreen3.GetComponent<Tile>().selected = false;
        ITileGreen4.GetComponent<Tile>().selected = false;
        ITileYellow1.GetComponent<Tile>().selected = false;
        ITileYellow2.GetComponent<Tile>().selected = false;
        ITileYellow3.GetComponent<Tile>().selected = false;
        ITileYellow4.GetComponent<Tile>().selected = false;
    }
}