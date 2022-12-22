using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRespawn : MonoBehaviour
{
    public float health;
    public float maxHealth = 100;
    public bool alive = true; //reset entities if alive == false so when they're respawned they act the same
    public bool canRespawn;
    public Vector3 spawnPoint;
    public bool isPlayer;
    public GameObject refToGameManager;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        if (alive == false && canRespawn == true && isPlayer)
        {
            refToGameManager.GetComponent<GameManager>().ResetLevel();
            transform.position = spawnPoint;
            print("Respawned");
        }
        else if (alive == false && canRespawn == true && !isPlayer)
        {
            if (refToGameManager.GetComponent<GameManager>().resettingLevel == false)
            {
                enabled = false;
            }
            else
            {
                enabled = true;
                transform.position = spawnPoint;
            }
        }
        else if (alive == false)
        {
            enabled = false;
        }
    }

    public void SetMaxHealth(float value)
    {
        maxHealth = value;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public void SetHealth(float value)
    {
        health = value;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public void Damage(float value)
    {
        health -= value;
        if (health <= 0)
        {
            alive = false;
            health = 0;
        }
    }

    public void Heal(float value)
    {
        health += value;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }
}
