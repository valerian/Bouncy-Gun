using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    private UnityEventBool _onClickStateChanged;
    public UnityEventBool onClickStateChanged { get { return _onClickStateChanged ?? (_onClickStateChanged = new UnityEventBool()); } }

    private Vector3 _worldMousePosition;
    public Vector3 worldMousePosition { get { return isWorldMousePositionUpdated ? UpdateWorldMousePosition() : worldMousePosition; } private set; }

    private bool isWorldMousePositionUpdated = false;
 
    void Update()
    {
        isWorldMousePositionUpdated = false;
    }

    private Vector3 UpdateWorldMousePosition()
    {
        Vector3 position = Vector3.zero;
        isWorldMousePositionUpdated = true;
        return _worldMousePosition = position;
    }
}
