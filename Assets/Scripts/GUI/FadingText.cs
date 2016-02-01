using UnityEngine;
using System.Collections;

public class FadingText : MonoBehaviour {

    public float timeToLive;
    public float finalVerticalOffset;

    private float startTime;

    #region private TextMesh textMesh
    private TextMesh _textMesh;
    private TextMesh textMesh { get { return _textMesh ?? (_textMesh = GetComponent<TextMesh>()); } }
    #endregion
    

    void OnEnable()
    {
        startTime = Time.time;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + (finalVerticalOffset * (Time.deltaTime / timeToLive)), transform.position.z);
        textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, (1f - ((Time.time - startTime) / timeToLive)));
        if (Time.time - startTime > timeToLive)
            Pool.Despawn(gameObject);
    }
}
