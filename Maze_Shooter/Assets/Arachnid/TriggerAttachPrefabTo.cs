using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Arachnid
{
    /// <summary>
    /// Extends the filtered trigger base class. When another object triggers this, it can attach a prefab
    /// as a child of the other object.
    /// </summary>
    public class TriggerAttachPrefabTo : FilteredTrigger2D
    {
        [AssetsOnly]
        public GameObject prefabToAttach;

        protected override void OnTriggered(Collider2D triggerer)
        {
            if (!prefabToAttach)
            {
                Debug.LogWarning("It seems like you meant to attach a prefab from this trigger," +
                                 " but there's no prefab referenced!", gameObject);
                return;
            }

            Transform triggererTransform = triggerer.gameObject.transform;
            Instantiate(prefabToAttach, triggererTransform.position, Quaternion.identity, triggererTransform);
        }
    }
}
