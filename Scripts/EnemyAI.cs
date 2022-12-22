using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public HealthRespawn refToHealth;
    public Vector3 firstPosition;
    public Vector3 secondPosition;
    public GameObject refToPlayer;
    public float speed;
    public GameObject projectilePrefab;
    public GameObject projectile;
    public float projectileForce;
    public float shootCD;
    private float shootCDcount;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Magnitude(refToPlayer.transform.position - ((Vector3.Normalize(secondPosition - transform.position) * speed * Time.deltaTime) + transform.position)) < Vector3.Magnitude(refToPlayer.transform.position - transform.position))
        {
            transform.position = (Vector3.Normalize(secondPosition - transform.position) * speed * Time.deltaTime) + transform.position;
        }
        else if (Vector3.Magnitude(refToPlayer.transform.position - ((Vector3.Normalize(firstPosition - transform.position) * speed * Time.deltaTime) + transform.position)) < Vector3.Magnitude(refToPlayer.transform.position - transform.position))
        {
            transform.position = (Vector3.Normalize(firstPosition - transform.position) * speed * Time.deltaTime) + transform.position;
        }

        shootCDcount += Time.deltaTime;

        if (shootCDcount >= shootCD)
        {
            projectile = Instantiate(projectilePrefab);
            projectile.transform.position = transform.position;
            projectile.GetComponent<Rigidbody2D>().AddForce(projectileForce * Vector3.Normalize(refToPlayer.transform.position - transform.position));
            shootCD = 0;
        }
    }
}
