using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickWall : DefendUnitBehavior
{
    private PlayerOrbitPositionAdjuster orbitAdjuster;

    protected override void Start()
    {
        base.Start();

        transform.SetParent(playerTF);
        orbitAdjuster = GetComponent<PlayerOrbitPositionAdjuster>();
        orbitAdjuster.constantDst = 1.5f;
    }

    private void Update()
    {
        Vector2 direction = new Vector2(Mathf.Cos(Time.time), Mathf.Sin(Time.time)).normalized;
        direction *= 5f;

        orbitAdjuster.targetPoint = (Vector2)playerTF.position + direction;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.transform.TryGetComponent(out Enemy enemy) && !enemy.Info.IsDead)
        {
            enemy.Info.Hurt(2);
            Info.Hurt(1);

            if (Info.IsDead)
            {
                Destroy(gameObject);
            }
        }
    }
}
