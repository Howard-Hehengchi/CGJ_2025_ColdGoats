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
        body2D.AddForce(14f * moveSpeed * targetOffset.normalized);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (Info.IsDead) return;
        base.OnCollisionEnter2D(collision);

        if (collision.collider.TryGetComponent(out PlayerInfo _))
        {
            Destroy(gameObject);
        }
    }
}
