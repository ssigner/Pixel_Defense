using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[ExecuteInEditMode]
public class PathPoint : MonoBehaviour
{

    public int index;

    private void OnEnable()
    {
        index = this.transform.GetSiblingIndex();
    }



    private void OnDrawGizmos()
    {
        // ���ڿ� ��ü�� �׸��� ���� �ڵ�
        Gizmos.color = Color.red;

        var position = this.transform.position;
        // ��ü �׸���
        Gizmos.DrawWireSphere(position, 0.3f);

#if UNITY_EDITOR
        // ���� ǥ��
        UnityEditor.Handles.Label(position, index.ToString());
#endif

    }

}
