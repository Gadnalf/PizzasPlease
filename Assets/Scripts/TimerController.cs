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
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RunClock());
    }

    IEnumerator RunClock() {
        TextMeshProUGUI textComponent = clock.GetComponent<TextMeshProUGUI>();
        while (hours < 9) {
            yield return new WaitForSeconds(delay);
            mins++;
            if (mins >= 60) {
                mins -= 60;
                hours ++;
            }
            textComponent.text = System.String.Format("{0:00}:{1:00}", hours, mins);
        }

        SceneManager.LoadScene("EndSceneGood");
    }
}
