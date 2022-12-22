using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    #region BoatColliders
    public GameObject col0;
    public GameObject col1;
    public GameObject col2;
    public GameObject col3;
    public GameObject col4;
    public GameObject col5;
    public GameObject col6;
    public GameObject col7;
    public GameObject col8;
    public GameObject col9;
    public GameObject col10;
    public GameObject col11;
    public GameObject col12;
    public GameObject col13;
    public GameObject col14;
    public GameObject col15;
    public GameObject col16;
    public GameObject col17;
    public GameObject col18;
    public GameObject col19;
    public GameObject col20;
    public GameObject col21;
    public GameObject col22;
    public GameObject col23;
    public GameObject col24;
    public GameObject col25;
    public GameObject col26;
    public GameObject col27;
    public GameObject col28;
    public GameObject col29;
    public GameObject col30;
    public GameObject col31;
    public GameObject col32;
    public GameObject col33;
    public GameObject col34;
    public GameObject col35;
    public GameObject col36;
    public GameObject col37;
    public GameObject col38;
    public GameObject col39;
    #endregion

    public enum State { Shipyard, Drag, Active, Sunk }
    public State state = State.Active;

    public float health = 100;
    public float maxHealth = 100;
    public float minHealth = 0;

    public int stage = 0;

    public GameObject shipyard;

    public GameObject upgradeButton;

    public GameObject cursor;

    public GameObject gameManager;

    public int iterateOrder;

    public Vector3 activePos;

    public int stuckArrows;

    public bool removeArrows;

    public float rotationSpeed;

    public bool processedIterator;

    public int roundSunk;

    public int sunkRemainTime;

    public Vector3 lastSafePos;

    GameObject boat0;
    GameObject boat1;
    GameObject boat2;
    GameObject boat3;
    GameObject boat4;

    /// <summary>
    /// In decimal percentage per level ie. 0.1 for 110% on first upgrade
    /// </summary>
    public float sizeIncrease;
    /// <summary>
    /// In decimal percentage per level ie. 0.1 for 110% on first upgrade
    /// </summary>
    public float healthIncrease;

    bool roundSunkSet;


    void Start()
    {
        activePos = transform.position;
    }

    void Update()
    {
        if (gameManager.GetComponent<GameManager>().sunkShips >= 4 && state == State.Shipyard)
        {
            gameObject.transform.position = gameManager.GetComponent<GameManager>().shipyardBoat.GetComponent<Boat>().activePos;
            state = State.Active;
            gameManager.GetComponent<GameManager>().shipyardBoat = null;
        }

        if (boat0 == null)
        {
            boat0 = GameObject.Find("Boat");
        }
        if (boat1 == null)
        {
            boat1 = GameObject.Find("Boat (1)");
        }
        if (boat2 == null)
        {
            boat2 = GameObject.Find("Boat (2)");
        }
        if (boat3 == null)
        {
            boat3 = GameObject.Find("Boat (3)");
        }
        if (boat4 == null)
        {
            boat4 = GameObject.Find("Boat (4)");
        }

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        if (health < minHealth)
        {
            health = minHealth;
        }

        if (health <= minHealth)
        {
            state = State.Sunk;
            if (!roundSunkSet)
            {
                roundSunk = gameManager.GetComponent<GameManager>().round;
                roundSunkSet = true;
            }
        }

        if (cursor.GetComponent<SpriteRenderer>().bounds.Intersects(GetComponent<SpriteRenderer>().bounds))
        {
            cursor.GetComponent<Cursor>().healthbarEnabled = true;
            cursor.GetComponent<Cursor>().healthbar.transform.localScale = new Vector3(0.5f * (health / maxHealth), cursor.GetComponent<Cursor>().healthbar.transform.localScale.y);
        }

        if (state == State.Sunk)
        {
            GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.4f, 0.4f, 1);
            if (gameManager.GetComponent<GameManager>().round - roundSunk > sunkRemainTime)
            {
                gameManager.GetComponent<GameManager>().sunkShips++;
                transform.position = new Vector3(100, 100);
                gameObject.SetActive(false);
            }
        }

        transform.localScale = new Vector3(6 + stage * sizeIncrease * 6, 6 + stage * sizeIncrease * 6);
        maxHealth = 100 + (healthIncrease * 100 * stage);

        if (state == State.Drag)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(new Vector3(0, 0, -rotationSpeed * Time.deltaTime));
            }

            transform.position = cursor.transform.position;


            if (GetComponent<Boat>().state == Boat.State.Drag)
            {
                if (gameObject == boat0)
                {
                    if (boat1.GetComponent<SpriteRenderer>().bounds.Intersects(GetComponent<SpriteRenderer>().bounds) || boat2.GetComponent<SpriteRenderer>().bounds.Intersects(GetComponent<SpriteRenderer>().bounds) || boat3.GetComponent<SpriteRenderer>().bounds.Intersects(GetComponent<SpriteRenderer>().bounds) || boat4.GetComponent<SpriteRenderer>().bounds.Intersects(GetComponent<SpriteRenderer>().bounds))
                    {
                        transform.position = lastSafePos;
                    }
                    else
                    {
                        lastSafePos = transform.position;
                    }
                }
                if (gameObject == boat1)
                {
                    if (boat0.GetComponent<SpriteRenderer>().bounds.Intersects(GetComponent<SpriteRenderer>().bounds) || boat2.GetComponent<SpriteRenderer>().bounds.Intersects(GetComponent<SpriteRenderer>().bounds) || boat3.GetComponent<SpriteRenderer>().bounds.Intersects(GetComponent<SpriteRenderer>().bounds) || boat4.GetComponent<SpriteRenderer>().bounds.Intersects(GetComponent<SpriteRenderer>().bounds))
                    {
                        transform.position = lastSafePos;
                    }
                    else
                    {
                        lastSafePos = transform.position;
                    }
                }
                if (gameObject == boat2)
                {
                    if (boat1.GetComponent<SpriteRenderer>().bounds.Intersects(GetComponent<SpriteRenderer>().bounds) || boat0.GetComponent<SpriteRenderer>().bounds.Intersects(GetComponent<SpriteRenderer>().bounds) || boat3.GetComponent<SpriteRenderer>().bounds.Intersects(GetComponent<SpriteRenderer>().bounds) || boat4.GetComponent<SpriteRenderer>().bounds.Intersects(GetComponent<SpriteRenderer>().bounds))
                    {
                        transform.position = lastSafePos;
                    }
                    else
                    {
                        lastSafePos = transform.position;
                    }
                }
                if (gameObject == boat3)
                {
                    if (boat1.GetComponent<SpriteRenderer>().bounds.Intersects(GetComponent<SpriteRenderer>().bounds) || boat2.GetComponent<SpriteRenderer>().bounds.Intersects(GetComponent<SpriteRenderer>().bounds) || boat0.GetComponent<SpriteRenderer>().bounds.Intersects(GetComponent<SpriteRenderer>().bounds) || boat4.GetComponent<SpriteRenderer>().bounds.Intersects(GetComponent<SpriteRenderer>().bounds))
                    {
                        transform.position = lastSafePos;
                    }
                    else
                    {
                        lastSafePos = transform.position;
                    }
                }
                if (gameObject == boat4)
                {
                    if (boat1.GetComponent<SpriteRenderer>().bounds.Intersects(GetComponent<SpriteRenderer>().bounds) || boat2.GetComponent<SpriteRenderer>().bounds.Intersects(GetComponent<SpriteRenderer>().bounds) || boat3.GetComponent<SpriteRenderer>().bounds.Intersects(GetComponent<SpriteRenderer>().bounds) || boat0.GetComponent<SpriteRenderer>().bounds.Intersects(GetComponent<SpriteRenderer>().bounds))
                    {
                        transform.position = lastSafePos;
                    }
                    else
                    {
                        lastSafePos = transform.position;
                    }
                }
            }

            if (transform.position.x > 7) transform.position = new Vector3(7, transform.position.y);
            if (transform.position.x < -7) transform.position = new Vector3(-7, transform.position.y);
            if (transform.position.y > 2) transform.position = new Vector3(transform.position.x, 2);
            if (transform.position.y < -4) transform.position = new Vector3(transform.position.x, -4);
        }

        if (gameManager.GetComponent<GameManager>().state == GameManager.State.Break)
        {
            //impl drag and shipyard states
            if (GetComponent<SpriteRenderer>().bounds.Intersects(cursor.GetComponent<SpriteRenderer>().bounds) && Input.GetKeyDown(KeyCode.Mouse0) && state != State.Sunk)
            {
                if (state == State.Shipyard)
                {
                    gameManager.GetComponent<GameManager>().shipyardBoat = null;
                }
                state = State.Drag;
                GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 0.5f);
            }

            if (Input.GetKeyUp(KeyCode.Mouse0) && state == State.Drag)
            {
                if (shipyard.GetComponent<SpriteRenderer>().bounds.Contains(transform.position))
                {
                    //check if already a boat in shipyard, if so replace, if not place.
                    if (gameManager.GetComponent<GameManager>().shipyardBoat != null)
                    {
                        gameManager.GetComponent<GameManager>().shipyardBoat.transform.position = gameManager.GetComponent<GameManager>().shipyardBoat.GetComponent<Boat>().activePos;
                        gameManager.GetComponent<GameManager>().shipyardBoat = gameObject;
                        state = State.Shipyard;
                        GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 1);
                        transform.position = shipyard.transform.position;
                        transform.rotation = Quaternion.Euler(0, 0, 90);
                    }
                    else
                    {
                        gameManager.GetComponent<GameManager>().shipyardBoat = gameObject;
                        state = State.Shipyard;
                        GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 1f);
                        transform.position = shipyard.transform.position;
                        transform.rotation = Quaternion.Euler(0, 0, 90);
                    }
                }
                else
                {
                    activePos = transform.position;
                    state = State.Active;
                    Relocate();
                    removeArrows = false;
                    if (gameManager.GetComponent<GameManager>().shipyardBoat == gameObject)
                    {
                        gameManager.GetComponent<GameManager>().shipyardBoat = null;
                    }
                    GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 1f);
                }
            }
        }
        else if (gameManager.GetComponent<GameManager>().state == GameManager.State.Iterate)
        {
            if (state != State.Shipyard)
            {
                removeArrows = false;
            }

            //revert non-shipyard ships to active positions;
            if (state == State.Drag && gameManager.GetComponent<GameManager>().movable == false)
            {
                GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 1f);
                transform.position = activePos;
                state = State.Active;
                Relocate();
            }

            if (state == State.Active && gameManager.GetComponent<GameManager>().movable == true && GetComponent<SpriteRenderer>().bounds.Intersects(cursor.GetComponent<SpriteRenderer>().bounds) && Input.GetKeyDown(KeyCode.Mouse0))
            {
                state = State.Drag;
                GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 0.5f);
            }

            if (Input.GetKeyUp(KeyCode.Mouse0) && state == State.Drag)
            {
                if (shipyard.GetComponent<SpriteRenderer>().bounds.Contains(transform.position))
                {
                    GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 1f);
                    transform.position = activePos;
                    state = State.Active;
                    Relocate();
                }
                else
                {
                    activePos = transform.position;
                    state = State.Active;
                    Relocate();
                    GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 1f);
                }
            }
        }
    }

    public BoatCollider BC(GameObject obj)
    {
        return obj.GetComponent<BoatCollider>();
    }

    public bool IsColliding(GameObject obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (BC(col0).IsColliding(obj) ||
            BC(col1).IsColliding(obj) ||
            BC(col2).IsColliding(obj) ||
            BC(col3).IsColliding(obj) ||
            BC(col4).IsColliding(obj) ||
            BC(col5).IsColliding(obj) ||
            BC(col6).IsColliding(obj) ||
            BC(col7).IsColliding(obj) ||
            BC(col8).IsColliding(obj) ||
            BC(col9).IsColliding(obj) ||
            BC(col10).IsColliding(obj) ||
            BC(col11).IsColliding(obj) ||
            BC(col12).IsColliding(obj) ||
            BC(col13).IsColliding(obj) ||
            BC(col14).IsColliding(obj) ||
            BC(col15).IsColliding(obj) ||
            BC(col16).IsColliding(obj) ||
            BC(col17).IsColliding(obj) ||
            BC(col18).IsColliding(obj) ||
            BC(col19).IsColliding(obj) ||
            BC(col20).IsColliding(obj) ||
            BC(col21).IsColliding(obj) ||
            BC(col22).IsColliding(obj) ||
            BC(col23).IsColliding(obj) ||
            BC(col24).IsColliding(obj) ||
            BC(col25).IsColliding(obj) ||
            BC(col26).IsColliding(obj) ||
            BC(col27).IsColliding(obj) ||
            BC(col28).IsColliding(obj) ||
            BC(col29).IsColliding(obj) ||
            BC(col30).IsColliding(obj) ||
            BC(col31).IsColliding(obj) ||
            BC(col32).IsColliding(obj) ||
            BC(col33).IsColliding(obj) ||
            BC(col34).IsColliding(obj) ||
            BC(col35).IsColliding(obj) ||
            BC(col36).IsColliding(obj) ||
            BC(col37).IsColliding(obj) ||
            BC(col38).IsColliding(obj) ||
            BC(col39).IsColliding(obj)
            )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddHealth(float health)
    {
        this.health += health;
        if (this.health > maxHealth)
        {
            this.health = maxHealth;
        }
    }

    public float CurrentHealth()
    {
        return health;
    }

    public void RemoveHealth(float health)
    {
        this.health -= health;
        if (this.health < minHealth)
        {
            this.health = minHealth;
        }
    }

    public void SetHealth(float health)
    {
        this.health = health;
        if (this.health > maxHealth)
        {
            this.health = maxHealth;
        }
        if (this.health < minHealth)
        {
            this.health = minHealth;
        }
    }

    public void FixBoat()
    {
        health = maxHealth;
        if (stuckArrows > 0)
        {
            gameManager.GetComponent<GameManager>().currency += stuckArrows;
            gameManager.GetComponent<GameManager>().increasedCurrencyText.GetComponent<TextMesh>().text = "+$" + stuckArrows.ToString();
            gameManager.GetComponent<GameManager>().increasedCurrencyText.GetComponent<TextMesh>().color = new Color(0, 1, 0, 1);
            gameManager.GetComponent<GameManager>().fadeTimer = 0;
            stuckArrows = 0;
            removeArrows = true;
        }
    }

    //sets the return to a very high value if the boat has already been processed by iterator
    public float GetProcessedXPos()
    {
        if (processedIterator)
        {
            return 9999;
        }
        else return transform.position.x;
    }

    void Relocate()
    {
        RelocateRepeatable(boat0);
        RelocateRepeatable(boat1);
        RelocateRepeatable(boat2);
        RelocateRepeatable(boat3);
        RelocateRepeatable(boat4);
    }

    void RelocateRepeatable(GameObject boat)
    {
        if (boat != gameObject)
        {
            if (boat.transform.position.x == transform.position.x) boat.transform.position += new Vector3(0.1f, 0);
        }
    }

}
