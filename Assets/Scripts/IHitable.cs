using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitable
{
    public void OnHit(Vector2 direction);
}
