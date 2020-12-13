﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Ghost/Sprite Turnaround Animation")]
public class SpriteAnimation : ScriptableObject
{
    public enum DirectionsType
    {
        FourWay,
        EightWay,
		OneWay,
    }

    public DirectionsType directionsCount = DirectionsType.EightWay;
    
	[HideIf("IsOneWay")]
    [PreviewField, HorizontalGroup("frames", marginLeft:2, marginRight:2, MaxWidth = 45)]
    public List<Sprite> north = new List<Sprite>();
    
    [LabelText("NE")]
    [ShowIf("IsEightWay"), PreviewField, HorizontalGroup("frames", marginLeft:2, marginRight:2, MaxWidth = 45)]
    public List<Sprite> northEast = new List<Sprite>();
    
    [PreviewField, HorizontalGroup("frames", marginLeft:2, marginRight:2, MaxWidth = 45)]
    public List<Sprite> east = new List<Sprite>();
    
    [LabelText("SE")]
    [ShowIf("IsEightWay"), PreviewField, HorizontalGroup("frames", marginLeft:2, marginRight:2, MaxWidth = 45)]
    public List<Sprite> southeast = new List<Sprite>();
    
	[HideIf("IsOneWay")]
    [PreviewField, HorizontalGroup("frames", marginLeft:2, marginRight:2, MaxWidth = 45)]
    public List<Sprite> south = new List<Sprite>();

    bool IsEightWay => directionsCount == DirectionsType.EightWay;
	bool IsOneWay => directionsCount == DirectionsType.OneWay;

    public List<Sprite> ClipForDirection(Vector2 direction)
    {
		if (IsOneWay) return east;

        float angle = Vector2.Angle(Vector2.up, direction);
        
        if (directionsCount == DirectionsType.EightWay)
        {
            if (angle < 22.5f) return north;
            if (angle < 67.5f) return northEast;
            if (angle < 112.5f) return east;
            if (angle < 157.5f) return southeast;
            return south;
        }
        else
        {
            if (angle < 45) return north;
            if (angle < 135) return east;
            return south;
        }
    }
}
