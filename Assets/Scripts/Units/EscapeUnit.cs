using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeUnit : MonoBehaviour, IHitable
{
    private Rigidbody2D body2D;
    [SerializeField] CardFlip cardComponent;

    private Vector2 fleeDir = Vector2.zero;
    [SerializeField] float acceleration = 5f;

    private void Start()
    {
        body2D = GetComponent<Rigidbody2D>();

        fleeDir = new Vector2(Random.Range(-1f, 1f), 1f).normalized;
    }

    private void FixedUpdate()
    {
        body2D.velocity += acceleration * Time.fixedDeltaTime * fleeDir;
    }

    public void OnHit(Vector2 position, int hitAmount = 1)
    {
        cardComponent.DoFlip();
    }
}
