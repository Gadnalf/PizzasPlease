using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewChallengeSpawner : MonoBehaviour
{
    public GameObject instantiatedPizza;
    public GameObject instantiatedReceipt;

    public PizzaFactory pizzaFactory;
    private int warningPoints;
    public bool currentPizzaGood;
    void Start() {
        OrderNewPizza();
    }

    public void onGoodReview() {
        if (!currentPizzaGood) {
            warningPoints++;
            Debug.Log("Uh oh, you have "+ warningPoints + "warnings!");
        }
        OnReviewSubmitted();
    }

    public void onBadReview() {
        if (currentPizzaGood) {
            warningPoints++;
            Debug.Log("Uh oh, you have "+ warningPoints + "warnings!");
        }
        OnReviewSubmitted();
    }

    public void OnReviewSubmitted() {
        Destroy(instantiatedPizza);
        Destroy(instantiatedReceipt);
        OrderNewPizza();
    }

    public void OrderNewPizza() {
        PizzaFactory.PizzaOrder order = pizzaFactory.GenerateNewPizzaOrder();
        PizzaFactory.GeneratedPizza pizza = pizzaFactory.CreatePizza(order);
        instantiatedPizza = pizza.Pizza;
        instantiatedReceipt = pizza.Receipt;
        currentPizzaGood = pizza.CurrentPizzaGood;
    }
}
