using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    [SerializeField] SpriteRenderer barSpRenderer;

    private const float sizeMinValue = 0.3f;
    private const float sizeMaxValue = 1.0f;

    private const float posMinValue = -0.35f;
    private const float posMaxValue = 0f;

    private Vector2 defaultPosition = new Vector2(0f, 0.7f);
    private Vector2 defaultScale = new Vector2(1f, 1f);

    private Vector2 targetPosition;
    private Vector2 targetScale;

    //[SerializeField, Range(0f, 1f)] private float testValue = 1f;

    //private void OnValidate()
    //{
    //    SetValue(testValue);
    //}

    private void Start()
    {
        targetPosition = defaultPosition;
        targetScale = defaultScale;
    }

    private void Update()
    {
        if(Vector2.Distance(transform.localPosition, targetPosition) > 0.01f)
        {
            transform.localPosition = Vector2.Lerp(transform.localPosition, targetPosition, Time.deltaTime * 10f);
        }
        if(Vector2.Distance(transform.localScale, targetScale) > 0.01f)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, targetScale, Time.deltaTime * 10f);
        }
    }

    public void SetValue(float value)
    {
        if (value == 0f)
        {
            barSpRenderer.gameObject.SetActive(false);
            Hide();
        }
        else 
        {
            Show();
            if (barSpRenderer.gameObject.activeSelf == false)
            {
                barSpRenderer.gameObject.SetActive(true);
            }
        } 

        barSpRenderer.size = new Vector2(Mathf.Lerp(sizeMinValue, sizeMaxValue, value), barSpRenderer.size.y);
        barSpRenderer.transform.localPosition = new Vector3(Mathf.Lerp(posMinValue, posMaxValue, value), barSpRenderer.transform.localPosition.y, barSpRenderer.transform.localPosition.z);
    }

    public void Show()
    {
        targetPosition = defaultPosition;
        targetScale = defaultScale;
    }

    public void Hide()
    {
        targetPosition = Vector2.zero;
        targetScale = Vector2.zero;
    }
}
