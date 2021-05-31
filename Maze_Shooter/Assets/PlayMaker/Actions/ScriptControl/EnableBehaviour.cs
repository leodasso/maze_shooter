// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.ScriptControl)]
	[Tooltip("Enables/Disables a Behaviour on a GameObject. Optionally reset the Behaviour on exit - useful if you want the Behaviour to be active only while this state is active.")]
	public class EnableBehaviour : FsmStateAction
	{
		[RequiredField]
        [Tooltip("The GameObject that owns the Behaviour.")]
		public FsmOwnerDefault gameObject;
		
		[UIHint(UIHint.Behaviour)]
        [Tooltip("The name of the Behaviour to enable/disable.")]
		public FsmString behaviour;
		
		[Tooltip("Optionally drag a component directly into this field (behavior name will be ignored).")]
		public Component component;
		
		[RequiredField]
        [Tooltip("Set to True to enable, False to disable.")]
		public FsmBool enable;
		
		public FsmBool resetOnExit;

		public override void Reset()
		{
			gameObject = null;
			behaviour = null;
			component = null;
			enable = true;
			resetOnExit = true;
		}

		Behaviour componentTarget;
		Collider colliderComponent;

		public override void OnEnter()
		{
			DoEnableBehaviour(Fsm.GetOwnerDefaultTarget(gameObject));
			
			Finish();
		}

		void DoEnableBehaviour(GameObject go)
		{
			if (go == null)
			{
				return;
			}

			if (component != null)
				ProcessComponent(component, go);

			else
				ProcessComponent(go.GetComponent(ReflectionUtils.GetGlobalType(behaviour.Value)), go);
		}

		void ProcessComponent(Component component, GameObject go)
		{
			colliderComponent = component as Collider;
			componentTarget = component as Behaviour;

			if (colliderComponent != null)
				colliderComponent.enabled = enable.Value;

			else if (componentTarget != null)
				componentTarget.enabled = enable.Value;

			else LogWarning(" " + go.name + " missing behaviour: " + behaviour.Value);
		}

		public override void OnExit()
		{
			if (componentTarget == null)
			{
				return;
			}

			if (resetOnExit.Value)
			{
				componentTarget.enabled = !enable.Value;
			}
		}

	    public override string ErrorCheck()
	    {
	        var go = Fsm.GetOwnerDefaultTarget(gameObject);

	        if (go == null || component != null || behaviour.IsNone || string.IsNullOrEmpty(behaviour.Value))
	        {
	            return null;
	        }

	        var comp = go.GetComponent(ReflectionUtils.GetGlobalType(behaviour.Value)) as Behaviour;
	        return comp != null ? null : "Behaviour missing";
	    }

		public override string AutoName()
		{
			return enable.Value ? "Enable Behavior" : "Disable Behavior";
		}
	}
}