using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;
using System;

[Prefab("GAME", true)]
public partial class Game : Singleton<Game>
{
    // ENUM

    public enum STATE
    {
        preInit,
        home,
        play,
        playLose,
        playEndLevel,
        playPause,
    }

    // STATIC SHORTCUTS

    public static STATE State { get { return instance.state; } }
    public static InputManager InputManager { get { return instance.inputManager; } }
    public static SpawnManager SpawnManager { get { return instance.spawnManager; } }
    public static GameData GameData { get { return instance.gameData; } }
    
    // PUBLIC EVENTS

    public UnityEventGameState onStateChanged { get { return _onStateChanged; } }
    public Dictionary<STATE, UnityEvent> onState = Enum.GetValues(typeof(STATE)).Cast<STATE>().ToDictionary(state => state, state => new UnityEvent());

    // FIELDS

    [SerializeField] private GameData gameData = default(GameData);
    [SerializeField] private STATE _state = STATE.preInit;
    private UnityEventGameState _onStateChanged = new UnityEventGameState();
    private InputManager inputManager = null;
    private SpawnManager spawnManager = null;

    // PROPERTIES

    private STATE state
    {
        get { return _state; }
        
        set
        {
            if (_state == value)
                return;
            _state = value;
            onStateChanged.Invoke(value);
            onState[value].Invoke();
        }
    }

    public void InitializeState(GameStateInit initializer)
    {
        state = initializer.initialState;
    }

    void Init()
    {
        if (state != STATE.preInit)
            Debug.LogWarning("Init called while state was not preInit!");
        inputManager = FindObjectOfType<InputManager>();
        spawnManager = FindObjectOfType<SpawnManager>();
        if (spawnManager != null)
            spawnManager.onLevelCleared.AddListener(OnLevelCleared);
    }

    void Awake()
    {
        Init();
    }

    void OnLevelWasLoaded(int index) 
    { 
        Init();
    }

    void FixedUpdate()
    {
        switch (state)
        {
            case STATE.play:
                break;
            default:
                break;
        }
    }

    void OnSceneUnload()
    {
        // remove references to local managers
        inputManager = null;
        spawnManager = null;

        // clean events from their listeners
        onStateChanged.RemoveAllListeners();
    }

    void OnLevelCleared()
    {
        state = STATE.playEndLevel;
    }
}
