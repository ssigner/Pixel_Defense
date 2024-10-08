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
        // 숫자와 구체를 그리기 위한 코드
        Gizmos.color = Color.red;

        var position = this.transform.position;
        // 구체 그리기
        Gizmos.DrawWireSphere(position, 0.3f);

#if UNITY_EDITOR
        // 숫자 표시
        UnityEditor.Handles.Label(position, index.ToString());
#endif

    }

}
