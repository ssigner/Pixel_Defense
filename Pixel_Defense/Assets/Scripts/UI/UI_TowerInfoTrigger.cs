using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class UI_TowerInfoTrigger : DIMono
{
    [Inject("atkRange")]
    GameObject atkRange;
    public TowerUnit towerUnit;

    [Inject]
    Camera mainCamera;

    [Inject]
    UI_TowerInfo UI_TowerInfo;

    [Inject]
    AudioManager audioManager;

    [Inject]
    TowerPlacer towerPlacer;

    float startTime;
    private new SpriteRenderer renderer;
    //private MaterialPropertyBlock materialPropertyBlock;
    private Material effectMaterial;
    public Material originalMaterial;

    //public GameObject hiddenTowerPlace;

    protected override void Init()
    {
        towerUnit = GetComponent<TowerUnit>();
        startTime = Time.time;


        renderer = GetComponent<SpriteRenderer>();
        effectMaterial = Addressables.LoadAssetAsync<Material>("Materials/outline.mat").WaitForCompletion();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && Time.time - startTime > 0.3f)
        {
            HideAllUI();
        }

        if (Input.GetMouseButtonDown(0) && Time.time - startTime > 0.3f)
        {
            Debug.Log("Click Check");
            // 마우스 클릭 위치를 월드 좌표로 변환
            Vector3 clickPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            // Raycast를 사용하여 클릭한 위치에 대한 정보를 얻음
            RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);
            Debug.DrawRay(clickPosition, hit.point, Color.red, 0.5f); 

            if (hit.collider != null 
                && hit.collider.gameObject == this.gameObject 
                && !UI_TowerInfo.IsVisible()
                && !towerPlacer.IsPreviewTowerVisible())
            {
                Debug.Log("Hit Check");
                ShowUIWindow();
                
                ShowTowerAtkRange();
                UI_TowerInfo.HideHiddenPlace();

                audioManager.Play("BtnClickAndBuyUnit");
                ShowEffectMaterial();

                Debug.Log("ShowEffect Check");
            }
            startTime = Time.time;
        }
    }
    //TODO:material property block 사용해보기
    /*    private void setSelectedEffect()
        {
            renderer.SetPropertyBlock(materialPropertyBlock);
        }*/
    //
    public void SpriteMaterialCheck()
    {
        if(renderer.material == effectMaterial)
        {
            Debug.Log("effectMaterial");
        }
        else
        {
            Debug.Log("originalMaterial");
        }
    }
    //
    public void HideAllUI()
    {
        HideEffectMaterial();
        HideUIWindow();
        HideTowerAtkRange();
        SpriteMaterialCheck();
    }

    private void ShowEffectMaterial()
    {
        renderer.material = effectMaterial;
    }
    private void HideEffectMaterial()
    {
        renderer.material = originalMaterial;
    }
    private void ShowTowerAtkRange()
    {
        atkRange.SetActive(true);
        atkRange.transform.position = towerUnit.transform.position;
        atkRange.transform.localScale = Vector3.one * towerUnit.Tower.atkRange * 2;
    }
    public void HideTowerAtkRange()
    {
        atkRange.SetActive(false);
    }

    private void HideUIWindow()
    {
         UI_TowerInfo.Hide();
    }

    private void ShowUIWindow()
    {
         UI_TowerInfo.Show(towerUnit);
    }
}
