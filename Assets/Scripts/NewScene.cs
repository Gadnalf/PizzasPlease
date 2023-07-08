using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewScene : MonoBehaviour
{
    public void NextScene() {
        SceneManager.LoadScene("LoadingScene");
    }
}
