using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/*public class StageConfig : ScriptableObject
{

    public int stageDuraction;
    public static void CreateStageConfig()
    {
        // ScriptableObject를 생성하고 저장할 경로 설정
        StageConfig stageConfig = ScriptableObject.CreateInstance<StageConfig>();
        string path = AssetDatabase.GenerateUniqueAssetPath("Assets/Prefabs/StageConfig.asset");

        // ScriptableObject를 에셋으로 저장
        AssetDatabase.CreateAsset(stageConfig, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // 에디터에서 생성된 에셋을 선택합니다.
        Selection.activeObject = stageConfig;
    }

}
*/