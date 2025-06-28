using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBehavior : MonoBehaviour
{
    protected UnitInfo Info
    {
        get
        {
            if(info == null)
                info = GetComponent<UnitInfo>();
            return info;
        }
    }
    protected UnitInfo info;
    protected Rigidbody2D body2D;

    protected Transform targetTF;

    [SerializeField] protected float detectRadius = 6f;

    protected virtual void Start()
    {
        info = GetComponent<UnitInfo>();
        body2D = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate()
    {
        if (Info.IsDead) return;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (Info.IsDead) return;
    }
}
