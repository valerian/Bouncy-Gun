using UnityEngine;
using System.Collections;

public class TEST_Instantiation : MonoBehaviour
{
    public GameObject prefabEmpty;
    public GameObject prefabFull;


    void Awake()
    {
        StartCoroutine(Spawner());
    }

    IEnumerator Spawner()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Instantiating empty " + Time.realtimeSinceStartup);
        GameObject obj = GameObject.Instantiate(prefabEmpty);
        Debug.Log("Instantiated empty " + Time.realtimeSinceStartup);
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Adding component " + Time.realtimeSinceStartup);
        TEST_ExecutionOrder comp = obj.AddComponent<TEST_ExecutionOrder>();
        Debug.Log("Enabling component " + Time.realtimeSinceStartup);
        comp.enabled = false;
        Debug.Log("Enabled component " + Time.realtimeSinceStartup);
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Activating empty " + Time.realtimeSinceStartup);
        obj.SetActive(true);
        Debug.Log("Activated empty " + Time.realtimeSinceStartup);
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Instantiating full " + Time.realtimeSinceStartup);
        GameObject obj2 = GameObject.Instantiate(prefabFull);
        Debug.Log("Instantiated full " + Time.realtimeSinceStartup);
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Activating full " + Time.realtimeSinceStartup);
        obj2.SetActive(true);
        Debug.Log("Activated full " + Time.realtimeSinceStartup);
    }
}
