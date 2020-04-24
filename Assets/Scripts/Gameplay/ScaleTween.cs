using UnityEngine;
using DG.Tweening;

public class ScaleTween : MonoBehaviour
{
    [SerializeField] bool fadeInScale = true;

    [SerializeField] public Vector3 initialFadeInScale = new Vector3(1, 1, 1);
    [SerializeField] public Vector3 initialScale = new Vector3(1, 1, 1);
    [SerializeField] public Vector3 finalScale = Vector3.one;

    void Start() { }

    public void FadeIn(float fadeDuration) {
        transform.localScale = initialFadeInScale;
        if (!fadeInScale || initialFadeInScale == initialScale) { return; }
        transform.DOScale(initialScale, fadeDuration);
    }

    public void FadeStep(int step, int totalSteps, float duration) {
        Vector3 scale = Vector3.Lerp(initialScale, finalScale, (float)step / (float)totalSteps);
        transform.DOScale(scale, duration);
    }

    public void FadeInBackwards(int totalSteps, float fadeDuration) {
        transform.localScale = finalScale;
        //if (!fadeInScale || initialFadeInScale == finalScale) { return; }
        FadeStepBackwards(totalSteps - 1, totalSteps, fadeDuration);
        //transform.DOScale(initialScale, fadeDuration);
    }

    public void FadeStepBackwards(int step, int totalSteps, float duration) {
        Vector3 scale = Vector3.Lerp(finalScale, initialScale, (float)step / (float)totalSteps);
        transform.DOScale(scale, duration);
    }

}