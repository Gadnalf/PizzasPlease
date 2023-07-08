using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaTrashBehaviour : MonoBehaviour
{
    public GameObject eventSystem;
    public bool shouldDrop;
    void Update() {
        if (Input.GetMouseButtonUp(0) && shouldDrop) {
            if (eventSystem.GetComponents<NewChallengeSpawner>().Length != 0) {
                NewChallengeSpawner spawner = eventSystem.GetComponents<NewChallengeSpawner>()[0];
                spawner.onGoodReview();
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
