using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class UI_HiddenTowerPlace : DIMono
{
    public GridLayoutGroup gridLayout;

    protected override void Init()
    {
        base.Init();
        CheckInject();
    }

    public void setTowerCell(List<Tower> hiddenTowers)
    {
        var protoItem = gridLayout.transform.GetChild(0).gameObject;
        var protoImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        protoImage.sprite = Addressables.LoadAssetAsync<Sprite>(hiddenTowers[0].spritePath).WaitForCompletion();
        int index = 0;

        transform.parent.gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(true);

        foreach (Transform t in gridLayout.transform)
        {
            t.gameObject.SetActive(false);
        }

        foreach (var tower in hiddenTowers)
        {
            var cell = GetCellItem(index, protoItem);
            cell.SetData(tower);
            cell.gameObject.SetActive(true);
            index++;

        }
    }
    private UI_HiddenTowerCell GetCellItem(int idx, GameObject protoItem)
    {
        if (idx >= gridLayout.transform.childCount)
        {
            return Instantiate(protoItem, gridLayout.transform).GetComponent<UI_HiddenTowerCell>();

        }
        return gridLayout.transform.GetChild(idx).GetComponent<UI_HiddenTowerCell>();
    }
}



