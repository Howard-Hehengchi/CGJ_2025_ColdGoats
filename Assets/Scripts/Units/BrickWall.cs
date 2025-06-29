using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickWall : DefendUnitBehavior
{
    private PlayerOrbitPositionAdjuster orbitAdjuster;

    private BoxCollider2D boxCollider;

    private const int totalCount = 6; // 砖墙的总数量
    private const float angleInterval = Mathf.PI * 2f / totalCount;
    private static bool[] indexOccupiedArray;
    private static int currentCount = 0; // 当前砖墙数量
    private int currentIndex = 0;
    private float angleOffset = 0f;

    protected override void Start()
    {
        base.Start(); 

        if(indexOccupiedArray == null)
        {
            indexOccupiedArray = new bool[totalCount];
            for (int i = 0; i < totalCount; i++) indexOccupiedArray[i] = false;
        }
        else
        {
            for(int i = 0; i < totalCount; i++)
            {
                if (!indexOccupiedArray[i]) // 如果有空位，则占用该位置
                {
                    indexOccupiedArray[i] = true;
                    currentIndex = i;
                    currentCount++;
                    angleOffset = (i + 1) * angleInterval; // 根据索引设置偏移量
                    break;
                }
            }
        }

        if (currentCount >= totalCount) // 如果砖墙数量已满，则销毁当前对象
        {
            Destroy(gameObject);
            return;
        }

        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;

        transform.SetParent(playerTF);
        PlayerController.Instance.OnlyBrick = this;
        orbitAdjuster = GetComponent<PlayerOrbitPositionAdjuster>();
        orbitAdjuster.constantDst = 1.5f;
    }

    private void Update()
    {
        Vector2 direction = new Vector2(Mathf.Cos(Time.time + angleOffset), Mathf.Sin(Time.time + angleOffset)).normalized;
        direction *= 5f;

        orbitAdjuster.targetPoint = (Vector2)playerTF.position + direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.TryGetComponent(out Enemy enemy) && !enemy.Info.IsDead)
        {
            enemy.Info.Hurt(2);
            Info.Hurt(1);
            if (Info.IsDead)
            {
                PlayerController.Instance.OnlyBrick = null;
                currentCount--;
                indexOccupiedArray[currentIndex] = false; // 释放当前索引位置
                Destroy(gameObject);
            }
        }
    }
}
