using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Rain : MonoBehaviour
{
    [SerializeField] float duration = 5f;
    [SerializeField] float delayToNotify = 1f;
    [SerializeField] AudioClip rainClip = default;

    public delegate void RainEvent(bool isRaining);
    public static event RainEvent OnRainEvent;

    ParticleSystem[] particleSystems;
    
    private void OnEnable() {
        particleSystems = gameObject.GetComponentsInChildren<ParticleSystem>();
        foreach (var p in particleSystems) {
            p.Play();
        }
        StartCoroutine(DoRain());
    }

    void Start() { }

    IEnumerator DoRain() {
        float currentDuration = duration;

        var audioSource = PlayAudio();

        yield return new WaitForSeconds(delayToNotify);

        OnRainEvent?.Invoke(true);

        while (currentDuration > 0) {
            currentDuration -= Time.deltaTime;
            yield return null;
        }

        OnRainEvent?.Invoke(false);
        float dur = 0;

        audioSource.DOFade(0, 1f);

        yield return new WaitForSeconds(.7f);

        foreach (var p in particleSystems) {
            dur = p.main.duration > dur ? p.main.duration : dur;
            p.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        }


        yield return new WaitForSeconds(dur);
        gameObject.SetActive(false);
    }

    AudioSource PlayAudio() {
        Vector3 pos = new Vector3(transform.position.x,
                                                    1f,
                                                    transform.position.z);

        var audio = GameplayManager.I.GetAudioFromPool(pos);

        audio.PlayAndDisable(rainClip, 0);
        var audioSource = audio.GetComponentInChildren<AudioSource>();
        audioSource.DOFade(1, .3f);
        return audioSource;
    }

}