using UnityEngine;
using System.Collections;

public class GameStateInit : MonoBehaviour
{
    public Game.STATE initialState;

    void Awake()
    {
        //Force Game from instantiate
        if (Game.instance)
            return;
    }
}
