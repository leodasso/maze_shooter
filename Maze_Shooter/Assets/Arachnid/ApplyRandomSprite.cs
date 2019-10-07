using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Arachnid
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ApplyRandomSprite : MonoBehaviour
    {
        public List<Sprite> possibleSprites = new List<Sprite>();

        [Tooltip("Choose a random sprite and apply it to the sprite renderer on awake")]
        public bool applyOnAwake;

        // Start is called before the first frame update
        void Awake()
        {
            if (applyOnAwake) ApplySprite();
        }


        [Button]
        public void ApplySprite()
        {
            var sr = GetComponent<SpriteRenderer>();
            if (!sr) return;
            int index = Mathf.FloorToInt(Random.Range(0, possibleSprites.Count));
            sr.sprite = possibleSprites[index];
        }
    }
}