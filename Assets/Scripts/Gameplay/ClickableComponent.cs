using UnityEngine;
using UnityEngine.Events;

public class ClickableComponent : MonoBehaviour, IGetClicked
{
    [SerializeField] bool oneTime = true;
    [SerializeField] UnityEvent onClick = null;

    bool hasBeenClicked = false;
    
    void Start(){}

    void IGetClicked.OnClick() {
        Debug.Log("1");
        if(oneTime && hasBeenClicked) { return; }
        hasBeenClicked = true;
        Debug.Log("2");
        onClick?.Invoke();
    }
}