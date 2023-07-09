using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class NewChallengeSpawner : MonoBehaviour
{
    public GameObject instantiatedPizza;
    public GameObject instantiatedReceipt;
    public GameObject alert;
    public GameObject scoreText;
    public GameObject streakText;

    private PizzaFactory pizzaFactory;
    private int warningPoints;
    public static int score;
    private int streak;
    public bool currentPizzaGood;
    void Start() {
        pizzaFactory = GetComponent<PizzaFactory>();

        OrderNewPizza();
        if (alert) {
            alert.SetActive(false);
        }
    }

    public void Review(bool incorrect){
        if (incorrect) {
            streak = 0;
            score -= 500;
            StartCoroutine(ErroredJudgement());
            streakText.GetComponent<TextMeshProUGUI>().faceColor = Color.red;
            streakText.GetComponent<TextMeshProUGUI>().text = "- 500";
        } else {
            streak = Math.Min(streak + 1, 3);
            score += 100*streak;
            streakText.GetComponent<TextMeshProUGUI>().faceColor = Color.green;
            streakText.GetComponent<TextMeshProUGUI>().text = "+ " + (streak*100).ToString();
        }
        scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + score.ToString();
        StartCoroutine(ShowStreak());
        OnReviewSubmitted();
    }

    public void onGoodReview() {
        Review(!currentPizzaGood);
    }

    public void onBadReview() {
        Review(currentPizzaGood);
    }

    public void OnReviewSubmitted() {
        Destroy(instantiatedPizza);
        Destroy(instantiatedReceipt);
        OrderNewPizza();
    }

    IEnumerator ShowStreak(){
        streakText.GetComponent<TextMeshProUGUI>().enabled = true;
        yield return new WaitForSeconds(2f);
        streakText.GetComponent<TextMeshProUGUI>().enabled = false;
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
