using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLantern : AttackUnitBehavior
{
    [SerializeField] float lifeTime = 10f;
    private float lifeTimer = 0f;
    [SerializeField] float attackInterval = 0.5f;
    private float intervalTimer = 0f;
    [SerializeField] int damage = 2;

    protected override void Start()
    {
        base.Start();

        VFXManager.Instance.GenerateExplosionVFX(transform.position, lifeTime);
        intervalTimer = attackInterval;
    }

    protected override void FixedUpdate()
    {
        lifeTimer += Time.fixedDeltaTime;
        if(lifeTimer >= lifeTime)
        {
            Destroy(gameObject);
            return; // FireLantern is destroyed after its lifetime
        }

        intervalTimer += Time.fixedDeltaTime;
        if(intervalTimer >= attackInterval)
        {
            intervalTimer = 0f;
            var colliders = Physics2D.OverlapCircleAll(transform.position, 2.5f);

            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out Enemy hitEnemy) && !hitEnemy.Info.IsDead)
                {
                    hitEnemy.OnHit(Vector2.zero, hitAmount: damage);
                }
            }
        }
    }

    protected override void DoDamage(Enemy enemy)
    {
        return; // FireLantern does not deal damage on collision
    }
}
