using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StageConfig : ScriptableObject
{

    public float stageDuraction;


    [MenuItem("Assets/Create/StageConfig")]
    public static void CreateStageConfig()
    {
        // ScriptableObject�� �����ϰ� ������ ��� ����
        StageConfig stageConfig = ScriptableObject.CreateInstance<StageConfig>();
        string path = AssetDatabase.GenerateUniqueAssetPath("Assets/Prefabs/StageConfig.asset");

        // ScriptableObject�� �������� ����
        AssetDatabase.CreateAsset(stageConfig, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // �����Ϳ��� ������ ������ �����մϴ�.
        Selection.activeObject = stageConfig;
    }

}
