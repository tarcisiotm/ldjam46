using System.Collections;
using UnityEngine;

public class Rain : MonoBehaviour
{
    [SerializeField] float duration = 5f;
    [SerializeField] float delayToNotify = 1f;

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

    IEnumerator DoRain() {
        float currentDuration = duration;

        yield return new WaitForSeconds(delayToNotify);

        OnRainEvent?.Invoke(true);

        while (currentDuration > 0) {
            currentDuration -= Time.deltaTime;
            yield return null;
        }

        OnRainEvent?.Invoke(false);
        float dur = 0;

        foreach(var p in particleSystems) {
            dur = p.main.duration > dur ? p.main.duration : dur;
            p.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        }

        yield return new WaitForSeconds(dur);
        gameObject.SetActive(false);
    }

    void Start()
    {
        
    }

}