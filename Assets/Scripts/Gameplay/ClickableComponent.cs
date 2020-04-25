using UnityEngine;
using UnityEngine.Events;

public class ClickableComponent : MonoBehaviour, IGetClicked
{
    [SerializeField] protected bool oneTime = true;
    [SerializeField] protected UnityEvent onClick = null;

    protected bool hasBeenClicked = false;
    
    void Start(){}

    void IGetClicked.OnClick() {
        OnClick();
    }

    protected virtual void OnClick() {
        if (oneTime && hasBeenClicked) { return; }
        hasBeenClicked = true;
        onClick?.Invoke();
    }
}