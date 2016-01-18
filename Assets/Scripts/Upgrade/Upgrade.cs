using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public struct UpgradeBonusSetting
{
    public BONUS_TYPE bonusType;
    public int percentBonus;

    public Bonus instantiate()
    {
        return BonusFactory.CreateInstance(bonusType);
    }
}

public class Upgrade : MonoBehaviour {

    public string upgradeName;
    public UnityEngine.UI.RawImage icon;
    public UpgradeBonusSetting[] bonusses;
    
}
