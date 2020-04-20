using UnityEngine;
using DG.Tweening;

public class MoveOnEnable : MonoBehaviour
{
    [SerializeField] Vector3 finalLocalPos = default;
    [SerializeField] float duration = 3f;
    [SerializeField] float delay = 1f;

    void OnEnable() {
        transform.DOLocalMove(finalLocalPos, duration).SetDelay(delay);
    }
}
