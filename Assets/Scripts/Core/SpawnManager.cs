using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class SpawnManager : MonoBehaviour
{
    private UnityEvent _onLevelCleared;
    public UnityEvent onLevelCleared { get { return _onLevelCleared ?? (_onLevelCleared = new UnityEvent()); } }

    void FixedUpdate()
    {
        if (Game.instance.state != Game.STATE.play)
            return;
    }
}
