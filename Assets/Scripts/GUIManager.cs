using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIManager : MonoBehaviour {

    public GameObject bonusPanel;
    public GameObject gameOverPanel;
    public Button bonusButton1;
    public Button bonusButton2;
    public Button bonusButton3;
    public Button bonusButton4;

    public Text levelText;
    public Text healthText;
    public Text energyText;
    public Text energyRegenText;
    public Text bulletCostText;

    private bool nextLevelUIActive = false;
    private bool gameOverUIActive = false;
    
    void OnGUI()
    {
        levelText.text = "Level " + GameManager.instance.currentLevel;
        healthText.text = Mathf.RoundToInt(GameManager.instance.health) + " HP";
        energyText.text = Mathf.RoundToInt(GameManager.instance.energy) + " E";
        energyRegenText.text = Mathf.RoundToInt(GameManager.instance.energyRegen) + " E/s";
        bulletCostText.text = Mathf.RoundToInt(GameManager.instance.energyPerShot) + " E";
    }

    // Update is called once per frame
    void Update () {
        if (!nextLevelUIActive && !GameManager.instance.playing && GameManager.instance.levelCleared)
        {
            nextLevelUIActive = true;
            bonusPanel.SetActive(true);
            bonusButton1.SendMessage("GenerateBonusses");
            bonusButton2.SendMessage("GenerateBonusses");
            bonusButton3.SendMessage("GenerateBonusses");
            bonusButton4.SendMessage("GenerateBonusses");
        }

        if (nextLevelUIActive && GameManager.instance.playing)
        {
            nextLevelUIActive = false;
            bonusPanel.SetActive(false);
        }

        if (!gameOverUIActive && !GameManager.instance.playing && !GameManager.instance.levelCleared)
        {
            Debug.Log("GAME OVER");
            gameOverUIActive = true;
            gameOverPanel.SetActive(true);
        }
    }
}
