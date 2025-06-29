using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    [Header("攻击参数")]
    [SerializeField] float attackDst = 3f;
    [SerializeField] float dashPower = 50f;
    [SerializeField] float attackPrepTime = 0.5f;
    [SerializeField] float attackInterval = 1f;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!canMove) return;
        if (Info.IsDead) return;

        Vector2 targetOffset = targetPoint - (Vector2)transform.position;
        if (TargetIsPlayer && targetOffset.magnitude <= attackDst)
        {
            StartCoroutine(Dash(targetOffset.normalized));
        }
        else if(targetOffset.magnitude > 0.2f && targetOffset.magnitude < DetectRadius)
        {
            body2D.AddForce(14f * moveSpeed * targetOffset.normalized);
        }
    }

    private IEnumerator Dash(Vector2 direction)
    {
        canMove = false;
        yield return new WaitForSeconds(attackPrepTime);

        if (Info.IsDead) yield break; // 如果在攻击准备期间死亡，则不执行冲刺

        SFXManager.Instance.PlayDashSFX(); // 播放冲刺音效
        body2D.AddForce(direction * dashPower, ForceMode2D.Impulse);
        yield return new WaitForSeconds(attackInterval);
        canMove = true;
        //targetUpdateTimer = targetUpdateInterval; // 攻击之后立即寻路
    }
}
