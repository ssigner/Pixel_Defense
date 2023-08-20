using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CubeWindow : EditorWindow
{
    [MenuItem("Examples/Mywindow")]
    static void Init()
    {
        CubeWindow window = GetWindow<CubeWindow>(typeof(Cube));
        window.minSize = new Vector2(100, 100);
        window.maxSize = new Vector2(500, 500);
        window.Show();
    }

    void OnGUI()
    {
        if (GUILayout.Button("��ư")) Debug.Log("����"); 
        if (GUILayout.RepeatButton("���� ��ư")) Debug.Log("����"); 
        if (EditorGUILayout.DropdownButton(new GUIContent("��Ӵٿ� ��ư"), FocusType.Keyboard)) Debug.Log("����"); 
    }
}
