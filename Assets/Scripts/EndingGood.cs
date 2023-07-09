using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingGood : MonoBehaviour
{
    public GameObject congratsText;
    public GameObject salaryText;
    public GameObject scoreText;
    public GameObject clickIndicator1;
    public GameObject clickIndicator2;
    public GameObject clickIndicator3;
    private bool accept1;
    private bool accept2;
    private bool accept3;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RenderCongrats());
    }

    IEnumerator RenderCongrats() {
        float delay = congratsText.GetComponent<TypewriterEffect>().StartTyping();
        yield return new WaitForSeconds(delay);
        clickIndicator1.SetActive(true);
        accept1 = true;
    }

    IEnumerator RenderSalaryText() {
        float delay = salaryText.GetComponent<TypewriterEffect>().StartTyping();
        yield return new WaitForSeconds(delay);
        clickIndicator2.SetActive(true);
        accept2 = true;
    }

    IEnumerator RenderScoreText() {
        float delay = scoreText.GetComponent<TypewriterEffect>().StartTyping();
        yield return new WaitForSeconds(delay);
        clickIndicator3.SetActive(true);
        accept3 = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && accept1) {
            clickIndicator1.SetActive(false);
            accept1 = false;
            StartCoroutine(RenderSalaryText());
        }
        if (Input.GetMouseButtonDown(0) && accept2) {
            clickIndicator2.SetActive(false);
            accept2 = false;
            StartCoroutine(RenderScoreText());
        }
        if (Input.GetMouseButtonDown(0) && accept2) {
            clickIndicator3.SetActive(false);
            accept3 = false;
            Application.Quit();
        }
    }
}