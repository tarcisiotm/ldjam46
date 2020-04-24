using TG.Core;
using UnityEngine;

public class TreeUpgrade : MonoBehaviour {
    [SerializeField] TreeUpgradeData[] treeUpgrades = default;
    [SerializeField] float fadeInDuration = 2f;
    [SerializeField] float stepDuration = 2f;
    [SerializeField] GameObject fadeGO = default;
    TreeUpgradeData currentUpgrade = default;

    public TreeUpgradeData CurrentUpgradeData => currentUpgrade;

    [Header("Debug")]
    [SerializeField] int currentIndex = -1;

    bool isDone = false;

    void Start() {
        if (treeUpgrades.Length > 0) {
            currentUpgrade = treeUpgrades[0];
        }
    }

#if UNITY_EDITOR
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) { Upgrade(); }
        if (Input.GetKeyDown(KeyCode.Return)) { DeUpgrade(); }
    }
#endif

    public void Upgrade() {
        if (isDone) { return; }

        //if is final step next and enemy is alive

        if (currentIndex + 1 >= treeUpgrades.Length && currentUpgrade.IsDone) { return; }

        if (currentUpgrade.IsDone) {
            currentUpgrade.Deactivate();
            currentUpgrade = treeUpgrades[++currentIndex];
            currentUpgrade.Activate(fadeInDuration);
            fadeGO.SetActive(true);
        } else {
            currentUpgrade.DoNextStep(stepDuration);
        }
    }

    public void SetIsDone() {
        isDone = true;
    }

    public void DeUpgrade() {
        if (currentIndex - 1 < 0) {
            ScenesManager.I.ReloadScene();
            return;
        }

        if (isDone) { return; }

        currentIndex--;

        currentUpgrade.Deactivate();
        currentUpgrade = treeUpgrades[currentIndex];
        currentUpgrade.ActivateReverse(fadeInDuration);

        fadeGO.SetActive(true);

    }
}
