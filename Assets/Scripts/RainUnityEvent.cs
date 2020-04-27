using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RainUnityEvent : MonoBehaviour
{

    [SerializeField] bool destroyAfterInvoke = true;
    [SerializeField] UnityEvent onRain = default;

    private void OnEnable() {
        Rain.OnRainEvent += OnRain;
    }

    private void OnDisable() {
        Rain.OnRainEvent -= OnRain;
    }

    private void OnRain(bool isRaining) {
        if (!isRaining) { return; }
        onRain?.Invoke();
        if (destroyAfterInvoke) {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
