using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private float _ballisticPlaneZ = 0.35f;
    public float ballisticPlaneZ { get { return _ballisticPlaneZ; } }

    private UnityEventBool _onClickStateChanged;
    public UnityEventBool onClickStateChanged { get { return _onClickStateChanged ?? (_onClickStateChanged = new UnityEventBool()); } }

    private Vector3 _worldMousePosition;
    public Vector3 worldMousePosition { get { return isWorldMousePositionUpdated ? _worldMousePosition : UpdateWorldMousePosition(); } }

    public bool isClickPressed { get { return Input.GetButton("Fire1");  } }

    private bool isWorldMousePositionUpdated = false;
 
    void Update()
    {
        isWorldMousePositionUpdated = false;
        if (Input.GetButtonDown("Fire1"))
            onClickStateChanged.Invoke(true);
        else if (Input.GetButtonUp("Fire1"))
            onClickStateChanged.Invoke(false);
    }

    private Vector3 UpdateWorldMousePosition()
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        float translationFactor = (ballisticPlaneZ - position.z) / Camera.main.transform.forward.z;
        position += translationFactor * Camera.main.transform.forward;
        isWorldMousePositionUpdated = true;
        return _worldMousePosition = position;
    }
}
