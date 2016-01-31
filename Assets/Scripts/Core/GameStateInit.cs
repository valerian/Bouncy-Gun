using UnityEngine;
using System.Collections;


public partial class GameStateInit : MonoBehaviour
{
    public Game.STATE initialState;

    void Awake()
    {
        //Force Game to instantiate
        if (Game.instance)
            return;
    }

    void Start()
    {
        Game.instance.InitializeState(this);
    }
}
