using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : AttackUnitBehavior
{
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] int damage = 1;

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
        else
        {
            Vector2 playerPos = PlayerController.Instance.transform.position;
            Vector2 dir = (playerPos - (Vector2)transform.position).normalized;
            Vector2 targetPoint = playerPos - dir * Random.Range(1.5f, 3.5f);
            Vector2 targetDir = targetPoint - (Vector2)transform.position;
            targetDir.Normalize();
            body2D.AddForce(5f * moveSpeed * targetDir);
        }
    }

    protected override void DoDamage(Enemy enemy)
    {
        enemy.OnHit(Vector2.zero, damage); // 无意义传参
        Destroy(gameObject);
    }
}
