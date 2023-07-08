using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewChallengeSpawner : MonoBehaviour
{
    public GameObject pizzaPrefab;
    public GameObject receiptPrefab;
    public GameObject instantiatedPizza;
    public GameObject instantiatedReceipt;
    private int warningPoints;
    public bool currentPizzaGood;
    void Start() {
        orderNewPizza();
    }

    public void onGoodReview() {
        if (!currentPizzaGood) {
            warningPoints++;
            Debug.Log("Uh oh, you have "+ warningPoints + "warnings!");
        }
        onReviewSubmitted();
    }

    public void onBadReview() {
        if (currentPizzaGood) {
            warningPoints++;
            Debug.Log("Uh oh, you have "+ warningPoints + "warnings!");
        }
        onReviewSubmitted();
    }

    public void onReviewSubmitted() {
        Destroy(instantiatedPizza);
        Destroy(instantiatedReceipt);
        orderNewPizza();
    }

    private void orderNewPizza() {
        Vector3 centerPosition = new Vector3(0.5f, 0.5f, -Camera.main.transform.position.z);
        Vector3 centerCameraPosition = Camera.main.ViewportToWorldPoint(centerPosition);
        instantiatedPizza = Instantiate(pizzaPrefab, centerCameraPosition, Quaternion.identity);

        Vector3 transform = new Vector3(5f, 0f, 0f);
        Vector3 offsetCameraPositon = centerCameraPosition + transform;
        instantiatedReceipt = Instantiate(receiptPrefab, offsetCameraPositon, Quaternion.identity);
        Color pizzaColor = new Color(
            Random.Range(0f, 1f), 
            Random.Range(0f, 1f), 
            Random.Range(0f, 1f)
        );
        instantiatedPizza.GetComponent<Renderer>().material.SetColor("_Color", pizzaColor);

        currentPizzaGood = Random.Range(0, 2) != 0;
        if (currentPizzaGood) {
            instantiatedReceipt.GetComponent<Renderer>().material.SetColor("_Color", pizzaColor);
        } else {
            Color receiptColor = new Color(
                Random.Range(0f, 1f), 
                Random.Range(0f, 1f), 
                Random.Range(0f, 1f)
            );
            instantiatedReceipt.GetComponent<Renderer>().material.SetColor("_Color", receiptColor);
        }
    }
}
