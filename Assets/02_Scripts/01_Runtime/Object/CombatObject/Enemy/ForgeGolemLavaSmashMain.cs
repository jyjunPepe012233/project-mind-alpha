using System;
using MinD.Runtime.Entity;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class ForgeGolemLavaSmashMain : MonoBehaviour
{
    [SerializeField] private float summonRadius;
    [SerializeField] private int lavaMaxCount;
    [SerializeField] private float lavaSummonDuration;
    [SerializeField] private GameObject lavaPrefab;
    
    private int currentLavaCount;
    private float summonTimer;

    private BaseEntity owner;

    public void SetOwner(BaseEntity owner)
    {
        this.owner = owner;
    }
    
    private void Update()
    {
        summonTimer += Time.deltaTime;
        
        while (Mathf.Lerp(0, lavaMaxCount, summonTimer / lavaSummonDuration) > currentLavaCount)
        {
            SummonLava();
            if (currentLavaCount >= lavaMaxCount)
            {
                Destroy(gameObject);
            }
        }
    }

    private void SummonLava()
    {
        currentLavaCount++;

        Vector2 summon2DPosition = Random.insideUnitCircle * summonRadius;
        Vector3 summonPosition = transform.position + new Vector3(summon2DPosition.x, 0, summon2DPosition.y);
        Debug.DrawLine(transform.position, summonPosition, Color.white, 3);
        if (NavMesh.SamplePosition(summonPosition, out NavMeshHit hitInfo, 1.5f, NavMesh.AllAreas))
        {
            GameObject lava = Instantiate(lavaPrefab, hitInfo.position, Quaternion.identity);
            lava.GetComponent<DreadDwarfMinerQuake>().SetOwner(owner);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, summonRadius);
    }
}
