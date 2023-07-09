using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    private float delay = 0.05f;
    public string fullText;
    private string currentText = "";

    public float StartTyping() {
        StartCoroutine(ShowText());
        return fullText.Length * delay;
    }

    IEnumerator ShowText() {
        for (int i = 0; i <= fullText.Length; i++) {
            currentText = fullText.Substring(0, i);
            this.GetComponent<TextMeshProUGUI>().text = currentText;
            if (i != 0 && fullText[i - 1] == '.') {
                yield return new WaitForSeconds(delay + 0.5f);
            } else {
                yield return new WaitForSeconds(delay);
            }
        }
    }
}
