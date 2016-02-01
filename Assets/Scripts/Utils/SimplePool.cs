using UnityEngine;
using System.Collections.Generic;

public static class SimplePool
{
    const int DEFAULT_POOL_SIZE = 16;
    static Dictionary<GameObject, Pool> pools;

    static private GameObject _root;
    static private GameObject root { get { return _root ?? InitPoolRoot(); } }

    class PoolRoot : MonoBehaviour
    {
        void OnDestroy()
        {
            SimplePool._root = null;
            pools = null;
        }
    }

    class Pool
    {
        int nextId = 1;
        Stack<GameObject> inactive;
        GameObject prefab;

        GameObject _root;
        GameObject root
        {
            get
            { 
                if (!_root)
                {
                    _root = new GameObject(prefab.name + " Pool");
                    _root.transform.parent = SimplePool.root.transform;
                }
                return _root;
            } 
        }

        public Pool(GameObject prefab, int initialQty)
        {
            this.prefab = prefab;
            inactive = new Stack<GameObject>(initialQty);
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
                obj.transform.parent = null;
                obj.SetActive(true);
            }

            return obj;
        }

        public void Despawn(GameObject obj)
        {
            obj.transform.parent = root.transform;
            obj.SetActive(false);
            inactive.Push(obj);
        }

    }

    class PoolMember : MonoBehaviour
    {
        public Pool myPool;
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
            pools = new Dictionary<GameObject, Pool>();
        if (prefab != null && pools.ContainsKey(prefab) == false)
            pools[prefab] = new Pool(prefab, qty);
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
}
