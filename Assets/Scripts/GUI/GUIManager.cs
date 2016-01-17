using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIManager : MonoBehaviour {

    public static GUIManager instance;

    public Canvas canvas;

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
    public Text gameOverScore;

    public GameObject healthPanel;
    public GameObject energyPanel;
    public GameObject chargePanel;
    public GameObject energyThreshold;

    public GameObject damageText;


    private bool nextLevelUIActive = false;
    private bool gameOverUIActive = false;

    void Awake()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;
    }

    void Destroy()
    {
        instance = null;
    }

    public void DamageText(Vector3 worldPosition, int damage, Color color)
    {
        SimplePool.Spawn(damageText, worldPosition, Quaternion.identity).GetComponent<TextMesh>().text = "- " + damage;
    }

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
        energyThreshold.SendMessage("UpdatePosition", GameManager.instance.energyPerShot / GameManager.instance.energyMax);
        healthPanel.SendMessage("UpdateFill", GameManager.instance.health / GameManager.instance.healthMax);
        energyPanel.SendMessage("UpdateFill", GameManager.instance.energy / GameManager.instance.energyMax);
        energyPanel.SendMessage("SpecialColor", GameManager.instance.energy < GameManager.instance.energyPerShot);
        if (GameManager.instance.isCharged || GameManager.instance.isCharging)
            chargePanel.SendMessage("UpdateFill", ((Time.time - GameManager.instance.chargingStartTime) / GameManager.instance.fireRate));
        else
            chargePanel.SendMessage("UpdateFill", 0f);

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
            gameOverUIActive = true;
            gameOverPanel.SetActive(true);
            gameOverScore.text = string.Format("{0:# ###0}", GameManager.instance.score);
        }
    }
}
