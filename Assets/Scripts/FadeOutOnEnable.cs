using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeOutOnEnable : MonoBehaviour
{
    [SerializeField] float duration = .3f; 
    [SerializeField] float delay = 0f;
    [SerializeField] Image image = default; 

    private void OnEnable() {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        image.DOFade(0, duration).SetDelay(delay).OnComplete(OnComplete);
    }

    void OnComplete() {
        gameObject.SetActive(false);
    }
}
