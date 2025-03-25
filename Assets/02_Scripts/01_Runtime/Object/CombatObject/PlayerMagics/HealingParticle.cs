using System;
using System.Collections;
using MinD.Runtime.Entity;
using UnityEngine;
using UnityEngine.Serialization;

public class HealingParticle : MonoBehaviour
{
    [SerializeField] private GameObject healingLightFx;
    [SerializeField] private GameObject magicCircleFx;
    [SerializeField] private GameObject healingParticleFx;

    private ParticleSystem healingPArticlePc;
    private ParticleSystem healLightPc;
    private ParticleSystem maigcCiclePc;
    
    [SerializeField] private float healFloat; // heal amount per second
    
    private Player castplayer;
    private void OnEnable()
        {
            maigcCiclePc = magicCircleFx.GetComponent<ParticleSystem>();
            healLightPc = healingLightFx.GetComponent<ParticleSystem>();
            healingPArticlePc = healingParticleFx.GetComponent<ParticleSystem>();
            
            maigcCiclePc.Play();
    
        }
    public void PlayHeal(Player player)
    {
        castplayer = player;

        healingPArticlePc.Play();
        healLightPc.Play();
        StartCoroutine(Healing());
    }

    private IEnumerator Healing()
    {
        float elapsedTime = 0;
        float healValueStack = 0;

        while (true)
        {
            elapsedTime += Time.deltaTime;

            healValueStack += healFloat * Time.deltaTime;
            while (healValueStack > 1)
            {
                castplayer.CurHp += 1;
                healValueStack -= 1;
            }
            
            
            if (castplayer.CurHp >= castplayer.attribute.MaxHp)
            {
                castplayer.CurHp = castplayer.attribute.MaxHp;
            }
            yield return null;
            
            if (elapsedTime >= 3)
            {
                Exit();
                yield break;
            }

        }
        
    }

    public void Exit()
    {
        magicCircleFx.SetActive(false);
        Destroy(gameObject,2);
    }
    
}
