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
        text.enabled = !GameManager.instance.playing;
    }
}
