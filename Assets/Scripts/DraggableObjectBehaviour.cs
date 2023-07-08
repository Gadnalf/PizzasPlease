using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableObjectBehaviour : MonoBehaviour
{
    public GameObject selectedObject;
    Vector3 offset;
    Vector3 selectionOffset = new Vector3(0f, 0f, 5f);

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0)) {
            Collider2D[] targetObjects = Physics2D.OverlapPointAll(mousePosition);
            if (targetObjects.Length > 0)
            {
                GameObject hoveredObject = targetObjects[0].transform.gameObject;
                if (hoveredObject == this.gameObject) {
                    selectedObject = hoveredObject;
                    offset = selectedObject.transform.position - mousePosition - selectionOffset;
                }
                
            }
        }
        if (selectedObject)
        {
            Vector3 newPos = mousePosition + offset;
            if (CheckBorders(newPos)) {
                selectedObject.transform.position = mousePosition + offset;                
            }
        }
        if (Input.GetMouseButtonUp(0) && selectedObject)
        {
            selectedObject.transform.position += selectionOffset;
            selectedObject = null;
        }
    }

    private bool CheckBorders(Vector3 newPos) {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(newPos);
        return viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0;
    }
}
