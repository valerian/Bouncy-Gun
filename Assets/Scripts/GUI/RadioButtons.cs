using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RadioButtons : MonoBehaviour
{
    public Toggle[] toggles;
    [HideInInspector]
    public int currentSelected = -1;
    private bool toggleLock = false;

    void Start()
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            int iValue = i;
            toggles[i].isOn = (i == currentSelected);
            toggles[i].onValueChanged.AddListener((value) => OnToggle(iValue, value));
        }
    }

    void OnToggle(int toggleIndex, bool isOn)
    {
        if (!isOn)
        {
            if (!toggleLock)
                toggles[toggleIndex].isOn = true;
            return;
        }
        toggleLock = true;
        currentSelected = toggleIndex;
        for (int i = 0; i < toggles.Length; i++)
            if (i != toggleIndex)
                toggles[i].isOn = false;
        toggleLock = false;
    }
}
