using System.Collections;
using System.Collections.Generic;
using TG.Core;
using TG.Core.Audio;
using UnityEngine;

/// <summary>
/// Handles gameplay functionality
/// </summary>
public class GameplayManager : Singleton<GameplayManager>
{
    [Header("References")]
    [SerializeField] GameObject audioTemplate = default;
    [SerializeField] HUDManager hudManager = default;

    [SerializeField] Camera mainCamera = default;

    PoolingManager poolingManager = default;
    GameObject auxGO = default;

    IEnumerator Start()
    {
        yield return null;
        yield return null;
        poolingManager = PoolingManager.I;
    }

    public void CreateObjectFromPool(GameObject prefab, Vector3 pos) {
        auxGO = poolingManager.GetPooledObject(prefab);
        auxGO.transform.position = pos;
        auxGO.SetActive(true);
    }

    public PlayAudioAndDisable GetAudioFromPool(Vector3 pos) {
        return GetObjectFromPool<PlayAudioAndDisable>(audioTemplate, pos);
    }

    public T GetObjectFromPool<T>(GameObject prefab, Vector3 pos, bool setActive = true) where T : Component{
        auxGO = poolingManager.GetPooledObject(prefab);
        auxGO.transform.position = pos;
        T obj = auxGO.GetComponent<T>();
        auxGO.SetActive(setActive);
        return obj;
    }

    public void ParentToCamera(Transform newChild) {
        newChild.SetParent(mainCamera.transform, true);
    }

    #region Player
    //public Vector3 GetPlayerPos() {
        //return Player.transform.position;
    //}

    IEnumerator DoRespawn() {
        yield return new WaitForSeconds(3);
        //Player.gameObject.SetActive(true);
        //Player.Respawn(pilot.transform);
    }

    #endregion Player

    #region HUD
    public void IncrementScore(int points) {
        //Score += points;
        //hudManager.UpdateScore(Score);
    }

    void LoseLife() {
        hudManager.LoseLife();
    }
    #endregion HUD

    public void GameOver() {
        hudManager.Save();
        StartCoroutine(DelayGameOver());
    }

    IEnumerator DelayGameOver() {
        yield return new WaitForSeconds(1);
        hudManager.ShowGameOver();
    }

}