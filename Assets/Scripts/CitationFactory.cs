using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CitationFactory : MonoBehaviour
{
    public GameObject citationPrefab;
    public AudioSource audioPlayer;
    public AudioClip citationSound;
    public float printSpeed = 1f;

    private GameObject currentCitation;

    Queue<GameObject> citationQueue = new Queue<GameObject>();

    private int printingProgress;
    int citationLevel = 0;

    string header = "PELP CUSTOMER SERVICE TICKET\n" +
        "CUSTOMER ID: 17KR67WK\n" +
        "---------------------\n" +
        "Message from Customer Service\n" +
        "Representative: Giorgio DiGiorno\n" +
        "---------------------\n";

    string[] warnings = new string[]
    {
        "Your account has been flagged on " +
        "suspicion of fabricating negative " +
        "Pelp reviews.\n\n" +
        "I'm sure it's just " +
        "a system error, but I wanted to check " +
        "in to see if everything is okay?",

        "Hi, your account has been flagged " +
        "once again.\n\n" +
        "I'm sorry to inform " +
        "you that this is your second strike " +
        "and that further infractions may " +
        "result in account termination.",

        "\n...\n\n\n" +
        "Stop. This is your final warning.",

        "I see you.\n" +
        "You can't get away this time.\n\n" +
        "Kindly stay put while the Pelp Content " +
        "Moderation and User Termination team " +
        "expunge your location."
    };

    public void RegisterCitation(bool zelp)
    {
        GameObject newCitation = Instantiate(citationPrefab);
        GameObject gameText = newCitation.transform.GetChild(0).GetChild(0).gameObject;
        if (zelp && citationLevel < 4)
        {
            gameText.GetComponent<TextMeshProUGUI>().text = header + warnings[citationLevel];
            citationLevel++;
            if (currentCitation is null)
            {
                currentCitation = newCitation;
            }
            else
            {
                citationQueue.Enqueue(newCitation);
            }
        }
    }

    public void FixedUpdate()
    {
        if (currentCitation != null) {
            if (currentCitation.GetComponent<DraggableObjectBehaviour>().draggable)
            {
                currentCitation.GetComponent<DraggableObjectBehaviour>().draggable = false;
                if (printingProgress < 3)
                {
                    audioPlayer.PlayOneShot(citationSound);
                    currentCitation.GetComponent<DraggableObjectBehaviour>().animateSlide(currentCitation.transform.position,
                    currentCitation.transform.position + Vector3.up * 2.5f,
                    printSpeed);
                    printingProgress++;
                }
                else
                {
                    currentCitation.GetComponent<DraggableObjectBehaviour>().draggable = true;
                    printingProgress = 0;
                    if (citationQueue.Count > 0)
                    {
                        
                        currentCitation = citationQueue.Dequeue();
                    }
                    else
                    {
                        currentCitation = null;
                    }
                }
            }
        }
    }
}
