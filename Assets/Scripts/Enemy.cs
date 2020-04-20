using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    [SerializeField] float timePerMeter = 1f;
    [SerializeField] float rotationDuration = .5f;
    [SerializeField] float distanceToAttack = .5f;

    [SerializeField] float attackCooldown = 3f;

    [Header("References")]
    [SerializeField] Animator animator = default;
    [SerializeField] Transform[] waypoints = default;
    //walk loop?
    [SerializeField] AudioClip attackClip = default;
    [SerializeField] AudioClip[] dieClips = default;

    [Header("Debug")]
    [SerializeField] int currentWaypointIndex = -1;

    Transform currentWaypoint = null;
    Transform originalParent = null;
    Vector3 targetPos;

    bool isAttacking = false;

    const string ANIM_WALK_FORWARD = "Walk Forward";
    const string ANIM_ATTACK_STAB = "Stab Attack";
    const string ANIM_ATTACK_SMASH = "Smash Attack";
    const string ANIM_DIE = "Die";

    Coroutine attackRoutine;

    void Start()
    {
        //transform.LookAt(waypoints[0]);
        animator.SetBool(ANIM_WALK_FORWARD, true);
        ExecuteWaypoint();
    }

    //void Update()
    //{
    //    if (!isAttacking) { return; }
    //}

    #region Waypoints
    void ExecuteWaypoint() {
        currentWaypointIndex++;

        if(currentWaypointIndex >= waypoints.Length) { return; }
        currentWaypoint = waypoints[currentWaypointIndex];

        float distance = Vector3.Distance(transform.localPosition, currentWaypoint.localPosition);

        targetPos = currentWaypointIndex + 1 == waypoints.Length ?
            Vector3.Lerp(transform.localPosition, currentWaypoint.localPosition, (distance - distanceToAttack) / distance ) :
            currentWaypoint.localPosition;

        float duration = distance / timePerMeter;

        transform.DOLocalMove(targetPos, 3f).OnComplete(DoRotation).SetEase(Ease.Linear);
    }

    void DoRotation() {
        if (currentWaypointIndex + 1 == waypoints.Length) {
            animator.SetBool(ANIM_WALK_FORWARD, false);
            Attack();
            return;
        }

        originalParent = transform.parent;
        transform.SetParent(currentWaypoint, true);
        transform.DOLocalRotate(Quaternion.identity.eulerAngles, rotationDuration).OnComplete(OnRotationCompleted);
    }

    void OnRotationCompleted() {
        transform.SetParent(originalParent, true);

        ExecuteWaypoint();
    }
    #endregion Waypoints

    IEnumerator DoAttack() {
        while (isAttacking) {
            yield return new WaitForSeconds(attackCooldown/2f);
            animator.SetTrigger(GetRandomAttack());
            yield return new WaitForSeconds(attackCooldown / 2f);
            //var audio = GameplayManager.I.GetAudioFromPool(transform.position);
            //audio.PlayAndDisable(attackClip, 1);
            //send signal to tree
        }

    }

    void Attack() {
        isAttacking = true;
        attackRoutine = StartCoroutine(DoAttack());
    }

    string GetRandomAttack() {
        float rand = Random.Range(0, 1);
        return rand < .5f ? ANIM_ATTACK_STAB : ANIM_ATTACK_SMASH;
    }

    private void OnTriggerEnter(Collider other) {

        if(other.GetComponentInParent<LightningController>() != null) {
            Die();
        }
        //Debug.Log(other.gameObject.name);
    }

    private void Die() {
        if (isAttacking) {
            isAttacking = false;
            StopCoroutine(attackRoutine);
            attackRoutine = null;
        }

        animator.SetTrigger(ANIM_DIE);

        var audio = GameplayManager.I.GetAudioFromPool(transform.position);
        int index = Random.Range(0, dieClips.Length);

        audio.PlayAndDisable(dieClips[index], 1);

        StartCoroutine(DoDie());
    }

    IEnumerator DoDie() {
        yield return new WaitForSeconds(2f);
        transform.DOLocalMoveY(transform.localPosition.y - .5f, 2f);
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}