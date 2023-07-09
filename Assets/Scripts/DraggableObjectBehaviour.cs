using UnityEngine;

public class DraggableObjectBehaviour : MonoBehaviour
{
    public bool draggable = true;
    public GameObject selectedObject;
    Vector3 offset;
    Vector3 selectionOffset = new Vector3(0f, 0f, 5f);

    private bool sliding;
    private bool spherical;
    private float speed;
    private Vector2 start;
    private Vector2 end;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Animate the slide with linear interpolation
    /// </summary>
    public void animateSlide(Vector2 start, Vector2 end, float speed)
    {
        animateSlide(start, end, speed, false);
    }

    /// <summary>
    /// Animate the slide with spherical interpolation
    /// </summary>
    public void animateSlide(Vector2 start, Vector2 end, float speed, bool spherical)
    {
        transform.position = start;
        this.start = start;
        this.end = end;
        this.speed = speed;
        this.spherical = spherical;  // Spherical interpolation vs linear interpolation
        sliding = true;
        draggable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (draggable)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))
            {
                Collider2D[] targetObjects = Physics2D.OverlapPointAll(mousePosition);
                if (targetObjects.Length > 0)
                {
                    GameObject hoveredObject = targetObjects[0].transform.gameObject;
                    if (hoveredObject == this.gameObject)
                    {
                        selectedObject = hoveredObject;
                        offset = selectedObject.transform.position - mousePosition - selectionOffset;
                    }

                }
            }
            if (selectedObject)
            {
                Vector3 newPos = mousePosition + offset;
                if (CheckBorders(newPos))
                {
                    selectedObject.transform.position = mousePosition + offset;
                }
            }
            if (Input.GetMouseButtonUp(0) && selectedObject)
            {
                selectedObject.transform.position += selectionOffset;
                selectedObject = null;
            }
        }
    }

    private void FixedUpdate()
    {
        if (sliding)
        {
            if (spherical)
            {
                rb.MovePosition(Vector3.Slerp(transform.position, end, speed * Time.fixedDeltaTime));
            }
            else
            {    
                rb.MovePosition(Vector3.Lerp(transform.position, end, speed * Time.fixedDeltaTime));
            }
            if ((end - (Vector2)transform.position).magnitude < 0.5f)
            {
                sliding = false;
                draggable = true;
            }
        }
    }

    private bool CheckBorders(Vector3 newPos) {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(newPos);
        return viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0;
    }
}
