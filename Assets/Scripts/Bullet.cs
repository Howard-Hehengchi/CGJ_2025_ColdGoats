using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D body2D;

    [SerializeField] float speed = 20f;

    [SerializeField] float lifeTime = 2f;
    private float lifeTimer = 0f;

    private Vector2 lastFramePos;
    [SerializeField] LayerMask hitLayerMask;

    private void Start()
    {
        body2D = GetComponent<Rigidbody2D>();
        lifeTimer = 0f;
        lastFramePos = transform.position;
    }

    public void Init(Vector2 direction)
    {
        if(body2D == null)
        {
            body2D = GetComponent<Rigidbody2D>();
        }

        body2D.velocity = direction.normalized * speed;
    }

    private void Update()
    {
        lifeTimer += Time.deltaTime;

        if(lifeTimer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        Vector2 currentPos = (Vector2)transform.position;
        Vector2 nextPos = (Vector2)transform.position + body2D.velocity * Time.fixedDeltaTime;

        Vector2 offset = nextPos - currentPos;
        var hit2D = Physics2D.Raycast(currentPos, offset.normalized, offset.magnitude, hitLayerMask);
        if (hit2D && hit2D.transform != transform)
        {
            if(hit2D.transform.TryGetComponent(out Rigidbody2D hitBody2D))
            {
                // Apply force to the hit object
                Vector2 direction = (hit2D.point - currentPos).normalized;
                hitBody2D.AddForce(body2D.mass * body2D.velocity.magnitude * direction, ForceMode2D.Impulse);
            }

            if (hit2D.transform.TryGetComponent(out IHitable hitable))
            {
                //Vector2 direction = (hit2D.transform.position - transform.position).normalized;
                //print("Ray hit " + hit2D.transform.name);
                hitable.OnHit(hit2D.point);
            }

            if(!hit2D.transform.TryGetComponent(out UnitBehavior _))
            {
                SelfDestroy(hit2D.point, body2D.velocity.normalized);
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IHitable hitable))
        {
            //Vector2 direction = (collision.transform.position - transform.position).normalized;
            hitable.OnHit(collision.contacts[0].point);
        }
        
        SelfDestroy(collision.contacts[0].point, body2D.velocity.normalized);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        if (go.TryGetComponent(out HealthbarUnit hbUnit))
        {
            //Vector2 direction = (collision.transform.position - transform.position).normalized;
            //print("Trigger enter");
            hbUnit.OnHit(Vector2.zero); // 无意义传参
        }
        else if(go.TryGetComponent(out EscapeUnit escapeUnit))
        {
            escapeUnit.OnHit(Vector2.zero); // 无意义传参
        }

        if (!go.TryGetComponent(out BrickWall _))
        {
            Destroy(gameObject);
        }
    }

    private void SelfDestroy(Vector2 collidePoint, Vector2 velDir)
    {
        VFXManager.Instance.GenerateCollideVFX(collidePoint, -velDir);
        Destroy(gameObject);
    }
}
