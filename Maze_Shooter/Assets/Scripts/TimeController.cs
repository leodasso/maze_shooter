using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu]
public class TimeController : ScriptableObject
{
    public AnimationCurve timeBendCurve;
    public float duration = 1;

    [Button]
    public void DoTimeBend()
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("This only works in play mode!");
            return;
        }
        Arachnid.CoroutineHelper.NewCoroutine(BendTime());
    }

    IEnumerator BendTime()
    {
        float elapsed = 0;

        while (elapsed < duration)
        {
            Time.timeScale = timeBendCurve.Evaluate(elapsed);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        Time.timeScale = 1;
    }
}
