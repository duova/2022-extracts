using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public bool isPlayer;
    Collider2D outCollider2D;
    HealthRespawn outHealthRespawn;

    public void RaycastAttack(Vector3 rangeDirection, Vector3 relativeOrigin, float damage)
    {
        if (TryGetComponent<Collider2D>(out outCollider2D)) GetComponent<Collider2D>().enabled = false;
        RaycastHit2D cast = Physics2D.Raycast(transform.position + relativeOrigin, Vector3.Normalize(rangeDirection), Vector3.Magnitude(rangeDirection));
        if (TryGetComponent<Collider2D>(out outCollider2D)) GetComponent<Collider2D>().enabled = true;

        if (cast.collider != null)
        {
            if (cast.collider.gameObject.TryGetComponent<HealthRespawn>(out outHealthRespawn))
            {
                outHealthRespawn.Damage(damage);
            }
        }
    }
}
