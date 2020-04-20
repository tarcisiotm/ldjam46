using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScaleOnEnable : MonoBehaviour
{
    [SerializeField] Vector3 finalScale = default;
    [SerializeField] float duration = 3f;
    [SerializeField] float delay = 1f;

    void OnEnable()
    {
        transform.DOScale(finalScale, duration).SetDelay(delay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
