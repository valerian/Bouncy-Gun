using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;
using System;

[Prefab("GAME", true)]
public partial class Game : Singleton<Game>
{
    // CLASSES

    [System.Serializable]
    public class UnityEventGameStates : UnityEvent<Game.STATE, Game.STATE> { }

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
    public static EnemySpawnManager SpawnManager { get { return instance.spawnManager; } }
    public static GameData GameData { get { return instance.gameData; } }
    
    // PUBLIC EVENTS

    public UnityEventGameStates onStateChanged { get { return _onStateChanged; } }
    public Dictionary<STATE, UnityEvent> onState = Enum.GetValues(typeof(STATE)).Cast<STATE>().ToDictionary(state => state, state => new UnityEvent());

    // FIELDS

    [SerializeField] private GameData gameData = default(GameData);
    [SerializeField] private STATE _state = STATE.preInit;
    public Dictionary<STATE, bool> GCOnState = new Dictionary<STATE, bool>
    {
        {STATE.preInit, true},
        {STATE.home, false},
        {STATE.play, false},
        {STATE.playLose, false},
        {STATE.playEndLevel, true},
        {STATE.playPause, true},
    };
    private UnityEventGameStates _onStateChanged = new UnityEventGameStates();
    private InputManager inputManager = null;
    private EnemySpawnManager spawnManager = null;
    
    // PROPERTIES

    private STATE state { get { return _state; } set { if (_state == value) return; StateChangeTriggers(_state, _state = value); } }

    public void InitializeState(GameStateInit initializer)
    {
        state = initializer.initialState;
    }

    void Init()
    {
        if (state != STATE.preInit)
            Debug.LogWarning("Init called while state was not preInit!");
        inputManager = FindObjectOfType<InputManager>();
        spawnManager = FindObjectOfType<EnemySpawnManager>();
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

    void StateChangeTriggers(STATE previousState, STATE newState)
    {
        Debug.Log("State changed: " + previousState + " => " + newState);
        if (newState == STATE.playPause)
            Time.timeScale = 0f;
        if (previousState == STATE.playPause)
            Time.timeScale = 1f;
        onStateChanged.Invoke(previousState, newState);
        onState[newState].Invoke();
        if (GCOnState.ContainsKey(newState) && GCOnState[newState])
        {
            GC.Collect();
            Resources.UnloadUnusedAssets();
        }
    }

    public void TogglePlayPause()
    {
        if (state == STATE.play)
            state = STATE.playPause;
        else if (state == STATE.playPause)
            state = STATE.play;
    }
}
