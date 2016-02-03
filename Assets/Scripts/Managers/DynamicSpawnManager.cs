using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class DynamicSpawnManager : MonoBehaviour
{
    [System.Serializable]
    [SerializeField]
    private struct DynamicSpawnSetting
    {
        public GameObject prefab;
        public GameObject spawnParent;
    }

    [SerializeField]
    private DynamicSpawnSetting[] spawnSettings;
    private Dictionary<GameObject, GameObject> spawnParents;

    void Awake()
    {
        spawnParents = spawnSettings.ToDictionary(ss => ss.prefab, ss => ss.spawnParent);
    }

    public Transform GetSpawnParent(GameObject prefab)
    {
        return spawnParents.ContainsKey(prefab) ? spawnParents[prefab].transform : null;
    }
}
