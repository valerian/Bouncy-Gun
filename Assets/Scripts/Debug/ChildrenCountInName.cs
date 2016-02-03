using UnityEngine;
using System.Collections;

public class ChildrenCountInName : MonoBehaviour
{
    string initialName;

    void OnEnable()
    {
        initialName = gameObject.name;
    }

    void Update()
    {
        gameObject.name = initialName + " x" + (gameObject.transform.childCount);
    }
}
