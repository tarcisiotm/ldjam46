using System.Collections;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    [SerializeField] float water = 100;

    [SerializeField] float waterPerSecRaining = 5;
    [SerializeField] float waterPerSecIdle = .5f;

    [SerializeField] TreeUpgrade treeUpgrade;

    TreeUpgradeData upgradeData;
    ScaleTween scaleTween;

    bool raining = false;
    bool isDone = false;
    bool hasInit = false;

    float deltaTime = 0f;

    IEnumerator Start()
    {
        yield return null;
        Rain.OnRainEvent += OnRainEvent;
        upgradeData = treeUpgrade.CurrentUpgradeData;
        scaleTween = upgradeData.GetScaleTween();
        hasInit = true;
    }

    void Update() {
        if (!hasInit || isDone) { return; }
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
            scaleTween.transform.localScale = Vector3.Lerp(scaleTween.initialScale, scaleTween.finalScale, water / 100f);

            if (water == 100) {
                treeUpgrade.Upgrade();
                upgradeData = treeUpgrade.CurrentUpgradeData;
                scaleTween = upgradeData.GetScaleTween();
                //scale?
                water = 65;
            }

            return;
        }

        water += deltaTime * waterPerSecIdle;

        scaleTween.transform.localScale = Vector3.Lerp(scaleTween.initialScale, scaleTween.finalScale, water / 100f);
    }



}
