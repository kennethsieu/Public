using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    private bool isDragging = false;
    private bool isOverDropZone = false;
    private Vector2 startPosition;
    private GameObject target;


    void Update()
    {
        if(isDragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (this.gameObject.GetComponent<Card>().validUsage(collision))
        {
            isOverDropZone = true;
            target = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isOverDropZone = false;
    }

    public void StartDrag()
    {
        startPosition = transform.position;
        isDragging = true;
    }

    public void EndDrag()
    {
        isDragging = false;
        if (isOverDropZone)
        {
            this.gameObject.GetComponent<Card>().cardAction(target);
            Destroy(this.gameObject);
        } else
        {
            transform.position = startPosition;
        }
    }
}
