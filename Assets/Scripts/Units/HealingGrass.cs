using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingGrass : DefendUnitBehavior
{
    [SerializeField] float acceleration = 40f;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

            // chase enemy
            Vector2 dir = playerTF.position - transform.position;
            dir.Normalize();

            float t = Mathf.Clamp01(body2D.velocity.magnitude / 5f);
            float f = Mathf.Lerp(0.1f, 1f, t * t * t);
            body2D.velocity += f * acceleration * Time.fixedDeltaTime * dir;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.collider.TryGetComponent(out PlayerInfo playerInfo))
        {
            playerInfo.OnHit(amount: -1);
            Destroy(gameObject);
        }
    }
}
