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
        if (GUILayout.Button("버튼")) Debug.Log("반응"); 
        if (GUILayout.RepeatButton("연속 버튼")) Debug.Log("반응"); 
        if (EditorGUILayout.DropdownButton(new GUIContent("드롭다운 버튼"), FocusType.Keyboard)) Debug.Log("반응"); 
    }
}
