using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;


[CreateAssetMenu]
[TypeInfoBox("This can be referenced by guns in order to fire more complex firing patterns per shot.")]
public class FiringPattern : SerializedScriptableObject
{
    public int bullets = 1;
    [Tooltip("Interval (in seconds) between each bullet firing.")]
    public float interval;
    public float widthSpread = 1;
    public float heightSpread = .5f;
    public float angleSpread;
    [Tooltip("If there's an angle spread, bullets will snap to increments of this value. For instance, if this " +
             "value is '15' and angleSpread is 30, bullets will all have an angle of either 0, 15, or 30")]
    public int snapAngle;
    

}
