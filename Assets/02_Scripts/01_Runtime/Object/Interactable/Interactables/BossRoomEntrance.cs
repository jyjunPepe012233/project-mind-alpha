using System;
using System.Collections;
using MinD.Interfaces;
using MinD.Runtime.Entity;
using MinD.Runtime.Managers;
using MinD.Runtime.Object;
using MinD.SO.Game;
using UnityEngine;
using UnityEngine.Serialization;

public class BossRoomEntrance : Interactable, IWorldIndexable
{
    private float fadeInTime = 1f;
    private float fadeOutTime = 1f;
    
    public event Action<Enemy, BossFightInfoSo> OnEnter;
    
    [SerializeField] private Enemy boss;
    [SerializeField] private BossFightInfoSo bossFightInfo;
    [SerializeField] private GameObject gateTransform;
    [SerializeField] private Transform reachTransform;

    [HideInInspector] public bool isFelled;

    private bool _hasBeenIndexed;
    private int _worldIndex;
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

    private void Awake()
    {
        boss.dieAction += FellBoss;
    }

    public override void Interact(Player interactor)
    {
        OnEnter?.Invoke(boss, bossFightInfo);
        interactor.interaction.RemoveInteractableInList(this);
        interactor.interaction.RefreshInteractableList();

        StartCoroutine(EnterBossRoomCoroutine());
    }

    private void FellBoss()
    {
        isFelled = true;
    }

    private IEnumerator EnterBossRoomCoroutine()
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
        gateTransform?.SetActive(hasBeenFelled);
        GetComponent<Collider>().enabled = false;
    }
    
}
