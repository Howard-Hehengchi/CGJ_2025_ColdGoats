using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : AttackUnitBehavior
{
    [SerializeField] float acceleration = 40f;
    [SerializeField] int damage = 5;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (targetEnemy == null || targetEnemy.Info.IsDead)
        {
            FindNearestEnemy();
        }

        if (targetTF != null)
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
        VFXManager.Instance.GenerateExplosionVFX(transform.position, 0.2f);
        var colliders = Physics2D.OverlapCircleAll(transform.position, 2.5f);

        foreach (var collider in colliders)
        {
            if(collider.TryGetComponent(out Enemy hitEnemy) && !hitEnemy.Info.IsDead)
            {
                hitEnemy.OnHit(Vector2.zero, hitAmount: damage);
            }
        }

        Destroy(gameObject);
    }
}
