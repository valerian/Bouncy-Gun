using UnityEngine;
using System.Collections;
using System.Reflection;
using System;

///<summary>Replaces OnEnable() with OnEnableAwake().
///Provides OnEnableDelayed() for a delayed version of OnEnable().
///</summary>
public abstract class MonoBehaviourDelayedEnable : MonoBehaviour
{
    private bool __dynamicInitialized = false;
    private Action __OnEnableAwake = null;
    private Action __OnEnableDelayed = null;

    private void OnEnable()
    {
        if (!__dynamicInitialized)
            _DynamicInitialize();
        StartCoroutine(_OnEnableDelayer());
    }

    private void _DynamicInitialize()
    {
        Type t = this.GetType();
        MethodInfo onEnableAwakeInfo = t.GetMethod("OnEnableAwake", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
        MethodInfo onEnableDelayedInfo = t.GetMethod("OnEnableDelayed", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
        if (onEnableAwakeInfo != null)
            __OnEnableAwake = (Action)Delegate.CreateDelegate(typeof(Action), this, onEnableAwakeInfo);
        if (onEnableDelayedInfo != null)
            __OnEnableDelayed = (Action)Delegate.CreateDelegate(typeof(Action), this, onEnableDelayedInfo);
        __dynamicInitialized = true;
    }

    private IEnumerator _OnEnableDelayer()
    {
        if (__OnEnableAwake != null)
            __OnEnableAwake();
        yield return null;
        if (__OnEnableDelayed != null)
            __OnEnableDelayed();
    }
}
