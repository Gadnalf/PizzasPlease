using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiptTrashBehaviour : MonoBehaviour
{
    public GameObject eventSystem;
    public bool shouldDrop;
    public float pizzaTrashSpeed = 0.9f;

    private AudioManager audioManager;

    private void Start()
    {
        eventSystem = GameObject.Find("EventSystem");
        audioManager = eventSystem.GetComponent<AudioManager>();
    }

    void Update() {
        if (Input.GetMouseButtonUp(0) && shouldDrop) {
            if (eventSystem.GetComponents<NewChallengeSpawner>().Length != 0) {
                NewChallengeSpawner spawner = eventSystem.GetComponents<NewChallengeSpawner>()[0];

                GameObject receipt = spawner.instantiatedReceipt;

                receipt.GetComponent<DraggableObjectBehaviour>().animateSlide(
                                        receipt.transform.position,
                                        new Vector2(receipt.transform.position.x, receipt.transform.position.y - 15),
                                        pizzaTrashSpeed);

                audioManager.PlaySound(audioManager.trashSound, 0.7f);

                // Delay the onGoodReview call to give time for the pizza slide out animation
                // before the pizza gameobject is destroyed
                spawner.Invoke("onTrashReceipt", 1.0f);
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
