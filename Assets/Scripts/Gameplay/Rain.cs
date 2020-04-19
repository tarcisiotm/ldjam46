using System.Collections;
using UnityEngine;

public class Rain : MonoBehaviour
{
    [SerializeField] float duration = 5f;
    [SerializeField] float delayToNotify = 1f;

    public delegate void RainEvent(bool isRaining);
    public static event RainEvent OnRainEvent;
    
    private void OnEnable() {
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
        //fadeout rain
        gameObject.SetActive(false);
    }

    void Start()
    {
        
    }

}