using UnityEngine;

public class TreeController : MonoBehaviour
{
    [SerializeField] float water = 100;

    [SerializeField] float waterPerSecRaining = 5;
    [SerializeField] float waterPerSecDraining = .5f;

    [SerializeField] float sunPerSecDay = 5;
    [SerializeField] float sunPerSecDraining = .5f;

    [SerializeField] float sun = 50;
    [SerializeField] float health = 100;

    bool isDay = false;
    bool raining = false;
    float deltaTime = 0f;

    void Start()
    {
        Rain.OnRainEvent += OnRainEvent;   
    }

    void Update() {
        deltaTime = Time.deltaTime;
        UpdateWater();
        UpdateSun();
    }

    private void OnRainEvent(bool isRaining) {
        raining = isRaining;
    }

    void UpdateSun() {
        if (isDay && !raining) {
            sun += deltaTime * sunPerSecDay;
            return;
        }
        sun -= deltaTime * sunPerSecDraining;
    }

    void UpdateWater() {
        if (raining) {
            water += deltaTime * waterPerSecRaining;
            return;
        }
        water -= deltaTime * waterPerSecDraining;
    }



}
