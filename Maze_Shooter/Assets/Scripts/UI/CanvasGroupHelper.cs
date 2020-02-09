using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupHelper : MonoBehaviour
{
    public float lerpSpeed = 5;
    public float alpha = 1;
    CanvasGroup _canvasGroup;
    
    // Start is called before the first frame update
    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = alpha;
    }

    // Update is called once per frame
    void Update()
    {
        _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, alpha, Time.unscaledDeltaTime * lerpSpeed);
    }

    public void SetAlpha(float newAlpha)
    {
        alpha = newAlpha;
    }

    public void SnapAlpha(float newAlpha)
    {
        alpha = newAlpha;
        _canvasGroup.alpha = newAlpha;
    }
}
