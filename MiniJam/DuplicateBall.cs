using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuplicateBall : Ball
{
    public int bouncesUntilDestroy;

    protected override void Start()
    {
        base.Start();
        //spawn effects
    }

    protected override void Update()
    {
        base.Update();
        if (bouncesUntilDestroy <= bounceNumber && GetComponent<Rigidbody2D>().velocity.y < 0.1f)
        {
            //impl effects
            Destroy(gameObject);
        }
    }
}
