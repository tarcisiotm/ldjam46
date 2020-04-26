using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnedObjectsUnityEvents : MonoBehaviour
{
    [SerializeField] bool destroyAfterInvoke = true;
    [SerializeField] Spawner spawner;
    [Space]
    [SerializeField] int smallTarget = -1;
    [SerializeField] int mediumTarget = -1;
    [SerializeField] int bigTarget = -1;
    [SerializeField] int waterTarget = -1;
    [SerializeField] int nearWaterTarget = -1;
    [Space]
    [SerializeField] UnityEvent onTargetReached;

    bool hasTriggered = false;

    private void OnEnable() {
        spawner.OnSpawned += OnSpawned;
    }

    private void OnDisable() {
        spawner.OnSpawned -= OnSpawned;
    }

    private void OnSpawned(int smallTotal, int mediumTotal, int bigTotal, int waterTotal, int nearWaterTotal) {
        if(smallTarget > 0 && smallTarget == smallTotal) { TriggerEvent(); }
        if(mediumTarget > 0 && mediumTarget == mediumTotal) { TriggerEvent(); }
        if(bigTarget > 0 && bigTarget == bigTotal) { TriggerEvent(); }
        if(waterTarget > 0 && waterTarget == waterTotal) { TriggerEvent(); }
        if(nearWaterTarget > 0 && nearWaterTarget == nearWaterTotal) { TriggerEvent(); }

        if (hasTriggered && destroyAfterInvoke) {
            Destroy(gameObject);
        }
    }

    void TriggerEvent() {
        hasTriggered = true;
        onTargetReached?.Invoke();
    }

}