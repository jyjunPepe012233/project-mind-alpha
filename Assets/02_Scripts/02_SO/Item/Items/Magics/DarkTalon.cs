using System.Collections;
using MinD.Runtime.Object.Magics;
using MinD.Runtime.Utils;
using MinD.SO.Object;
using UnityEngine;

namespace MinD.SO.Item.Items {
    /// <summary>
    /// 'Dark Talon' 마법 스킬 ScriptableObject.
    /// 전방으로 일정 간격으로 가시(Thorn)를 소환하여 피해를 주는 마법.
    /// </summary>
    [CreateAssetMenu(menuName = "MinD/Item/Items/Magics/Dark Talon")]
    public class DarkTalon : Magic {
        [Header("[ Settings ]")]
        [SerializeField] private DamageData damageData;       // 데미지 정보
        [SerializeField] private GameObject thornPrefab;      // 소환할 가시 프리팹

        private int thornCount = 5;                           // 가시 개수
        private float thornInterval = 0.5f;                   // 소환 간격
        private float thornSpacing = 3f;                      // 가시 간격 거리

        private Transform spawnRoot;                          // 가시 시작 위치 기준 Transform
        private Coroutine spawnRoutine;

        public override void OnUse() {
            castPlayer.animation.PlayTargetAction("DarkTalon_Start", true, true, false, false);
        }

        /// <summary>
        /// 가시를 일정 간격으로 전방에 생성.
        /// </summary>
        private IEnumerator SpawnThorns() {
            float[] thornScales = { 0.4f, 0.8f, 1.2f, 1.6f, 2f };

            if (spawnRoot == null) yield break;

            for (int i = 0; i < thornCount; i++) {
                GameObject thorn = Instantiate(thornPrefab);

                // 크기 설정
                float scale = thornScales[Mathf.Min(i, thornScales.Length - 1)];
                if (thorn.TryGetComponent(out DarkTalonThorn thornScript)) {
                    thornScript.Initialize(scale);
                }

                // 위치 및 회전 설정
                thorn.transform.position = spawnRoot.position + spawnRoot.forward * (i * thornSpacing);
                thorn.transform.rotation = spawnRoot.rotation;

                // 데미지 세팅
                var damageCollider = thorn.GetComponentInChildren<DamageCollider>();
                if (damageCollider != null) {
                    damageCollider.soData = damageData;
                    damageCollider.blackList.Add(castPlayer);  // 자신에게는 데미지 안 들어가도록
                }

                yield return new WaitForSeconds(thornInterval);
            }

            spawnRoutine = null;
        }

        public override void OnSuccessfullyCast() {
            if (spawnRoutine != null) return;

            Vector3 startPos = castPlayer.transform.position + Vector3.up * 0.1f + castPlayer.transform.forward * 2f;
            Vector3 direction = castPlayer.transform.forward;

            // 기준 위치 객체 생성
            spawnRoot = new GameObject("DarkTalon_SpawnRoot").transform;
            spawnRoot.position = startPos;
            spawnRoot.rotation = Quaternion.LookRotation(direction);

            // 소환 시작
            spawnRoutine = castPlayer.StartCoroutine(SpawnThorns());
        }

        public override void OnCancel() {
            if (spawnRoutine != null) {
                castPlayer.StopCoroutine(spawnRoutine);
                spawnRoutine = null;
            }

            if (spawnRoot != null) {
                Destroy(spawnRoot.gameObject);
                spawnRoot = null;
            }
        }

        public override void OnExit() {
            if (spawnRoot != null) {
                Destroy(spawnRoot.gameObject);
                spawnRoot = null;
            }
        }

        public override void Tick() {
            // 마법 도중 중단되면 종료
            if (!castPlayer.isPerformingAction && castPlayer.combat.currentCastingMagic == this) {
                castPlayer.combat.ExitCurrentMagic();
            }
        }

        public override void OnReleaseInput() { }
    }
}
