using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteChanger : MonoBehaviour
{
    public List<Sprite> messageBoxes;
    private int currentSprite = -1;

    public void ChangeImage() {
        int selected = Random.Range(0, messageBoxes.Count);
        if (selected == currentSprite) {
            currentSprite = (currentSprite + 1) % messageBoxes.Count;
        }
        GetComponent<Image>().sprite = messageBoxes[selected];
    }
}
