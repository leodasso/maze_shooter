using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class GuiHeart : MonoBehaviour
{
	public bool filled {
		get {
			return _filled;
		}
		set {
			_filled = value;
			image.sprite = filled ? filledSprite : emptySprite;
		}
	}

	public int fraction {
		set {
			image.sprite = fractions[value - 1];
		}
	}


	[SerializeField, Space, PreviewField]
	Sprite filledSprite;

	[SerializeField, PreviewField]
	Sprite emptySprite;

	[SerializeField, PreviewField]
	List<Sprite> fractions = new List<Sprite>();

	[SerializeField]
	Image image;


	bool _filled;

	[ButtonGroup]
	void Fill() {filled = true;}

	[ButtonGroup]
	void Empty() {filled = false;}
	

    // Start is called before the first frame update
    void Start()
    {
    }
}
