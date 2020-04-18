using UnityEngine;
using DG.Tweening;

public class ScaleTween : MonoBehaviour
{
    [SerializeField] bool fadeInScale = true;

    [SerializeField] Vector3 initialFadeInScale = new Vector3(1, 1, 1);
    [SerializeField] Vector3 initialScale = new Vector3(1, 1, 1);
    [SerializeField] Vector3 finalScale = Vector3.one;

    public void FadeIn(float fadeDuration) {
        transform.localScale = initialFadeInScale;
        if (!fadeInScale || initialFadeInScale == initialScale) { return; }
        transform.DOScale(initialScale, fadeDuration);
    }

    public void FadeStep(int step, int totalSteps, float duration) {
        Vector3 scale = Vector3.Lerp(initialScale, finalScale, (float)step / (float)totalSteps);
        Debug.Log("Scale: "+scale);
        transform.DOScale(scale, duration);
    }

    void Start()
    {
        
    }


}
