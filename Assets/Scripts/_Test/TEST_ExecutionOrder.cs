using UnityEngine;
using System.Collections;

public class TEST_ExecutionOrder : MonoBehaviour
{
    int updateToLog = 3;
    int fixedUpdateToLog = 3;
    
    void Awake()
    {
        Debug.Log("Awake " + Time.realtimeSinceStartup);
    }

    void OnEnable()
    {
        Debug.Log("OnEnable Begin " + Time.realtimeSinceStartup);
        StartCoroutine(MyCoroutine());
        Debug.Log("OnEnable End " + Time.realtimeSinceStartup);
    }

    void Start()
    {
        Debug.Log("Start " + Time.realtimeSinceStartup);
    }

    void FixedUpdate()
    {
        if (fixedUpdateToLog > 0)
        {
            Debug.Log("FixedUpdate " + Time.realtimeSinceStartup);
            fixedUpdateToLog--;
        }

    }

    void Update()
    {
        if (updateToLog > 0)
        {
            Debug.Log("Update " + Time.realtimeSinceStartup);
            updateToLog--;
        } 
    }


    IEnumerator MyCoroutine()
    {
        Debug.Log("MyCoroutine Begin " + Time.realtimeSinceStartup);
        yield return new WaitForFixedUpdate();
        Debug.Log("MyCoroutine End " + Time.realtimeSinceStartup);
    }
}
