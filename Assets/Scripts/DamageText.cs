using UnityEngine;
using System.Collections;

public class DamageText : MonoBehaviour {

    public float timeToLive;
    public float finalVerticalOffset;

    private float startTime;

    private TextMesh _textMesh;
    private TextMesh textMesh
    {
        get
        {
            if (_textMesh == null)
                _textMesh = GetComponent<TextMesh>();
            return _textMesh;
        }
    }

    void OnEnable()
    {
        startTime = Time.time;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + (finalVerticalOffset * (Time.deltaTime / timeToLive)), transform.position.z);
        textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, (1f - ((Time.time - startTime) / timeToLive)));
        if (Time.time - startTime > timeToLive)
            SimplePool.Despawn(gameObject);
    }
}
