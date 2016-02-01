using UnityEngine;
using System.Collections;

public class PlayPause : MonoBehaviour
{
    public void TogglePlayPause()
    {
        Game.instance.TogglePlayPause();
    }
}
