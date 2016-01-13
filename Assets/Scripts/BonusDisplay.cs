using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BonusDisplay : MonoBehaviour {

    public GameObject DescriptionText;
    public GameObject ValueText;
    
    void DisplayBonus(Bonus bonus)
    {
        DescriptionText.GetComponent<Text>().text = bonus.GetDescriptionText();
        ValueText.GetComponent<Text>().text = bonus.GetValueText();
        if (bonus.isMalus)
            ValueText.GetComponent<Text>().color = new Color(1.0f, 0.1f, 0.1f);
        else
            ValueText.GetComponent<Text>().color = new Color(0.1f, 1.0f, 0.1f);
    }
}
