using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Ghost/Time Controller")]
public class TimeController : ScriptableObject
{
    public AnimationCurve timeBendCurve;
    
    [Tooltip("Does it have a limited duration? If not, you will have to manually call 'EndTimeBend()' to return " +
             "to normal timescale.")]
    public bool hasDuration = true;
    [ShowIf("hasDuration")]
    public float duration = 1;

    bool _active = false;

    [ButtonGroup()]
    public void DoTimeBend()
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("This only works in play mode!");
            return;
        }

        _active = true;
        Arachnid.CoroutineHelper.NewCoroutine(BendTime());
    }

    [ButtonGroup()]
    public void EndTimeBend()
    {
        _active = false;
        Time.timeScale = 1;
    }

    IEnumerator BendTime()
    {
        float elapsed = 0;

        while (elapsed < duration)
        {
            if (!_active)
            {
                Time.timeScale = 1;
                yield break;
            }
            Time.timeScale = timeBendCurve.Evaluate(elapsed);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        
        // If there isn't a limited duration, just keep time at what it was at the end of the curve.
        if (hasDuration)
            Time.timeScale = 1;
    }
}
