using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ButtonPowerUp : MonoBehaviour
{
    [SerializeField] Button button = null;
    [SerializeField] Image fillImage = null;
    [SerializeField] float fillPerSec = .02f;
    [Space]
    [SerializeField] bool isToggle = false;
    [SerializeField] GameObject toggleGO = null;
    [Space]
    [Tooltip("For powerups that need setup, this is a cooldown.")]
    [SerializeField] bool reEnableFromExternalEvent = false;

    //if this is still enabled, and not null, do not click
    [SerializeField] GameObject goToCheck = null;

    [SerializeField] UnityEvent onClick = null;
    [SerializeField] UnityEvent onToggleDisabled = null;

    Color originalColor;

    bool toggleState = false;

    [SerializeField] Color filledColor;

    bool isEnabled = true;
    bool isFull = false;

    void Start()
    {
        originalColor = fillImage.color;   
    }

    void Update()
    {
        if (!isEnabled || isFull) { return; }

        if(fillImage.fillAmount < 1) {
            fillImage.fillAmount += fillPerSec * Time.deltaTime;
        }

        if (fillImage.fillAmount >= 1) {
            fillImage.fillAmount = 1;
            fillImage.color = filledColor;
            isFull = true;
        }
    }

    public void Reset() {
        fillImage.color = originalColor;
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

        if(goToCheck != null && goToCheck.activeSelf) { return; }


        onClick?.Invoke();

        if (isToggle)
        {
            Toggle();
            return;
        }

        if (reEnableFromExternalEvent) { return; }

        Reset();
    }

    public void DisableToggle()
    {
        if (toggleState) { Toggle(); }
    }

    void Toggle()
    {
        toggleState = !toggleState;
        toggleGO.SetActive(toggleState);

        if (!toggleState)
        {
            onToggleDisabled?.Invoke();
        }
    }



}