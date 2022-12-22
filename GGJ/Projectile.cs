using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private HealthRespawn outHealthRespawn;
    private EnemyAI outEnemyAI;
    public float damage;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.Normalize(GetComponent<Rigidbody2D>().velocity), Vector3.up);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.TryGetComponent<HealthRespawn>(out outHealthRespawn) && outHealthRespawn.isPlayer == true)
        {
            outHealthRespawn.damage(damage);
            Destroy(this);
        }
        else if (!col.gameObject.TryGetComponent<EnemyAI>(out outEnemyAI))
        {
            Destroy(this);
        }
    }
}
