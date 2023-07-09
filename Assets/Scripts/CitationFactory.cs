using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CitationFactory : MonoBehaviour
{
    public GameObject citationPrefab;

    public float printSpeed = 1f;

    private GameObject currentCitation;
    private AudioManager audioManager;

    Queue<GameObject> citationQueue = new Queue<GameObject>();

    private int printingProgress;
    private int currentLayerOffset = 101;
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
        "result in termination.",

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
        "New data point received in category: " +
        "Wastefulness/Incompetence\n" +
        "---------------------\n" +
        "Detail: ";

    private void Start()
    {
        audioManager = GameObject.Find("EventSystem").GetComponent<AudioManager>();
    }

    public void RegisterCitation(bool pelp, string message = "")
    {
        GameObject newCitation = Instantiate(citationPrefab);
        GameObject gameText = newCitation.transform.GetChild(0).GetChild(0).gameObject;
        newCitation.GetComponent<Renderer>().sortingOrder = currentLayerOffset;
        newCitation.transform.GetChild(0).GetComponent<Canvas>().sortingOrder = currentLayerOffset;
        currentLayerOffset++;
        if (pelp && citationLevel < 4) {
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
        } else if (pelp) {
            SceneManager.LoadScene("EndSceneBad");
        }
        else
        {
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
                    audioManager.PlaySound(audioManager.citationSound, 0.25f);
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
