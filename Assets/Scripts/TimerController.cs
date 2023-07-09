using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimerController : MonoBehaviour
{
    float delay = 0.5f;
    int mins = 0;
    int hours = 8;
    
    public GameObject clock;

    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RunClock());
        audioManager = GameObject.Find("EventSystem").GetComponent<AudioManager>();
    }

    // Private method accessible in the editor for testing
    [ContextMenu("Set Time to hour 17")]
    private void SetClockToOneHourLeft()
    {
        hours = 17;
    }

    IEnumerator RunClock() {
        TextMeshProUGUI textComponent = clock.GetComponent<TextMeshProUGUI>();
        while (hours < 18) {
            yield return new WaitForSeconds(delay);
            mins++;
            if (mins >= 60) {
                mins -= 60;
                hours ++;
            }
            textComponent.text = System.String.Format("{0:00}:{1:00}", hours, mins);

            if (hours == 17 && mins == 36)
            {
                audioManager.PlaySound(audioManager.timerTicking, 0.6f);
            }
        }

        int currentScore = GetComponent<NewChallengeSpawner>().GetCurrentScore();

        PlayerPrefs.SetInt("score", currentScore);
        PlayerPrefs.Save();

        SceneManager.LoadScene("EndSceneGood");
    }
}
