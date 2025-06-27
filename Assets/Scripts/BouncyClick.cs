using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyClick : MonoBehaviour
{
    private float originalSize = 1f;
    [SerializeField, Range(0f, 1f)] float shrinkFactor = 0.85f;
    private float shrinkSize;
    [SerializeField] float animSpeed = 15f;

    private bool focused = false;

    private void Start()
    {
        originalSize = transform.localScale.x;
        shrinkSize = originalSize * shrinkFactor;
    }

    private void OnMouseDown()
    {
        focused = true;
        if(SFXManager.Instance != null)
            SFXManager.Instance.PlayPressSFX();
    }

    private void OnMouseUp()
    {
        focused = false;
    }


    private void Update()
    {
        float currentSize = 0f;
        if (focused)
        {
            currentSize = Mathf.Lerp(transform.localScale.x, shrinkSize, animSpeed * Time.deltaTime);
            transform.localScale = new Vector2(currentSize, currentSize);
        }
        else
        {
            currentSize = Mathf.Lerp(transform.localScale.x, originalSize, animSpeed * Time.deltaTime);
            transform.localScale = new Vector2(currentSize, currentSize);
        }
    }
}
