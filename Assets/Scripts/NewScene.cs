using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewScene : MonoBehaviour
{
    public float delay = 1.0f;

    public void NextScene() {
        SceneManager.LoadScene("LoadingScene");
    }

    public void NextSceneWithDelay()
    {
        Invoke("NextScene", delay);
    }
}
