using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[Prefab("GAME", true)]
public class Game : Singleton<Game>
{
    // STATIC SHORTCUTS

    public static STATE State { get { return instance.state; } }

    public static InputManager InputManager { get { return instance.inputManager; } }
    public static SpawnManager SpawnManager { get { return instance.spawnManager; } }
    public static GameData GameData { get { return instance.gameData; } }

    // EVENTS

    private UnityEventGameState _onStateChanged;
    public UnityEventGameState onStateChanged { get { return _onStateChanged ?? (_onStateChanged = new UnityEventGameState()); } }

    // GAME STATE

    public enum STATE
    {
        preInit,
        homeStart,
        home,
        play,
        playStart,
        playLose,
        playEndLevel,
        playPause,
    }

    [SerializeField]
    private STATE _state = STATE.preInit;
    private STATE state
    {
        get
        {
            return _state;
        }
        
        set
        {
            if (_state == value)
                return;
            _state = value;
            onStateChanged.Invoke(value);
        } 
    }

    // MANAGERS

    private InputManager inputManager = null;
    private SpawnManager spawnManager = null;
    [SerializeField]
    private GameData gameData = default(GameData);


    void Init()
    {
        if (state != STATE.preInit)
            Debug.LogWarning("Init called while state was not preInit!");
        GameStateInit initializer = FindObjectOfType<GameStateInit>();
        if (initializer == null)
        {
            Debug.LogError("Could not find GameStateInit object");
            return;
        }

        state = initializer.initialState;
        inputManager = FindObjectOfType<InputManager>();
        spawnManager = FindObjectOfType<SpawnManager>();
        if (spawnManager != null)
            spawnManager.onLevelCleared.AddListener(OnLevelCleared);
    }
    void Awake() { Init(); }
    void OnLevelWasLoaded(int index) { Init(); }

    void FixedUpdate()
    {
        switch (state)
        {
            case STATE.playStart:
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
    }

    void OnLevelCleared()
    {
        state = STATE.playEndLevel;
    }
}
