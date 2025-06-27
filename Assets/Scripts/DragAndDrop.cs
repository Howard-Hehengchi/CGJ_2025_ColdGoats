using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Vector2 mouseOffset;

    private List<SpriteRenderer> spRenderers;
    private Collider2D itemCollider;

    private void Start()
    {
        spRenderers = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
        itemCollider = GetComponent<Collider2D>();
    }

    private void OnMouseDown()
    {
        mouseOffset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        foreach (var spRenderer in spRenderers)
        {
            spRenderer.sortingOrder = 1000;
        }
        itemCollider.isTrigger = true;

        transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y, Random.Range(-10f, 10f));
    }

    private void OnMouseDrag()
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - mouseOffset;
    }

    private void OnMouseUp()
    {
        itemCollider.isTrigger = false;
        foreach (var spRenderer in spRenderers)
        {
            spRenderer.sortingOrder = 0;
        }

        transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y, 0f);
    }
}
