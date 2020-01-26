using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.Physics)]
    [Tooltip("Set the collision detection method of rigidbody.")]
    public class SetCollisionDetection : ComponentAction<Rigidbody>
    {
        [RequiredField]
        [CheckForComponent(typeof(Rigidbody))]
        public FsmOwnerDefault gameObject;

        [RequiredField] 
        public CollisionDetectionMode collisionDetectionMode;

        public override void Reset()
        {
            gameObject = null;
            collisionDetectionMode = CollisionDetectionMode.Discrete;
        }

        public override void OnEnter()
        {
            DoSetCollisionDetection();
            Finish();
        }

        void DoSetCollisionDetection()
        {
            var go = Fsm.GetOwnerDefaultTarget(gameObject);
            if (UpdateCache(go))
            {
                rigidbody.collisionDetectionMode = collisionDetectionMode;
            }
        }
    }
}

