using UnityEngine;
using System.Collections.Generic;

public class ObjectPoolManager : MonoBehaviour
{
    // Dictionary를 사용하여 프리팹 별로 오브젝트 풀 관리
    private Dictionary<string, ObjectPool> objectPools = new Dictionary<string, ObjectPool>();

    // 프리팹과 풀 크기를 설정하기 위한 클래스
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
        // 각 프리팹 별로 오브젝트 풀 초기화
        foreach (var pi in prefabPoolPairs)
        {
            InitializeObjectPool(pi);
        }
    }

    // 프리팹 별로 오브젝트 풀 초기화 함수
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

        poolParent.transform.SetParent(transform); // 부모 transform 설정
    }

    // 프리팹 별로 오브젝트 가져오기
    public GameObject GetObjectFromPool(string key)
    {
        if (objectPools.ContainsKey(key)==false)
        {
            Debug.LogError("풀링되지 않은 프리팹을 요청하였습니다: " + key);
            return null;
        }

        ObjectPool objectPool = objectPools[key];
        var obj = objectPool.GetObjectFromPool();
        gameObjectKeys[obj] = key;
        return obj;
    }

    // 프리팹 별로 오브젝트 반환하기
    public void ReturnObjectToPool( GameObject obj)
    {
        if (gameObjectKeys.ContainsKey(obj)==false)
        {
            Debug.LogError("풀링되지 않은 프리팹을 반환하려고 합니다: " + obj.name);
            return;
        }


        var objPoolKey = gameObjectKeys[obj];
        if (objectPools.ContainsKey(objPoolKey) ==false)
        {
            Debug.LogError("풀링되지 않은 프리팹을 반환하려고 합니다: " + obj.name);
            return;
        }

        ObjectPool objectPool = objectPools[objPoolKey];
        objectPool.ReturnObjectToPool(obj);
    }
}
