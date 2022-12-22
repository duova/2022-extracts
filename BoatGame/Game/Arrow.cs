using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public enum State {Move, Stop}
    public State state = State.Move;
    public float arrowSpeed;
    public float arrowDamage;
    //false is normal true is enhanced;
    public bool arrowType;
    /// <summary>
    /// 1.1 is 110% of original value
    /// </summary>
    public float enhancedMultiplier;
    /// <summary>
    /// damage increased by this decimal percentage each round
    /// </summary>
    public float roundMultiplier;
    bool damageDealt;

    public GameObject boat0;
    public GameObject boat1;
    public GameObject boat2;
    public GameObject boat3;
    public GameObject boat4;

    GameObject contactedBoat;

    public GameObject gameManager;

    public bool checkCollisionWithBoatCheck;

    public bool onCollisionWithBoatCheck;

    public float sunkShipMultiplier;

    void Update()
    {
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

        if (gameManager == null)
        {
            gameManager = GameObject.Find("GameManager");
        }

        if (transform.position.y <= -10)
        {
            Destroy(gameObject);
        }

        if (state == State.Move)
        {
            transform.position += new Vector3(0, -arrowSpeed * Time.deltaTime);

            RunCollisionChecks();
        }

        if (state == State.Stop)
        {
            transform.parent = contactedBoat.transform;
            if (contactedBoat.GetComponent<Boat>().removeArrows)
            {
                Destroy(gameObject);
            }
        }
    }

    public bool CheckCollisionWithBoat(GameObject obj)
    {
        checkCollisionWithBoatCheck = true;
        if (GetComponent<SpriteRenderer>().bounds.Intersects(obj.GetComponent<SpriteRenderer>().bounds) && obj.GetComponent<Boat>().state != Boat.State.Shipyard)
        {
            if (obj.GetComponent<Boat>().IsColliding(gameObject))
            {
                OnCollisionWithBoat(obj);
                return true;
            }
            else
            {
                return false;
            }
        }
        else return false;
    }

    public void OnCollisionWithBoat(GameObject obj)
    {
        onCollisionWithBoatCheck = true;
        state = State.Stop;
        contactedBoat = obj;
        if (damageDealt == false)
        {
            obj.GetComponent<Boat>().stuckArrows++;
            if (!arrowType)
            {
                obj.GetComponent<Boat>().RemoveHealth(arrowDamage * (1 + (gameManager.GetComponent<GameManager>().round * roundMultiplier)) * (1 + (gameManager.GetComponent<GameManager>().sunkShips * sunkShipMultiplier)));
            }
            else
            {
                obj.GetComponent<Boat>().RemoveHealth(arrowDamage * (1 + (gameManager.GetComponent<GameManager>().round * roundMultiplier)) * (1 + (gameManager.GetComponent<GameManager>().sunkShips * sunkShipMultiplier)) * enhancedMultiplier);
            }
            damageDealt = true;
        }
    }

    public void RunCollisionChecks()
    {
        if (gameManager.GetComponent<GameManager>().Targetable(boat0))
        {
            if (CheckCollisionWithBoat(boat0)) return;
        }

        if (gameManager.GetComponent<GameManager>().Targetable(boat1))
        {
            if (CheckCollisionWithBoat(boat1)) return;
        }

        if (gameManager.GetComponent<GameManager>().Targetable(boat2))
        {
            if (CheckCollisionWithBoat(boat2)) return;
        }

        if (gameManager.GetComponent<GameManager>().Targetable(boat3))
        {
            if (CheckCollisionWithBoat(boat3)) return;
        }

        if (gameManager.GetComponent<GameManager>().Targetable(boat4))
        {
            if (CheckCollisionWithBoat(boat4)) return;
        }
    }
}
