using UnityEngine;

[System.Serializable]
public class TreeUpgradeData
{
    [Tooltip("How many times it will animate towards its final scale")]
    public int growSteps = 3;

    public GameObject mainGO = default;
    public GameObject enableOnFinalScaleReached = default;
    public GameObject disableOnFinalScaleReached = default;
    ScaleTween scaleTween = default;

    [Header("Debug")]
    public int currentStep = 1;

    public bool IsDone => currentStep >= growSteps || scaleTween == null;

    public ScaleTween GetScaleTween() {
        return mainGO.GetComponentInChildren<ScaleTween>();
    }

    public void Activate(float fadeInDuration) {
        scaleTween = mainGO.GetComponentInChildren<ScaleTween>();
        mainGO.SetActive(true);
        scaleTween?.FadeIn(fadeInDuration);
    }

    public void ActivateReverse(float fadeInDuration) {
        scaleTween = mainGO.GetComponentInChildren<ScaleTween>();
        mainGO.SetActive(true);
        scaleTween?.FadeInBackwards(growSteps, fadeInDuration);
    }

    public void DoNextStep(float stepDuration) {
        currentStep++;

        if (IsDone) { return; }

        scaleTween.FadeStep(currentStep, growSteps, stepDuration);
    }

    public void OnFinalScaleReached() {
        enableOnFinalScaleReached?.SetActive(true);
        disableOnFinalScaleReached?.SetActive(false);
    }

    public void Deactivate() {
        mainGO.SetActive(false);
    }
}