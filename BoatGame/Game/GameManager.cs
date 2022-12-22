using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum State { Iterate, Break, End }
    public State state;

    public int round;

    public float stateTimer;

    bool shipFixed;

    public GameObject boat0;
    public GameObject boat1;
    public GameObject boat2;
    public GameObject boat3;
    public GameObject boat4;

    //false is normal true is enhanced
    public bool currentArrowType;

    public GameObject shipyardBoat;

    public int currency;

    public GameObject currencyText;
    public GameObject roundText;
    public GameObject mainText;

    public float breakTime;

    public int enhancedArrowRoundDenominator;

    bool roundIncreased;

    float firstXValue;
    float secondXValue;
    float thirdXValue;
    float fourthXValue;
    float fifthXValue;

    GameObject firstOrderBoat;
    GameObject secondOrderBoat;
    GameObject thirdOrderBoat;
    GameObject fourthOrderBoat;
    GameObject fifthOrderBoat;

    bool determinedOrder;

    bool upgradeQueued;

    public int requiredCurrency;

    public GameObject upgradeButton;

    public int sunkShips;

    GameObject raycastLine;

    public GameObject raycastLinePrefab;

    public bool ballistaFireRequest;
    public bool ballistaEnhancedArrow;

    public int boatsToIterateThrough;
    float timeToNextVolley;
    public float timeBetweenVolley = 6;

    public bool movable;

    public int stageCap;

    public GameObject shipyardText;

    public GameObject increasedCurrencyText;

    public float fadeTimer;

    void Start()
    {
        round = 0;
        state = State.Break;
        stateTimer = 0;
    }

    void Update()
    {
        fadeTimer += Time.deltaTime;
        if (increasedCurrencyText.GetComponent<TextMesh>().color.a > 0)
        {
            increasedCurrencyText.GetComponent<TextMesh>().color = new Color(0, 1, 0, (1 - fadeTimer * 0.3f));
        }

        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

        if (raycastLine == null)
        {
            raycastLine = GameObject.Find("Line");
        }

        if (sunkShips >= 5)
        {
            PlayerPrefs.SetInt("Last", round);
            PlayerPrefs.Save();
            SceneManager.LoadScene(3);
        }

        timeToNextVolley -= Time.deltaTime;

        stateTimer += Time.deltaTime;

        roundText.GetComponent<TextMesh>().text = "Round " + round;

        currencyText.GetComponent<TextMesh>().text = "$" + currency;

        if (state == State.Break)
        {
            shipyardText.GetComponent<MeshRenderer>().enabled = false;
            if (Input.GetKeyDown(KeyCode.S) && (breakTime - stateTimer >= 1f))
            {
                stateTimer = breakTime;
            }

            if (!roundIncreased)
            {
                round++;
                roundIncreased = true;
            }

            shipFixed = false;
            mainText.GetComponent<TextMesh>().text = "Round Start In " + (int)(breakTime - stateTimer) + ". Press S to skip";
            if (stateTimer >= breakTime)
            {
                state = State.Iterate;
            }

            if (upgradeButton.GetComponent<Button>().keyDown)
            {
                //implement
                if (upgradeQueued == false && shipyardBoat != null)
                {
                    if (shipyardBoat.GetComponent<Boat>().stage >= stageCap)
                    {
                        upgradeButton.GetComponentInChildren<TextMesh>().text = "Max Stage";
                    }
                    else if (currency >= requiredCurrency)
                    {
                        upgradeQueued = true;
                        currency -= requiredCurrency;
                        upgradeButton.GetComponentInChildren<TextMesh>().text = "Queued";
                    }
                    else
                    {
                        upgradeButton.GetComponentInChildren<TextMesh>().text = "No $";
                    }
                }
            }

            determinedOrder = false;
            boatsToIterateThrough = 5;
        }
        else if (state == State.Iterate)
        {
            shipyardText.GetComponent<MeshRenderer>().enabled = true;
            stateTimer = 0;
            roundIncreased = false;
            if (round % enhancedArrowRoundDenominator == 0)
            {
                ballistaEnhancedArrow = true;
            }
            else
            {
                ballistaEnhancedArrow = false;
            }
            if (shipFixed == false)
            {
                //upgrading
                if (upgradeQueued == true)
                {
                    upgradeQueued = false;
                    upgradeButton.GetComponentInChildren<TextMesh>().text = "Upgrade";
                    shipyardBoat.GetComponent<Boat>().stage++;
                    shipyardBoat.GetComponent<Boat>().maxHealth = 100 + (shipyardBoat.GetComponent<Boat>().healthIncrease * 100 * shipyardBoat.GetComponent<Boat>().stage);
                }

                if (shipyardBoat != null)
                {
                    shipyardBoat.GetComponent<Boat>().FixBoat();
                }

                shipFixed = true;
            }

            if (!determinedOrder)
            {
                //determine boat order from left to right
                if (Targetable(boat0)) boat0.GetComponent<Boat>().processedIterator = false;
                if (Targetable(boat1)) boat1.GetComponent<Boat>().processedIterator = false;
                if (Targetable(boat2)) boat2.GetComponent<Boat>().processedIterator = false;
                if (Targetable(boat3)) boat3.GetComponent<Boat>().processedIterator = false;
                if (Targetable(boat4)) boat4.GetComponent<Boat>().processedIterator = false;
                AssignOrderAndRemove(ref firstXValue);
                AssignOrderAndRemove(ref secondXValue);
                AssignOrderAndRemove(ref thirdXValue);
                AssignOrderAndRemove(ref fourthXValue);
                AssignOrderAndRemove(ref fifthXValue);
                firstOrderBoat = GetBoatFromXPos(firstXValue);
                secondOrderBoat = GetBoatFromXPos(secondXValue);
                thirdOrderBoat = GetBoatFromXPos(thirdXValue);
                fourthOrderBoat = GetBoatFromXPos(fourthXValue);
                fifthOrderBoat = GetBoatFromXPos(fifthXValue);
                determinedOrder = true;
                print(firstOrderBoat);
                print(secondOrderBoat);
                print(thirdOrderBoat);
                print(fourthOrderBoat);
                print(fifthOrderBoat);
            }
            else
            {
                //impl raycasting and iterating and firing
                if (timeToNextVolley <= 0)
                {
                    mainText.GetComponent<TextMesh>().text = "Under Attack!";
                    movable = false;
                    switch (boatsToIterateThrough)
                    {
                        case 5:
                            if (Targetable(firstOrderBoat))
                            {
                                if (!RaycastForBoats(firstOrderBoat))
                                {
                                    FireBallistas(firstOrderBoat);
                                    timeToNextVolley = timeBetweenVolley;
                                }
                                else
                                {
                                    timeToNextVolley = 0;
                                }
                            }
                            else
                            {
                                timeToNextVolley = 0;
                            }
                            boatsToIterateThrough--;
                            break;
                        case 4:
                            if (Targetable(secondOrderBoat))
                            {
                                if (!RaycastForBoats(secondOrderBoat))
                                {
                                    FireBallistas(secondOrderBoat);
                                    timeToNextVolley = timeBetweenVolley;
                                }
                                else
                                {
                                    timeToNextVolley = 0;
                                }
                            }
                            else
                            {
                                timeToNextVolley = 0;
                            }
                            boatsToIterateThrough--;
                            break;
                        case 3:
                            if (Targetable(thirdOrderBoat))
                            {
                                if (!RaycastForBoats(thirdOrderBoat))
                                {
                                    FireBallistas(thirdOrderBoat);
                                    timeToNextVolley = timeBetweenVolley;
                                }
                                else
                                {
                                    timeToNextVolley = 0;
                                }
                            }
                            else
                            {
                                timeToNextVolley = 0;
                            }
                            boatsToIterateThrough--;
                            break;
                        case 2:
                            if (Targetable(fourthOrderBoat))
                            {
                                if (!RaycastForBoats(fourthOrderBoat))
                                {
                                    FireBallistas(fourthOrderBoat);
                                    timeToNextVolley = timeBetweenVolley;
                                }
                                else
                                {
                                    timeToNextVolley = 0;
                                }
                            }
                            else
                            {
                                timeToNextVolley = 0;
                            }
                            boatsToIterateThrough--;
                            break;
                        case 1:
                            if (Targetable(fifthOrderBoat))
                            {
                                if (!RaycastForBoats(fifthOrderBoat))
                                {
                                    FireBallistas(fifthOrderBoat);
                                    timeToNextVolley = timeBetweenVolley;
                                }
                                else
                                {
                                    timeToNextVolley = 0;
                                }
                            }
                            else
                            {
                                timeToNextVolley = 0;
                            }
                            boatsToIterateThrough--;
                            state = State.Break;
                            timeToNextVolley = 0;
                            break;
                        case 0:
                            state = State.Break;
                            timeToNextVolley = 0;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    mainText.GetComponent<TextMesh>().text = "Next Volley In " + (int)(timeToNextVolley) + ". Press F to fast forward";
                    if (Input.GetKey(KeyCode.F) && timeToNextVolley >= 1f)
                    {
                        timeToNextVolley = 1;
                    }
                    if (timeToNextVolley + 1 <= timeBetweenVolley)
                    {
                        movable = true;
                    }
                }
            }
        }
    }

    public GameObject GetBoatFromXPos(float XPos)
    {
        if (Targetable(boat0) && boat0.transform.position.x == XPos)
        {
            return boat0;
        }
        else if (Targetable(boat1) && boat1.transform.position.x == XPos)
        {
            return boat1;
        }
        else if (Targetable(boat2) && boat2.transform.position.x == XPos)
        {
            return boat2;
        }
        else if (Targetable(boat3) && boat3.transform.position.x == XPos)
        {
            return boat3;
        }
        else if (Targetable(boat4) && boat4.transform.position.x == XPos)
        {
            return boat4;
        }
        else return null;
    }

    public void AssignOrderAndRemove(ref float value)
    {
        float firstValue;
        float secondValue;
        float thirdValue;
        float fourthValue;
        float fifthValue;
        if (Targetable(boat0))
        {
            firstValue = boat0.GetComponent<Boat>().GetProcessedXPos();
        }
        else
        {
            firstValue = 9999;
        }
        if (Targetable(boat1))
        {
            secondValue = boat1.GetComponent<Boat>().GetProcessedXPos();
        }
        else
        {
            secondValue = 9999;
        }
        if (Targetable(boat2))
        {
            thirdValue = boat2.GetComponent<Boat>().GetProcessedXPos();
        }
        else
        {
            thirdValue = 9999;
        }
        if (Targetable(boat3))
        {
            fourthValue = boat3.GetComponent<Boat>().GetProcessedXPos();
        }
        else
        {
            fourthValue = 9999;
        }
        if (Targetable(boat4))
        {
            fifthValue = boat4.GetComponent<Boat>().GetProcessedXPos();
        }
        else
        {
            fifthValue = 9999;
        }

        value = Mathf.Min(firstValue, secondValue, thirdValue, fourthValue, fifthValue);
        if (GetBoatFromXPos(value) != null)
        {
            GetBoatFromXPos(value).GetComponent<Boat>().processedIterator = true;
        }
    }

    bool RaycastForBoats(GameObject castedObject)
    {
        print(castedObject);
        print(raycastLine);

        raycastLine.transform.position = new Vector3(castedObject.transform.position.x, castedObject.transform.position.y + 11.25f);

        if (RaycastForBoatsRepeatable(castedObject, boat0, raycastLine) ||
            RaycastForBoatsRepeatable(castedObject, boat1, raycastLine) ||
            RaycastForBoatsRepeatable(castedObject, boat2, raycastLine) ||
            RaycastForBoatsRepeatable(castedObject, boat3, raycastLine) ||
            RaycastForBoatsRepeatable(castedObject, boat4, raycastLine))
        {
            return true;
        }
        else return false;
    }

    bool RaycastForBoatsRepeatable(GameObject excludedBoat, GameObject checkedBoat, GameObject line)
    {
        if (excludedBoat == checkedBoat)
        {
            return false;
        }
        else
        {
            if (line.GetComponent<SpriteRenderer>().bounds.Intersects(checkedBoat.GetComponent<SpriteRenderer>().bounds))
            {
                return true;
            }
            else return false;
        }
    }

    void FireBallistas(GameObject castFrom)
    {
        raycastLine.transform.position = new Vector3(castFrom.transform.position.x, castFrom.transform.position.y + 10);
        ballistaFireRequest = true;
    }

    public bool Targetable(GameObject boat)
    {
        if (boat == null)
        {
            return false;
        }
        else if (boat.GetComponent<Boat>().state == Boat.State.Sunk)
        {
            return false;
        }
        else if (boat.GetComponent<Boat>().state == Boat.State.Shipyard)
        {
            return false;
        }
        else if (boat.activeInHierarchy == false)
        {
            return false;
        }
        else return true;
    }
}
