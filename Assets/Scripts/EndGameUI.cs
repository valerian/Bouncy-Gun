using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndGameUI : MonoBehaviour {

    private Text _text = null;
    private Text text
    {
        get
        {
            if (_text == null)
                _text = GetComponent<Text>();
            return _text;
        }
    }

    void Awake()
    {
        text.enabled = false;
    }

    // Update is called once per frame
    void Update ()
    {
        if (GameManager.instance.playing)
        {
            text.enabled = false;
            return;
        }
        text.enabled = true;
        text.text = GameManager.instance.levelCleared ? "Level Cleared" : "Game Over";
    }
}
