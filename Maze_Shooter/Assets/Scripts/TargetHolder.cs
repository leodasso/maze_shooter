using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;

public enum TargetReplaceType
{
    DontReplace = 0,
    ReplaceOldest = 1,
    ReplaceNewest = 2,
}

/// <summary>
/// Keeps track of one or many targets. Can forget targets after a certain amount of time not seeing them.
/// </summary>
public class TargetHolder : MonoBehaviour
{
    [Tooltip("After this much time without seeing, I will forget a target.")]
    public FloatReference timeToForget;
    [MinValue(1)]
    public int maxTargets = 3;
    public List<Target> targets = new List<Target>();
    [Tooltip("When a new target is found and there's already the max amount of targets, what should happen?")]
    public TargetReplaceType targetReplaceType;

    [ReadOnly]
    public GameObject currentTarget;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Target t in targets)
            t.Update();

        currentTarget = targets.Count > 0 ? targets[0].target : null;
    }

    public void AddNewTarget(GameObject newTarget)
    {
        // If over the max targets, determine how to replace
        if (targets.Count >= maxTargets)
        {
            switch (targetReplaceType)
            {
                    case TargetReplaceType.DontReplace: return;
                    
                    case TargetReplaceType.ReplaceNewest:
                        targets.RemoveAt(targets.Count - 1);
                        break;
                    
                    case TargetReplaceType.ReplaceOldest:
                        targets.RemoveAt(0);
                        break;
            }
        }
        
        // Add the new target
        targets.Add(new Target(newTarget));
    }

    [System.Serializable]
    public class Target
    {
        [SceneObjectsOnly]
        public GameObject target;
        public bool isVisible;
        public float timeWithoutSeeing = 0;

        public Target(GameObject newTarget)
        {
            isVisible = true;
            target = newTarget;
        }

        public void Update()
        {
            if (!isVisible) timeWithoutSeeing += Time.deltaTime;
            else timeWithoutSeeing = 0;
        }
        
    }
}
