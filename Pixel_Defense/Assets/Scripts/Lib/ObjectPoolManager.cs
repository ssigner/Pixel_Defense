using UnityEngine;
using System.Collections.Generic;

public class ObjectPoolManager : MonoBehaviour
{
    // Dictionary�� ����Ͽ� ������ ���� ������Ʈ Ǯ ����
    private Dictionary<string, ObjectPool> objectPools = new Dictionary<string, ObjectPool>();

    // �����հ� Ǯ ũ�⸦ �����ϱ� ���� Ŭ����
    [System.Serializable]
    public class PrefabInfo
    {
        public GameObject prefab;
        public string key;

        public int poolSize;
    }

    public List<PrefabInfo> prefabPoolPairs = new List<PrefabInfo>();
    Dictionary<GameObject, string> gameObjectKeys = new Dictionary<GameObject, string>();


    void Start()
    {
        // �� ������ ���� ������Ʈ Ǯ �ʱ�ȭ
        foreach (var pi in prefabPoolPairs)
        {
            InitializeObjectPool(pi);
        }
    }

    // ������ ���� ������Ʈ Ǯ �ʱ�ȭ �Լ�
    public void InitializeObjectPool(PrefabInfo  pi)
    {
        if (objectPools.ContainsKey(pi.key))
        {
            return;
        }

        GameObject poolParent = new GameObject(pi.prefab.name + " Pool");
        ObjectPool objectPool = poolParent.AddComponent<ObjectPool>();
        objectPool.prefab = pi.prefab;
        objectPool.poolSize = pi.poolSize;
        objectPool.InitializeObjectPool();
        objectPools[pi.key] = objectPool;

        poolParent.transform.SetParent(transform); // �θ� transform ����
    }

    // ������ ���� ������Ʈ ��������
    public GameObject GetObjectFromPool(string key)
    {
        if (objectPools.ContainsKey(key)==false)
        {
            Debug.LogError("Ǯ������ ���� �������� ��û�Ͽ����ϴ�: " + key);
            return null;
        }

        ObjectPool objectPool = objectPools[key];
        var obj = objectPool.GetObjectFromPool();
        gameObjectKeys[obj] = key;
        return obj;
    }

    // ������ ���� ������Ʈ ��ȯ�ϱ�
    public void ReturnObjectToPool( GameObject obj)
    {
        if (gameObjectKeys.ContainsKey(obj)==false)
        {
            Debug.LogError("Ǯ������ ���� �������� ��ȯ�Ϸ��� �մϴ�: " + obj.name);
            return;
        }


        var objPoolKey = gameObjectKeys[obj];
        if (objectPools.ContainsKey(objPoolKey) ==false)
        {
            Debug.LogError("Ǯ������ ���� �������� ��ȯ�Ϸ��� �մϴ�: " + obj.name);
            return;
        }

        ObjectPool objectPool = objectPools[objPoolKey];
        objectPool.ReturnObjectToPool(obj);
    }
}
