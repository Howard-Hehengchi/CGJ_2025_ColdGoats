using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOrbitPositionAdjuster : MonoBehaviour
{
    [HideInInspector]
    public float constantDst = 0f;

    [SerializeField] bool needRotation = true;
    [SerializeField] float constantDepthOffset = 0f;
    [SerializeField] bool controlledByMouse = true;

    [HideInInspector] public Vector2 targetPoint = Vector2.zero;

    private void Awake()
    {
        constantDst = transform.localPosition.y;
    }

    private void Update()
    {
        if (controlledByMouse)
        {
            targetPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        Vector2 offset = targetPoint - (Vector2)transform.parent.position;
        Vector2 targetPos = offset.normalized * constantDst;
        transform.localPosition = new Vector3(targetPos.x, targetPos.y, constantDepthOffset);

        if (needRotation)
        {
            float angle = Vector2.SignedAngle(Vector2.up, offset);
            transform.localEulerAngles = new Vector3(0f, 0f, angle);
        }
    }
}
