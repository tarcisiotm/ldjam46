using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Provides functionality for audio playback on UI buttons
/// </summary>
public class AudioButton : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] AudioSource audioSource = default;
    [SerializeField] AudioClip onHover = default;
    [SerializeField] float onHoverVolume = .6f;
    [SerializeField] float onClickVolume = .6f;
    [SerializeField] AudioClip clickClip = default;

    private void Awake()
    {

        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        if (clickClip == null) { return; }

        audioSource.volume = onClickVolume;
        audioSource.PlayOneShot(clickClip);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(onHover == null) { return; }

        audioSource.volume = onHoverVolume;
        audioSource.PlayOneShot(onHover);
    }
}
