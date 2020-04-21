using UnityEngine;

public class TreeController : MonoBehaviour
{
    [SerializeField] float water = 100;

    [SerializeField] float waterPerSecRaining = 5;
    [SerializeField] float waterPerSecDraining = .5f;

    [SerializeField] TreeUpgrade treeUpgrade;

    bool raining = false;
    bool isDone = false;
    float deltaTime = 0f;

    void Start()
    {
        Rain.OnRainEvent += OnRainEvent;   
    }

    void Update() {
        if (isDone) { return; }
        deltaTime = Time.deltaTime;
        UpdateWater();
    }

    public void SetIsDone() {
        water = 100;
        isDone = true;
    }

    private void OnRainEvent(bool isRaining) {
        raining = isRaining;
    }

    void UpdateWater() {
        if (raining) {
            water += deltaTime * waterPerSecRaining;
            water = Mathf.Clamp(water, 0, 100);

            if(water == 100) {
                treeUpgrade.Upgrade();
                water = 65;
            }

            return;
        }
        water -= deltaTime * waterPerSecDraining;

        if(water < 0) {
            // gameOver state
        }
    }



}
