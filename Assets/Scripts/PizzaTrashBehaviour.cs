using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaTrashBehaviour : MonoBehaviour
{
    public GameObject eventSystem;
    public bool shouldDrop;
    public float pizzaTrashSpeed = 0.9f;

    private void Start()
    {
        eventSystem = GameObject.Find("EventSystem");
    }

    void Update() {
        if (Input.GetMouseButtonUp(0) && shouldDrop) {
            if (eventSystem.GetComponents<NewChallengeSpawner>().Length != 0) {
                NewChallengeSpawner spawner = eventSystem.GetComponents<NewChallengeSpawner>()[0];

                GameObject pizza = spawner.instantiatedPizza;

                pizza.GetComponent<DraggableObjectBehaviour>().animateSlide(
                                        pizza.transform.position,
                                        new Vector2(pizza.transform.position.x, pizza.transform.position.y - 15),
                                        pizzaTrashSpeed);

                // Delay the onGoodReview call to give time for the pizza slide out animation
                // before the pizza gameobject is destroyed
                spawner.Invoke("onTrashPizza", 1.0f);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        shouldDrop = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        shouldDrop = false;
    }
}
