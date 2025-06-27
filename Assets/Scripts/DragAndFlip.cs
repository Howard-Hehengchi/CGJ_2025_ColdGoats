using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndFlip : MonoBehaviour
{
    [SerializeField, Range(0f, 1f), Tooltip("在尚未完全翻转时预判是否翻转，实为transform.forward.z的值，范围(0, 1)")]
    private float flipTolerance = 0.45f;

    private bool isUp = true;

    private bool mouseOver = false;

    private Vector3 mouseStartPos;

    private void Start()
    {
        mouseStartPos = Vector3.forward;
        isUp = true;
        mouseOver = false;
        transform.localEulerAngles = Vector3.zero;
    }

    private void OnMouseEnter()
    {
        mouseOver = true;
    }

    private void OnMouseExit()
    {
        mouseOver = false;
    }

    private void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(1) && mouseOver)
        {
            mouseStartPos = mousePosition;
        }

        if (Input.GetMouseButtonUp(1))
        {
            mouseStartPos = Vector3.forward;

            if (isUp && transform.forward.z < flipTolerance)
            {
                isUp = false;
            }
            else if(!isUp && transform.forward.z > -flipTolerance)
            {
                isUp = true;
            }
        }

        Vector3 targetForward = isUp ? Vector3.forward : Vector3.back;

        if (Input.GetMouseButton(1) && mouseStartPos.z == 0f)
        {
            float deltaX = mousePosition.x - mouseStartPos.x;

            float angle = ClampAngle((isUp ? 0f : 180f) + deltaX * 30f);

            transform.localEulerAngles = new Vector3(0f, angle, 0f);
        }
        else if(Vector3.Angle(transform.forward, targetForward) > 0.1f)
        {
            transform.forward = Vector3.Slerp(transform.forward, targetForward, Time.deltaTime * 10f);
        }
    }

    private float ClampAngle(float angle)
    {
        while(Mathf.Abs(angle) > 180f)
        {
            angle -= Mathf.Sign(angle) * 360f;
        }

        return angle;
    }
}
