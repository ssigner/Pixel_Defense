using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : DIMono
{
    [Inject]
    SceneChanger loadingSceneData;

    public Animator animator;

    AsyncOperation loadSceneOp;
    public void AppearDone()
    {

        StartCoroutine(SceneChangeIE());
    }

    public void DisappearDone()
    {
        SceneManager.UnloadSceneAsync(loadingSceneData.loadingScene);
    }

    IEnumerator SceneChangeIE()
    {
        yield return SceneManager.UnloadSceneAsync(loadingSceneData.fromScene); 
        loadSceneOp = SceneManager.LoadSceneAsync(loadingSceneData.toScene, LoadSceneMode.Additive);

        yield return loadSceneOp;
        animator.SetTrigger("Disappear");

    }


}
