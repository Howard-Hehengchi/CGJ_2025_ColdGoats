using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : AttackUnitBehavior
{
    [SerializeField] float moveSpeed = 3f;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        FindNearestEnemy();

        if (targetTF != null)
        {
            Vector2 targetDir = targetTF.position - transform.position;
            targetDir.Normalize();
            body2D.AddForce(5f * moveSpeed * targetDir);
        }
    }

    protected override void DoDamage(Enemy enemy)
    {
        enemy.OnHit(Vector2.zero); // 无意义传参
    }
}
