using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideEffect : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.relativeVelocity.magnitude >= 2f)
        {
            Vector2 leftDir = Vector2.Perpendicular(collision.relativeVelocity.normalized);
            VFXManager.Instance.GenerateCollideVFX(collision.contacts[0].point, leftDir);
            VFXManager.Instance.GenerateCollideVFX(collision.contacts[0].point, -leftDir);

            SFXManager.Instance.PlayCollideSFX();
        }
    }
}
