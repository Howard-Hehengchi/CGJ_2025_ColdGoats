using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFlip : MonoBehaviour
{
    private bool isUp = true;

    private void Start()
    {
        isUp = true;
    }

    public void DoFlip(float horizontalDir)
    {
        isUp = !isUp;
    }

    private void Update()
    {
        Vector3 targetForward = isUp ? Vector3.forward : Vector3.back;
        if (Vector3.Angle(transform.forward, targetForward) > 0.1f)
        {
            transform.forward = Vector3.Slerp(transform.forward, targetForward, Time.deltaTime * 10f);
        }
    }
}
