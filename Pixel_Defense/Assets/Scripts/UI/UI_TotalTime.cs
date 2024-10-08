using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class UI_TotalTime : DIMono
{
    // Start is called before the first frame update

    public TextMeshProUGUI timer;
    public float totalPlayTime;
    public string totalTimeText;
    protected override void Init()
    {
        base.Init();
        CheckInject();
        totalPlayTime = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        SetTotalPlayTime((int)totalPlayTime);
        totalPlayTime += Time.deltaTime;
    }

    private void SetTotalPlayTime(int time)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append((time / 3600).ToString() + ":");
        time %= 3600;
        if (time / 60 >= 10) sb.Append((time / 60).ToString() + ":");
        else sb.Append("0" + (time / 60).ToString() + ":");
        time %= 60;
        if (time >= 10) sb.Append(time.ToString());
        else sb.Append("0" + time.ToString());
        timer.text = sb.ToString();
        totalTimeText = sb.ToString();
    }
}
