using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : Enemy
{
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!canMove) return;
        if (Info.IsDead) return;

        Vector2 targetOffset = targetPoint - (Vector2)transform.position;
        if(targetOffset.magnitude > 0.2f && targetOffset.magnitude < DetectRadius)
        {
            body2D.AddForce(14f * moveSpeed * targetOffset.normalized);
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (Info.IsDead) return;
        if (collision.collider.TryGetComponent(out PlayerInfo playerInfo))
        {
            playerInfo.OnHit(damage, ignoreInvincible: true);

            Destroy(gameObject);
        }
    }
}
