// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Set the isTrigger option of a Collider. Optionally set all collider found on the gameobject Target.")]
	public class SetColliderIsTrigger : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Collider))]
		[Tooltip("The GameObject with the Collider attached")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[Tooltip("The flag value")]
		public FsmBool isTrigger;
		
		[Tooltip("Set all Colliders on the GameObject target")]
		public bool setAllColliders;

		public override void Reset()
		{
			gameObject = null;
			isTrigger = false;
			setAllColliders = false;
		}
		
		public override void OnEnter()
		{
			DoSetIsTrigger();

			Finish();
		}
		
		void DoSetIsTrigger()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) return;

			
			if (setAllColliders)
			{
				// Find all of the colliders on the gameobject and set them all to be triggers.
				Collider[] cols = go.GetComponents<Collider> ();
				foreach (Collider c in cols) {
						c.isTrigger = isTrigger.Value;
				}
			}else{
				if (go.GetComponent<Collider>() != null)
					go.GetComponent<Collider>().isTrigger  = isTrigger.Value;
			}
		}
	}
}

