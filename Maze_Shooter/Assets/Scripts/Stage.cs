using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Stage")]
public class Stage : ScriptableObject
{
    public string sceneName;
    public bool useDefaultPlayerShip = true;
    
    [HideIf("useDefaultPlayerShip")]
    public GameObject customShip;

    public GameObject PlayerShip
    {
        get
        {
            if (!useDefaultPlayerShip && customShip != null) 
                return customShip;

            return GameMaster.Get().defaultPlayerShip;
        }
    }

    public void Load(float delay)
    {
        CoroutineHelper.NewCoroutine(DelayLoadScene(delay));
    }

    IEnumerator DelayLoadScene(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        SceneManager.LoadScene(sceneName);
    }

    public FloatReference startingDelay;
    
    public List<GameEvent> immediateEvents;
    public List<GameEvent> postStartingDelayEvents;

}
