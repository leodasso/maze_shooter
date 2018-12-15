using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SetTilemapColor : ComponentAction<Tilemap>
{
    [RequiredField]
    public Tilemap tilemap;
    [RequiredField]
    public FsmColor color;
    public bool everyFrame;

    public override void Reset()
    {
        tilemap = null;
    }

    public override void OnEnter()
    {
        DoSetTilemapColor();
        if (!everyFrame) Finish();
    }

    public override void OnUpdate()
    {
        if (everyFrame)
            DoSetTilemapColor();
    }

    void DoSetTilemapColor()
    {
        if (!tilemap) return;
        tilemap.color = color.Value;
    }
}
