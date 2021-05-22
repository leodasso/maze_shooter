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

	static float BentTimeScale = 1;
	public static float GetBentTimeScale => BentTimeScale;

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
        SetTimeScale(1);
    }

    IEnumerator BendTime()
    {
        float elapsed = 0;

        while (elapsed < duration)
        {
            if (!_active)
            {
                SetTimeScale(1);
                yield break;
            }
            SetTimeScale(timeBendCurve.Evaluate(elapsed));
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        
        // If there isn't a limited duration, just keep time at what it was at the end of the curve.
        if (hasDuration)
            SetTimeScale(1);
    }

	void SetTimeScale(float newScale) {
		Time.timeScale = newScale;
		BentTimeScale = newScale;
	}

	/// <summary>
	/// Returns the timescale to the one managed by timeController.
	/// Use this if you modify the time outside of timeController and want to return to it.
	/// </summary>
	public static void ReturnTimeScale() {
		Time.timeScale = BentTimeScale;
	}
}
