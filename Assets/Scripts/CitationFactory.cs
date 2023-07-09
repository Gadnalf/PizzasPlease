using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    string pelpHeader = "PELP CUSTOMER SERVICE TICKET\n" +
        "CUSTOMER ID: 17KR67WK\n" +
        "---------------------\n" +
        "Message from Customer Service\n" +
        "Representative: Giorgio DiGiorno\n" +
        "---------------------\n";

    string[] pelpWarnings = new string[]
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
        "You won't get away this time.\n\n" +
        "Kindly stay put while the Pelp Content " +
        "Moderation and User Termination team " +
        "expunge your location."
    };

    string header = "KPI Report:\n" +
        "Achievement Regression Index\n" +
        "---------------------\n" +
        "New data point received!\n" +
        "Wastefulness/General Incompetence" +
        "Detail:\n";

    public void RegisterCitation(bool pelp, string message = "")
    {
        if (pelp && citationLevel < 4)
        {
            GameObject newCitation = Instantiate(citationPrefab);
            GameObject gameText = newCitation.transform.GetChild(0).GetChild(0).gameObject;
            gameText.GetComponent<TextMeshProUGUI>().text = pelpHeader + pelpWarnings[citationLevel];
            citationLevel++;
            if (currentCitation is null)
            {
                currentCitation = newCitation;
            }
            else
            {
                citationQueue.Enqueue(newCitation);
            }
        } else if (zelp) {
            SceneManager.LoadScene("BadEnding");
        }
        else if (!pelp)
        {
            GameObject newCitation = Instantiate(citationPrefab);
            GameObject gameText = newCitation.transform.GetChild(0).GetChild(0).gameObject;
            gameText.GetComponent<TextMeshProUGUI>().text = header + message;
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
