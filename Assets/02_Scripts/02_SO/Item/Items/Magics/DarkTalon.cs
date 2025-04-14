using System.Collections;
using MinD.Runtime.Object.Magics;
using MinD.Runtime.Utils;
using MinD.SO.Object;
using UnityEngine;

namespace MinD.SO.Item.Items
{
    [CreateAssetMenu(menuName = "MinD/Item/Items/Magics/Dark Talon")]
    public class DarkTalon : Magic
    {
        [Header("[ Settings ]")]
        [SerializeField] private DamageData damageData;
        [SerializeField] private GameObject thornPrefab;

        private int thornCount = 5;
        private float thornInterval = 0.3f;
        private float thornSpacing = 1.2f;

        private Transform spawnRoot;
        private Coroutine spawnRoutine;

        public override void OnUse()
        {
            castPlayer.animation.PlayTargetAction("DarkTalon_Start", true, true, false, false);
        }

        private IEnumerator SpawnThorns()
        {
            float[] thornScales = { 0.4f, 0.8f, 1.2f, 1.6f, 2f };

            if (spawnRoot == null) yield break;

            for (int i = 0; i < thornCount; i++)
            {
                GameObject thorn = Instantiate(thornPrefab);

                // Thorn 크기 설정
                int scaleIndex = Mathf.Min(i, thornScales.Length - 1);
                float scale = thornScales[scaleIndex];
                var thornScript = thorn.GetComponent<DarkTalonThorn>();
                thornScript?.Initialize(scale);

                thorn.transform.position = spawnRoot.position + spawnRoot.forward * (i * thornSpacing);
                thorn.transform.rotation = spawnRoot.rotation;

                // 데미지 설정
                var damageCollider = thorn.GetComponentInChildren<DamageCollider>();
                if (damageCollider != null)
                {
                    damageCollider.soData = damageData;
                    damageCollider.blackList.Add(castPlayer);
                }

                yield return new WaitForSeconds(thornInterval);
            }

            spawnRoutine = null;
        }

        public override void OnExit()
        {
            if (spawnRoot != null)
            {
                Destroy(spawnRoot.gameObject);
                spawnRoot = null;
            }
        }

        public override void OnCancel()
        {
            if (spawnRoutine != null)
            {
                castPlayer.StopCoroutine(spawnRoutine);
                spawnRoutine = null;
            }

            if (spawnRoot != null)
            {
                Destroy(spawnRoot.gameObject);
                spawnRoot = null;
            }
        }

        public override void OnSuccessfullyCast()
        {
            if (spawnRoutine != null) return;

            // 생성 위치 및 방향 설정
            Vector3 startPos = castPlayer.transform.position + Vector3.up * 0.1f + castPlayer.transform.forward * 2f;
            Vector3 direction = castPlayer.transform.forward;

            // 스폰 루트 설정
            spawnRoot = new GameObject("DarkTalon_SpawnRoot").transform;
            spawnRoot.position = startPos;
            spawnRoot.rotation = Quaternion.LookRotation(direction);

            spawnRoutine = castPlayer.StartCoroutine(SpawnThorns());
        }

        public override void Tick()
        {
            if (!castPlayer.isPerformingAction && castPlayer.combat.currentCastingMagic == this)
            {
                castPlayer.combat.ExitCurrentMagic();
            }
        }

        public override void OnReleaseInput() { }
    }
}