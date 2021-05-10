using UnityEngine;

namespace Arachnid {

	public class FollowCollection : MonoBehaviour
	{
		public Collection collectionToFollow;
		public Vector3 offset;

		// Update is called once per frame
		void LateUpdate()
		{
			if (!collectionToFollow) return;
			var objToFollow = collectionToFollow.GetFirstElement();
			if (!objToFollow) return;
			transform.position = objToFollow.transform.position + offset;
		}
	}
}