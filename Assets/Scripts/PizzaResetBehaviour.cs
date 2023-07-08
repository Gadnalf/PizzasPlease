using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaResetBehaviour : MonoBehaviour
{
    private Vector3 centerPosition = new Vector3(0.5f, 0.5f, 0);
    public void OnReviewSubmitted() {
        Vector3 centerCameraPosition = Camera.main.ViewportToWorldPoint(centerPosition);
        transform.position = centerCameraPosition;
        Destroy(this.gameObject);
    }
}
