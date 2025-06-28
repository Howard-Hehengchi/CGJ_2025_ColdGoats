using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExplosiveRock : UnitBehavior
{
    [SerializeField] float acceleration = 5f;
    [SerializeField] int damage = 5;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if(targetTF == null)
        {
            FindNearestEnemy();
        }

        if(targetTF != null)
        {
            // chase enemy
            Vector2 dir = targetTF.position - transform.position;
            dir.Normalize();

            float t = Mathf.Clamp01(body2D.velocity.magnitude / 5f);
            float f = Mathf.Lerp(0.1f, 1f, t * t * t);
            body2D.velocity += f * acceleration * Time.fixedDeltaTime * dir;
        }
    }

    protected override void DoDamage(Enemy enemy)
    {
        enemy.OnHit(Vector2.zero, hitAmount: damage);
    }
}
