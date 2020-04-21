using UnityEngine;
using DG.Tweening;

public class EndGameCanvas : MonoBehaviour
{
    [SerializeField] float fadeDuration = .5f;
    [SerializeField] float moveDuration = .5f;
    [SerializeField] float fullyFadedDuration = 5f;
    [SerializeField] float finalY = 5f;

    [SerializeField] RectTransform targetRectTransform;
    [SerializeField] CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup.DOFade(1, fadeDuration).OnComplete(OnFadedIn);
        targetRectTransform.DOLocalMoveY(finalY, moveDuration);
    }

    void OnFadedIn() {
        canvasGroup.DOFade(0, fullyFadedDuration).SetDelay(fullyFadedDuration);
    }

}