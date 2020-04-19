﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ButtonPowerUp : MonoBehaviour
{
    [SerializeField] Button button = null;
    [SerializeField] Image fillImage = null;
    [SerializeField] float fillPerSec = .02f;

    [Tooltip("For powerups that need setup, this is a cooldown.")]
    [SerializeField] bool reEnableFromExternalEvent = false;

    [SerializeField] UnityEvent onClick = null;

    bool isEnabled = true;
    bool isFull = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (!isEnabled || isFull) { return; }

        if(fillImage.fillAmount < 1) {
            fillImage.fillAmount += fillPerSec * Time.deltaTime;
        }

        if (fillImage.fillAmount >= 1) {
            fillImage.fillAmount = 1;
            isFull = true;
        }
    }

    public void Reset() {
        isFull = false;
        fillImage.fillAmount = 0;
    }

    public void Enable() {
        button.interactable = isEnabled = true;
    }
    public void Disable() {
        button.interactable = isEnabled = false;
    }

    //from Button
    public void OnClick() {
        if (!isFull) { return; }

        onClick?.Invoke();

        if (reEnableFromExternalEvent) { return; }

        Reset();
    }



}