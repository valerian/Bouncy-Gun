using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class GUIManager : MonoBehaviour {

    public static GUIManager instance;

    public Canvas canvas;

    public GameObject bonusPanel;
    public GameObject gameOverPanel;
    public BonusButton[] bonusButtons;

    public Text levelText;
    public Text healthText;
    public Text energyText;
    public Text energyRegenText;
    public Text bulletCostText;
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
        healthText.text = Mathf.RoundToInt(GameManager.instance.health) + " HP";
        energyText.text = Mathf.RoundToInt(GameManager.instance.energy) + " E";
        energyRegenText.text = Mathf.RoundToInt(GameManager.instance.energyRegen) + " E/s";
        bulletCostText.text = Mathf.RoundToInt(GameManager.instance.energyPerShot) + " E";
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
            Array.ForEach(bonusButtons, b => b.GenerateBonusses());
        }

        if (nextLevelUIActive && GameManager.instance.isPlaying)
        {
            nextLevelUIActive = false;
            bonusPanel.SetActive(false);
        }

        if (!gameOverUIActive && !GameManager.instance.isPlaying && !GameManager.instance.isLevelCleared)
        {
            gameOverUIActive = true;
            gameOverPanel.SetActive(true);
            gameOverScore.text = string.Format("{0:# ###0}", GameManager.instance.score);
        }
    }
}
