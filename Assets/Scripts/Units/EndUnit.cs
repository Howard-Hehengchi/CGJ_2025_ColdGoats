using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndUnit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.PlayerSucceed();
    }
}
