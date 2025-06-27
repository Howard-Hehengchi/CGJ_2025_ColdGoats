using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D body2D;

    [SerializeField] float speed = 20f;

    [SerializeField] float lifeTime = 2f;
    private float lifeTimer = 0f;

    private void Start()
    {
        body2D = GetComponent<Rigidbody2D>();
        lifeTimer = 0f;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent(out IHitable hitable))
        {
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            hitable.OnHit(direction);
        }


        Destroy(gameObject);
    }
}
