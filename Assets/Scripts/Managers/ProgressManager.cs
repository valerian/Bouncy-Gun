using UnityEngine;
using System.Collections;

public class ProgressManager : MonoBehaviour
{
    /*
    [Header("Special/Debug")]
    [SerializeField] private float skipToLevel = 0;
     * */

    public GameData IncrementLevel(GameData gameData)
    {
        gameData.currentLevel++;
        return gameData;
    }
}
