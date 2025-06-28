using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOrbitPositionAdjuster : MonoBehaviour
{
    private float constantDst = 0f;

    [SerializeField] bool needRotation = true;
    [SerializeField] float constantDepthOffset = 0f;

    private void Start()
    {
        constantDst = transform.localPosition.y;
    }

    private void Update()
    {
        Vector2 mouseOffset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.parent.position;
        Vector2 targetPos = mouseOffset.normalized * constantDst;
        transform.localPosition = new Vector3(targetPos.x, targetPos.y, constantDepthOffset);

        if (needRotation)
        {
            float angle = Vector2.SignedAngle(Vector2.up, mouseOffset);
            transform.localEulerAngles = new Vector3(0f, 0f, angle);
        }
    }
}
