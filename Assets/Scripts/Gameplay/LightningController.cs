using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class LightningController : MonoBehaviour
{
    [SerializeField] float fallDuration = .3f;
    [SerializeField] float duration = 1f;

    [SerializeField] GameObject parentGO = null;
    [SerializeField] GameObject startPosGO = null;
    [SerializeField] GameObject endPosGO = null;

    [SerializeField] UnityEvent onComplete = null;

    bool hasInit = false;
    float originalStartY;

    void Start(){}

    public void DoLightning(Vector3 basePos) {
        if (!hasInit) {
            originalStartY = startPosGO.transform.position.y;
            hasInit = true;
        }

        parentGO.transform.position = basePos;
        startPosGO.transform.position = new Vector3(startPosGO.transform.position.x,
                                                    originalStartY,
                                                    startPosGO.transform.position.z);

        endPosGO.transform.position = startPosGO.transform.position;
        parentGO.SetActive(true);
        endPosGO.transform.DOLocalMoveY(0, fallDuration);
        startPosGO.transform.DOLocalMoveY(0, .1f).SetDelay(fallDuration + duration).OnComplete(OnComplete);
    }

    void OnComplete() {
        onComplete?.Invoke();
        gameObject.SetActive(false);
    }
}
