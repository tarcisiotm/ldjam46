﻿using UnityEngine;

[System.Serializable]
public class TreeUpgradeData
{
    [Tooltip("How many times it will animate towards its final scale")]
    public int growSteps = 3;

    public GameObject mainGO = default;
    public GameObject enableOnFinalScaleReached = default;
    public GameObject disableOnFinalScaleReached = default;
    ScaleTween scaleTween;
    int currentStep = 0;

    public bool IsDone => currentStep > growSteps || scaleTween == null;

    public void Activate(float fadeInDuration) {
        scaleTween = mainGO.GetComponentInChildren<ScaleTween>();
        mainGO.SetActive(true);
        scaleTween?.FadeIn(fadeInDuration);
    }

    public void DoNextStep(float stepDuration) {
        currentStep++;
        Debug.Log("1");
        if (IsDone) { return; }
        Debug.Log("2");

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