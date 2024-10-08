using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TowerAbilityCell : DIMono
{
    EventTrigger eventTrigger;
    public Image image;

    public void SetImage(Sprite icon)
    {
        CheckInject();
        image.sprite = icon;
    }
    System.Action<PointerEventData> mouseEnterAction;
    System.Action<PointerEventData> mouseExitAction;

    public void SetMouseEnterCallback( System.Action<PointerEventData> action)
    {
        mouseEnterAction = action;
    }


    public void SetMouseExitCallback(System.Action<PointerEventData> action)
    {
        mouseExitAction=action;
    }

    bool initialized = false;

    protected override void Init()
    {

        if (initialized)
        {
            return;
        }
        initialized = true;

        CheckInject();
        eventTrigger= GetComponent<EventTrigger>();
  
        // EventTriggerType.PointerEnter 이벤트에 대한 콜백 함수 설정
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { OnPointerEnter((PointerEventData)data); });
        eventTrigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerExit;
        entry.callback.AddListener((data) => { OnPointerExit((PointerEventData)data); });
        eventTrigger.triggers.Add(entry);
    }

    private void OnPointerExit(PointerEventData data)
    {
        Debug.Log("ExitPointer");
        mouseExitAction(data);
    }

    private void OnPointerEnter(PointerEventData data)
    {
        Debug.Log("OnPointer");
        mouseEnterAction(data);
    }

}
