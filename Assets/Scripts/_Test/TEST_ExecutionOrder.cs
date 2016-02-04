using UnityEngine;
using System.Collections;

public class TEST_ExecutionOrder : MonoBehaviour
{
    int updateToLog = 3;
    int fixedUpdateToLog = 3;
    
    void Awake()
    {
        Debug.Log(gameObject.name + " Awake " + Time.realtimeSinceStartup);
    }

    void OnEnable()
    {
        Debug.Log(gameObject.name + " OnEnable Begin " + Time.realtimeSinceStartup);
        StartCoroutine(MyCoroutineNull());
        StartCoroutine(MyCoroutineFixedUpdate());
        Debug.Log(gameObject.name + " OnEnable End " + Time.realtimeSinceStartup);
    }

    void Start()
    {
        Debug.Log(gameObject.name + " Start " + Time.realtimeSinceStartup);
    }

    void FixedUpdate()
    {
        if (fixedUpdateToLog > 0)
        {
            Debug.Log(gameObject.name + " FixedUpdate " + Time.realtimeSinceStartup);
            fixedUpdateToLog--;
        }

    }

    void Update()
    {
        if (updateToLog > 0)
        {
            Debug.Log(gameObject.name + " Update " + Time.realtimeSinceStartup);
            updateToLog--;
        } 
    }


    IEnumerator MyCoroutineNull()
    {
        yield return null;
        Debug.Log(gameObject.name + " Coroutine Null " + Time.realtimeSinceStartup);
    }

    IEnumerator MyCoroutineFixedUpdate()
    {
        yield return new WaitForFixedUpdate();
        Debug.Log(gameObject.name + " Coroutine FixedUpdate " + Time.realtimeSinceStartup);
    }
}
