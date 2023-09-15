using UnityEngine;

public class Cube : DIMono
{
    [Inject]
    private GameData gameData;


    protected override void Init()
    {
        Debug.Log("Cube "+gameData);
    }

    public void SetData()
    {
        CheckInject();

    }
}


