using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class TextAutoResizer : MonoBehaviour
{
    public Vector2 margin;

    public TextMeshProUGUI text;

    public string adjustText(string str)
    {
        StringBuilder sb = new StringBuilder();
        for(int i = 0; i < str.Length; i++)
        {
            sb.Append(str[i]);
            if(i % 11 == 0 && i != 0)
            {
                sb.AppendLine();
            }
        }
        return sb.ToString();
    }

    public void SetText(string str)
    {
        //if (this.gameObject.name == "skillInfo") text.text = adjustText(str);
        //else 
        str = str.Replace("q", "\n");
        text.text = str;

        var rt = this.transform as RectTransform;
        Vector2 textSize = text.GetPreferredValues()+ margin;
        rt.sizeDelta = textSize;
    }

}
