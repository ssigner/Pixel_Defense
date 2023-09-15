using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab; // 재사용할 프리팹
    public int poolSize = 10; // 풀 크기

    private List<GameObject> objectPool = new List<GameObject>();

    void Start()
    {
        // 객체 풀 초기화
        InitializeObjectPool();
    }

    bool isInitialized = false;
    // 객체 풀 초기화 함수
    public void InitializeObjectPool()
    {
        if (isInitialized)
            return;
        isInitialized = true;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab,this.transform);
            obj.name = prefab.name;
            obj.SetActive(false);
            objectPool.Add(obj);
        }
    }

    // 비활성화된 오브젝트를 찾아 반환
    public GameObject GetObjectFromPool()
    {
        foreach (var obj in objectPool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }
        return null; // 사용 가능한 오브젝트가 없을 경우 null 반환
    }

    // 오브젝트를 다시 풀에 반환
    public void ReturnObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(this.transform);
    }
}
