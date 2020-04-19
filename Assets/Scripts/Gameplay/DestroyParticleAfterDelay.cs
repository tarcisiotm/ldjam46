using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DestroyParticleAfterDelay : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine(DoDestroyParticle());
    }

    IEnumerator DoDestroyParticle() {
        yield return new WaitForSeconds(GetComponentInChildren<ParticleSystem>().main.duration+.2f);
        Destroy(gameObject);
    }

}