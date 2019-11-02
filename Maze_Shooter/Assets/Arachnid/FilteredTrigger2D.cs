using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Arachnid
{
	[RequireComponent(typeof(Collider2D))]
	public class FilteredTrigger2D : MonoBehaviour
	{

		[ToggleLeft, Tooltip("Will only trigger once.")]
		public bool oneOff;

		[ToggleLeft, Tooltip("Only allow triggers from objects of a particular collection")]
		public bool filterTriggers = true;

		[ShowIf("filterTriggers"), Tooltip("Any object in one of these collections can trigger this."), AssetsOnly]
		public List<Collection> triggerers = new List<Collection>();
		
		[ToggleLeft]
		public bool useLayerMask;
		
		[Tooltip("Layers which will trigger this."), ShowIf("useLayerMask")]
		public LayerMask layerMask;

		bool _triggered;


		void OnTriggerEnter2D(Collider2D other)
		{
			if (!filterTriggers)
			{
				Trigger(other);
				return;
			}

			foreach (var c in triggerers)
			{
				if (c.ContainsGameObject(other.gameObject))
				{
					Trigger(other);
					return;
				}
			}
		}

		void OnTriggerExit2D(Collider2D other)
		{
			if (!filterTriggers)
			{
				TriggerExit(other);
				return;
			}

			foreach (var c in triggerers)
			{
				if (c.ContainsGameObject(other.gameObject))
				{
					TriggerExit(other);
					return;
				}
			}
		}

		void Trigger(Collider2D other)
		{
			if (_triggered && oneOff) 
				return;
			if (useLayerMask && !Math.LayerMaskContainsLayer(layerMask, other.gameObject.layer))
				return;
			_triggered = true;
			OnTriggered(other);
		}

		void TriggerExit(Collider2D other)
		{
			if (_triggered && oneOff) 
				return;
			if (useLayerMask && !Math.LayerMaskContainsLayer(layerMask, other.gameObject.layer))
				return;
			_triggered = true;
			OnTriggerExited(other);
		}

		protected virtual void OnTriggerExited(Collider2D triggerer)
		{
			
		}

		protected virtual void OnTriggered(Collider2D triggerer)
		{
		}
	}
}