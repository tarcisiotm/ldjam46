using UnityEngine;

public class TreeUpgrade : MonoBehaviour
{
    [SerializeField] TreeUpgradeData[] treeUpgrades = default;
    [SerializeField] float fadeInDuration = 2f;
    [SerializeField] float stepDuration = 2f;

    TreeUpgradeData currentUpgrade = default;

    [Header("Debug")]
    [SerializeField] int currentIndex = -1;

    void Start()
    {
        if (treeUpgrades.Length > 0) {
            currentUpgrade = treeUpgrades[0];
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) { Upgrade(); }
    }

    public void Upgrade() {

        if(currentIndex + 1 >= treeUpgrades.Length && currentUpgrade.IsDone) { return; }

        if (currentUpgrade.IsDone) {
            currentUpgrade.Deactivate();
            currentUpgrade = treeUpgrades[++currentIndex];
            currentUpgrade.Activate(fadeInDuration);
        } else {
            currentUpgrade.DoNextStep(stepDuration);
        }

    }
}
