using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class GUIManager : MonoBehaviour {

    public static GUIManager instance;

    public Canvas canvas;

    public GameObject[] helpGUI;
    public GameObject bonusPanel;
    public GameObject gameOverPanel;
    public BonusButton[] bonusButtons;

    public Text levelText;
    public Text scoreText;
    public Text healthText;
    public Text healthMaxText;
    public Text energyText;
    public Text energyMaxText;
    public Text energyRegenText;
    public Text bulletCostText;
    public Text bulletSpeedText;
    public Text bulletMassText;
    public Text bulletDiameterText;
    public Text bulletChargeTimeText;
    public Text gameOverScore;

    public UIVerticalBar healthPanel;
    public UIVerticalBar energyPanel;
    public UIVerticalBar chargePanel;
    public UIVerticalCursor energyThreshold;

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
        scoreText.text = "Score " + string.Format("{0:# ###0}", GameManager.instance.score);
        healthText.text = Mathf.RoundToInt(GameManager.instance.health).ToString();
        healthMaxText.text = "/" + Mathf.RoundToInt(GameManager.instance.healthMax);
        energyText.text = Mathf.RoundToInt(GameManager.instance.energy).ToString();
        energyMaxText.text = "/" + Mathf.RoundToInt(GameManager.instance.energyMax);
        energyRegenText.text = "+" + GameManager.instance.energyRegen.ToString("F1") + "/s";

        bulletCostText.text = (GameManager.instance.energyPerShot).ToString("F0");
        bulletDiameterText.text = (GameManager.instance.bulletSize * 10).ToString("F0");
        bulletChargeTimeText.text = (GameManager.instance.fireRate * 10).ToString("F0");
        bulletMassText.text = (GameManager.instance.bulletMass * 10).ToString("F0");
        bulletSpeedText.text = (GameManager.instance.bulletVelocity / 100f).ToString("F0");
    }

    // Update is called once per frame
    void Update () {
        healthPanel.UpdateFill(GameManager.instance.health / GameManager.instance.healthMax);
        energyPanel.UpdateFill(GameManager.instance.energy / GameManager.instance.energyMax);
        energyPanel.SpecialColor(GameManager.instance.energy < GameManager.instance.energyPerShot);
        energyThreshold.UpdatePosition(GameManager.instance.energyPerShot / GameManager.instance.energyMax);

        if (GameManager.instance.isCharged || GameManager.instance.isCharging)
            chargePanel.UpdateFill((Time.time - GameManager.instance.chargingStartTime) / GameManager.instance.fireRate);
        else
            chargePanel.UpdateFill(0f);

        if (!nextLevelUIActive && !GameManager.instance.isPlaying && GameManager.instance.isLevelCleared)
        {
            nextLevelUIActive = true;
            bonusPanel.SetActive(true);
            Array.ForEach(helpGUI, b => b.SetActive(true));
            Array.ForEach(bonusButtons, b => b.GenerateBonusses());
        }

        if (nextLevelUIActive && GameManager.instance.isPlaying)
        {
            nextLevelUIActive = false;
            Array.ForEach(helpGUI, b => b.SetActive(false));
            bonusPanel.SetActive(false);
        }

        if (!gameOverUIActive && !GameManager.instance.isPlaying && !GameManager.instance.isLevelCleared)
        {
            gameOverUIActive = true;
            gameOverPanel.SetActive(true);
            gameOverScore.text = string.Format("{0:# ###0}", GameManager.instance.score);
            Array.ForEach(helpGUI, b => b.SetActive(true));
        }
    }
}
