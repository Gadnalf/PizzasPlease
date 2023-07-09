using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private int score;
    // Start is called before the first frame update

    private string positiveScoreText = "As specified in the contract, you are not getting a salary. However, your efforts have earned you a bonus of:";
    private string negativeScoreText = "As specified in the contract, you are not getting a salary. In addition, your poor performance means you owe the company: ";
    void Start()
    {
        score = PlayerPrefs.GetInt("score");
        if (score < 0) {
            salaryText.GetComponent<TypewriterEffect>().fullText = negativeScoreText;
            scoreText.GetComponent<TypewriterEffect>().fullText = "$" + (-score).ToString();
        } else {
            salaryText.GetComponent<TypewriterEffect>().fullText = positiveScoreText;
            scoreText.GetComponent<TypewriterEffect>().fullText = "$" + score.ToString();
        }
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
        if (Input.GetMouseButtonDown(0) && accept3) {
            clickIndicator3.SetActive(false);
            accept3 = false;
            SceneManager.LoadScene("TitleScreen");
        }
    }
}
