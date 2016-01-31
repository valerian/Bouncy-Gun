using UnityEngine;
using System.Collections;

public class PlayManager : MonoBehaviour
{
    public float health { get; private set; }
    public float energy { get; private set; }

    public float healthRatio { get { return health / Game.GameData.healthMax; } }
    public float energyRatio { get { return energy / Game.GameData.energyMax; } }
}
