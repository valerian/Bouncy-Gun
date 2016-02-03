#if UNITY_EDITOR
#define POOL_USE_DYNAMIC_DESPAWN
#endif

#if UNITY_EDITOR
#define POOL_USE_DYNAMIC_SPAWN
#endif

using UnityEngine;
using System.Collections.Generic;


public static class Pool
{
    const int DEFAULT_POOL_SIZE = 16;
    static Dictionary<GameObject, GameObjectPool> pools;

    static private GameObject _root;
    static private GameObject root { get { return _root ?? InitPoolRoot(); } }

    class PoolRoot : MonoBehaviour
    {
        void OnDestroy()
        {
            Pool._root = null;
            pools = null;
        }
    }

    class GameObjectPool
    {
        int nextId = 1;
        Stack<GameObject> inactive;
        GameObject prefab;
 #if POOL_USE_DYNAMIC_SPAWN
        Transform spawnParent = null;
#endif

#if POOL_USE_DYNAMIC_DESPAWN
        GameObject _root = null;
        GameObject root
        {
            get
            { 

                if (!_root)
                {
                    _root = new GameObject(prefab.name + " Pool");
                    _root.transform.parent = Pool.root.transform;
                    _root.AddComponent<ChildrenCountInName>();
                }
                return _root;
            } 
        }
#endif

        public GameObjectPool(GameObject prefab, int initialQty)
        {
            this.prefab = prefab;
            inactive = new Stack<GameObject>(initialQty);
#if POOL_USE_DYNAMIC_SPAWN
            DynamicSpawnManager dynamicSpawnManager = Object.FindObjectOfType<DynamicSpawnManager>();
            if (dynamicSpawnManager)
                spawnParent = dynamicSpawnManager.GetSpawnParent(prefab);
#endif
        }

        public GameObject Spawn(Vector3 pos, Quaternion rot)
        {
            GameObject obj;
            if (inactive.Count == 0)
            {
                obj = (GameObject)GameObject.Instantiate(prefab, pos, rot);
                obj.name = prefab.name + " (" + (nextId++) + ")";
                obj.AddComponent<PoolMember>().myPool = this;
            }
            else
            {
                obj = inactive.Pop();

                if (obj == null)
                    return Spawn(pos, rot);
                obj.transform.position = pos;
                obj.transform.rotation = rot;
                obj.SetActive(true);
            }
#if POOL_USE_DYNAMIC_SPAWN
            obj.transform.parent = spawnParent;
#else
            obj.transform.parent = null;
#endif
            return obj;
        }

        public void Despawn(GameObject obj)
        {
#if POOL_USE_DYNAMIC_DESPAWN
            obj.transform.parent = root.transform;
#endif
            obj.SetActive(false);
            inactive.Push(obj);
        }

    }

    class PoolMember : MonoBehaviour
    {
        public GameObjectPool myPool;
    }

    static GameObject InitPoolRoot()
    {
        _root = new GameObject("POOL");
        _root.AddComponent<PoolRoot>();
        System.GC.Collect();
        Resources.UnloadUnusedAssets();
        return _root;
    }

    static void Init(GameObject prefab = null, int qty = DEFAULT_POOL_SIZE)
    {
        if (_root == null)
            InitPoolRoot();
        if (pools == null)
            pools = new Dictionary<GameObject, GameObjectPool>();
        if (prefab != null && pools.ContainsKey(prefab) == false)
            pools[prefab] = new GameObjectPool(prefab, qty);
    }

    static public void Preload(GameObject prefab, int qty = 1)
    {
        Init(prefab, qty);
        GameObject[] obs = new GameObject[qty];
        for (int i = 0; i < qty; i++)
            obs[i] = Spawn(prefab, Vector3.zero, Quaternion.identity);
        for (int i = 0; i < qty; i++)
            Despawn(obs[i]);
    }

    static public GameObject Spawn(GameObject prefab, Vector3? pos = null, Quaternion? rot = null)
    {
        Init(prefab);

        return pools[prefab].Spawn(pos ?? Vector3.zero, rot ?? Quaternion.identity);
    }

    static public void Despawn(GameObject obj)
    {
        PoolMember pm = obj.GetComponent<PoolMember>();
        if (pm == null)
        {
            Debug.Log("Object '" + obj.name + "' wasn't spawned from a pool. Destroying it instead.");
            GameObject.Destroy(obj);
        }
        else
            pm.myPool.Despawn(obj);
    }

    static public void Despawn(GameObject obj, float time)
    {
        
    }
}
