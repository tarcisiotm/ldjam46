using UnityEngine;

/// <summary>
/// A simple BGM handler script to change the music
/// </summary>
public class ChangeBGMOnEnable : MonoBehaviour
{
    [SerializeField] AudioClip bgmClip = default;
    [SerializeField] float volume = 1;
    [SerializeField] bool autoDestroy = true;

    public void PlayBGM() {
        AudioManager.I.PlayBGM(bgmClip, volume);
    }

    private void Start() {
        PlayBGM();
        if (autoDestroy) {
            Destroy(gameObject);
        }
    }

}