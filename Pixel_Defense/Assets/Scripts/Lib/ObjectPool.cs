using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab; // ������ ������
    public int poolSize = 10; // Ǯ ũ��

    private List<GameObject> objectPool = new List<GameObject>();

    void Start()
    {
        // ��ü Ǯ �ʱ�ȭ
        InitializeObjectPool();
    }

    bool isInitialized = false;
    // ��ü Ǯ �ʱ�ȭ �Լ�
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

    // ��Ȱ��ȭ�� ������Ʈ�� ã�� ��ȯ
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
        return null; // ��� ������ ������Ʈ�� ���� ��� null ��ȯ
    }

    // ������Ʈ�� �ٽ� Ǯ�� ��ȯ
    public void ReturnObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(this.transform);
    }
}
