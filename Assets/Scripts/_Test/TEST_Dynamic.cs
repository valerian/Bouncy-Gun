using UnityEngine;
using System.Collections;
using System.Reflection;
using System;

public class TEST_Dynamic_Parent : MonoBehaviour
{
    private bool __dynamicInitialized = false;
    private MethodInfo __OnEnableAwake = null;
    private MethodInfo __OnEnableDelayed = null;

    private void OnEnable()
    {
        if (!__dynamicInitialized)
            _DynamicInitialize();
        StartCoroutine(_OnEnableDelayer());
    }

    private void _DynamicInitialize()
    {
        Type t = this.GetType();
        __OnEnableAwake = t.GetMethod("OnEnableAwake", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
        __OnEnableDelayed = t.GetMethod("OnEnableDelayed", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
        __dynamicInitialized = true;
    }

    private IEnumerator _OnEnableDelayer()
    {
        if (__OnEnableAwake != null)
            __OnEnableAwake.Invoke(this, null);
        yield return null;
        if (__OnEnableDelayed != null)
            __OnEnableDelayed.Invoke(this, null);
    }
}

public class TEST_Dynamic : TEST_Dynamic_Parent
{
    void OnEnableAwake()
    {
        Debug.Log("awake" + Time.realtimeSinceStartup);
    }

    void OnEnableDelayed()
    {
        Debug.Log("delayed " + Time.realtimeSinceStartup);
    }
}
