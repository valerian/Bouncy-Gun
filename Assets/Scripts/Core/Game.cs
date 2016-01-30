using UnityEngine;
using System.Collections;

[Prefab("GAME", true)]
public class Game : Singleton<Game>
{
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

    public static InputManager inputManager { get; private set; }
    public static SpawnManager spawnManager { get; private set; }

    [SerializeField]
    private STATE _state = STATE.preInit;
    public STATE state { get { return _state; } private set { _state = value; } }

    void Awake() { Init(); }
    void OnLevelWasLoaded(int index) { Init(); }
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
        inputManager = null;
        spawnManager = null;
    }

    void OnLevelCleared()
    {

    }
}
