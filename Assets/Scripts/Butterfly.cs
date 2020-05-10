using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TG.Core;

public class Butterfly : MonoBehaviour, IPoolingItem
{
    [SerializeField] Color[] colors = null;

    [SerializeField] Vector2 lookAtTime = new Vector2(1, 3);
    [SerializeField] Vector2 metersPerSecond = new Vector2(.5f, 1f);

    [SerializeField] float speedScale = .1f;

    [SerializeField] Vector2 idleTime = new Vector2(2.5f, 5f);

    private Renderer targetRenderer;
    private MaterialPropertyBlock propertyBlock;
    //IEnumerator Start() {
    //    yield return null;
    //    GetNewTarget();
    //}

    void GetNewTarget()
    {
        MoveTo(FindObjectOfType<WaypointManager>().GetRandomWaypoint());
    }

    public void MoveTo(Transform targetTransform)
    {
        float lookTime = Random.Range(lookAtTime.x, lookAtTime.y);
        transform.DOLookAt(targetTransform.position, lookTime, AxisConstraint.None, Vector3.up);

        float dist = Vector3.Distance(targetTransform.position, transform.position);
        float duration = dist / (speedScale * Random.Range(metersPerSecond.x, metersPerSecond.y));
        transform.DOLocalMove(targetTransform.position, duration).SetDelay(lookTime * .5f).OnComplete(OnComplete).SetEase(Ease.Linear);
    }

    void OnComplete()
    {
        transform.DOLocalMoveX(transform.position.x, Random.Range(idleTime.x, idleTime.y)).OnComplete(GetNewTarget);
    }

    public void Die()
    {
        DOTween.Kill(transform);
        transform.DOScale(Vector3.one * .001f, .5f).OnComplete(Disable);
    }

    private void OnEnable()
    {
        propertyBlock = new MaterialPropertyBlock();
        targetRenderer = GetComponentInChildren<Renderer>();

        // Get the current value of the material properties in the renderer.
        targetRenderer.GetPropertyBlock(propertyBlock);
        // Assign our new value.
        propertyBlock.SetColor("_Color", colors[Random.Range(0, colors.Length)]);
        // Apply the edited values to the renderer.
        targetRenderer.SetPropertyBlock(propertyBlock);
    }

    void Disable()
    {
        gameObject.SetActive(false);
        propertyBlock = null;
        targetRenderer = null;
    }

    void IPoolingItem.Reset()
    {


        GetNewTarget();
    }

}
