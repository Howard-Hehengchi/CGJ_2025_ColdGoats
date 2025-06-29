using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthbarUnit : Enemy
{
    [HideInInspector]
    public UnitInfo EntityInfo;
    private BoxCollider2D boxCollider;

    private Healthbar healthbarComponent;

    protected override void Start()
    {
        base.Start();

        boxCollider = GetComponent<BoxCollider2D>();
        healthbarComponent = GetComponentInChildren<Healthbar>();
        Info.AddOnAlive(Detach);

        canMove = false;
        Info.SetInitState(_health: 0);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!canMove) return;
        if (Info.IsDead) return;

        Vector2 targetOffset = targetPoint - (Vector2)transform.position;
        body2D.AddForce(8f * moveSpeed * targetOffset.normalized);
    }

    private void Detach()
    {
        if (!Info.IsDead) return;

        healthbarComponent.enabled = false;
        EntityInfo.Hurt(10086);

        body2D = transform.AddComponent<Rigidbody2D>();
        body2D.gravityScale = 0f;
        body2D.drag = 2f;

        canMove = true;
        boxCollider.isTrigger = false;
        transform.SetParent(null);
        //Info.NeedHealthbar = true;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.collider.TryGetComponent(out PlayerInfo _))
        {
            Destroy(gameObject);
        }
    }

    public override void OnHit(Vector2 position, int hitAmount = 1)
    {
        base.OnHit(position, hitAmount);

        if (Info.IsDead)
        {
            EntityInfo.Hurt(hitAmount);
        }
    }
}
