using System;
using System.Collections;
using MinD.Interfaces;
using MinD.Runtime.Entity;
using MinD.Runtime.Managers;
using MinD.Runtime.Object;
using MinD.SO.Game;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class BossRoomEntrance : Interactable, IWorldIndexable
{
    private float fadeInTime = 1f;
    private float fadeOutTime = 1f;
    
    public event Action<Enemy, BossFightInfoSo> OnEnter;
    
    [SerializeField] private Enemy boss;
    [SerializeField] private BossFightInfoSo bossFightInfo; 
    [SerializeField] private GameObject obstacleTransform;
    [SerializeField] private Transform reachTransform;

    [SerializeField] private UnityEvent onBossFelled;
    
    private bool isCleared = false;
    public bool IsCleared => isCleared;

    [SerializeField, HideInInspector] private bool _hasBeenIndexed;
    [SerializeField, HideInInspector] private int _worldIndex;
    
    public bool hasBeenIndexed
    {
        get => _hasBeenIndexed;
        set => _hasBeenIndexed = value;
    }
    public int worldIndex
    {
        get => _worldIndex;
        set => _worldIndex = value;
    }

    public override void Interact(Player interactor)
    {
        OnEnter?.Invoke(boss, bossFightInfo);

        canInteraction = false;
        
        interactor.interaction.RemoveInteractableInList(this);
        interactor.interaction.RefreshInteractableList();

        StartCoroutine(PlayEnteringBossRoomAction());
        
        BossFightManager.Instance.OnBossFightFinish += OnBossFightFinish;
    }

    private IEnumerator PlayEnteringBossRoomAction()
    {
        Player.player.animation.PlayTargetAction("Anchor_Discover", true, true, false, false);
        
        PlayerHUDManager.Instance.FadeInToBlack(fadeInTime);
        yield return new WaitForSeconds(fadeInTime);
        
        Player.player.cc.enabled = false;
        Player.player.transform.SetPositionAndRotation(reachTransform.position, reachTransform.rotation);
        Player.player.cc.enabled = true;
        yield return new WaitForSeconds(1f);
        
        PlayerHUDManager.Instance.FadeOutFromBlack(fadeOutTime);
    }

    public void LoadBossData(bool hasBeenFelled)
    {
        obstacleTransform?.SetActive(!hasBeenFelled);
        GetComponent<Collider>().enabled = !hasBeenFelled;
    }

    private void OnBossFightFinish(bool isBossFelled)
    {
        if (isBossFelled)
        {
            LoadBossData(true);
            isCleared = true;
            
            onBossFelled?.Invoke();
        }

        BossFightManager.Instance.OnBossFightFinish -= OnBossFightFinish;
    }
}
