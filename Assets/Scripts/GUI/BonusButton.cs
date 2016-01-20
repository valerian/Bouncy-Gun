using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BonusButton : MonoBehaviour {

    private Bonus bonus1;
    public BonusDisplay bonusDisplay1;

    private Bonus bonus2;
    public BonusDisplay bonusDisplay2;

    private Bonus bonus3;
    public BonusDisplay bonusDisplay3;

    public void ApplyBonussesAndLoadNextLevel()
    {
        bonus1.Apply();
        bonus2.Apply();
        bonus3.Apply();
        GameManager.instance.NextLevel();
    }

    public void GenerateBonusses()
    {
        Bonus[] bonusses = GameManager.instance.bonusManager.GetRandomUpgrade(2, 1, 2, 3);
        bonus1 = bonusses[0];
        bonus2 = bonusses[1];
        bonus3 = bonusses[2];
        bonusDisplay1.DisplayBonus(bonus1);
        bonusDisplay2.DisplayBonus(bonus2);
        bonusDisplay3.DisplayBonus(bonus3);
    }
}
