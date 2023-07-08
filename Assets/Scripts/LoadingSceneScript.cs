using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneScript : MonoBehaviour
{
    public GameObject introText;
    public GameObject positionText;
    public GameObject clickIndicator;
    private bool accept;
    // Start is called before the first frame update
    void Start()
    {
        float delay = introText.GetComponent<TypewriterEffect>().StartTyping();
        StartCoroutine(RenderPositionText(delay));
    }

    IEnumerator RenderPositionText(float delay) {
        yield return new WaitForSeconds(delay);
        float secondDelay = positionText.GetComponent<TypewriterEffect>().StartTyping();
        yield return new WaitForSeconds(secondDelay);
        accept = true;
        clickIndicator.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && accept) {
            Scene scene = SceneManager.GetActiveScene();
            string nextScene = "LoadingScene2";
            if (scene.name == "LoadingScene2") {
                nextScene = "PrimaryPlayScene";
            }
            SceneManager.LoadScene(nextScene);
        }
    }
}
