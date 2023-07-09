using System.Collections;
using UnityEngine;

public class NewChallengeSpawner : MonoBehaviour
{
    public GameObject instantiatedPizza;
    public GameObject instantiatedReceipt;
    public GameObject alert;

    private PizzaFactory pizzaFactory;
    private int warningPoints;
    public bool currentPizzaGood;
    void Start() {
        pizzaFactory = GetComponent<PizzaFactory>();

        OrderNewPizza();
        if (alert) {
            alert.SetActive(false);
        }
    }

    public void onGoodReview() {
        if (!currentPizzaGood) {
            StartCoroutine(ErroredJudgement());
        }
        OnReviewSubmitted();
    }

    public void onBadReview() {
        if (currentPizzaGood) {
            StartCoroutine(ErroredJudgement());
        }
        OnReviewSubmitted();
    }

    public void OnReviewSubmitted() {
        Destroy(instantiatedPizza);
        Destroy(instantiatedReceipt);
        OrderNewPizza();
    }

    IEnumerator ErroredJudgement() {
        warningPoints++;
        Debug.Log("Uh oh, you have "+ warningPoints + " warnings!");
        alert.SetActive(true);
        yield return new WaitForSeconds(3f);
        alert.SetActive(false);
    }

    public void OrderNewPizza() {
        PizzaFactory.PizzaOrder order = pizzaFactory.GenerateNewPizzaOrder();
        PizzaFactory.GeneratedPizza pizza = pizzaFactory.CreatePizza(order);

        instantiatedPizza = pizza.Pizza;
        instantiatedReceipt = pizza.Receipt;
        currentPizzaGood = pizza.CurrentPizzaGood;
    }
}
