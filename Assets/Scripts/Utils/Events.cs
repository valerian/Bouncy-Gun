using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[System.Serializable]
public class UnityEventBool : UnityEvent<bool> {}

[System.Serializable]
public class UnityEventGameState : UnityEvent<Game.STATE> { }
