using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public GameObject healthbar;
    public GameObject abilitybar;
    public GameObject chargebar;
    public GameObject angelIcon;
    public GameObject devilIcon;

    public GameObject player;
    public enum State { Angel, Devil };
    public State state;
    public float charge;
    public float angelAbility;
    public float devilAbility;

    void Start()
    {
        state = State.Angel;
    }

    // Update is called once per frame
    void Update()
    {
        healthbar.GetComponent<RectTransform>().localScale = new Vector3(player.GetComponent<HealthRespawn>().health / player.GetComponent<HealthRespawn>().maxHealth, 1, 1);

        if (state == State.Angel)
        {
            abilitybar.GetComponent<RectTransform>().localScale = new Vector3(angelAbility, 0, 0);
        }
        else
        {
            abilitybar.GetComponent<RectTransform>().localScale = new Vector3(devilAbility, 0, 0);
        }

        if (state == State.Devil)
        {
            chargebar.GetComponent<RectTransform>().localScale = new Vector3(charge, 0, 0);
        }
        else
        {

        }

        if (state == State.Angel)
        {
            angelIcon.SetActive(true);
            devilIcon.SetActive(false);
        }
        else
        {
            angelIcon.SetActive(false);
            devilIcon.SetActive(true);
        }
    }
}
