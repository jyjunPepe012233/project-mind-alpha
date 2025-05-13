using MinD.Runtime.Entity;
using MinD.SO.Item;
using UnityEngine;

namespace MinD.Runtime.Object.Magics {
    /// <summary>
    /// '증오의 불꽃 - 폭' 마법.
    /// 장판이 있는 위치에 폭발을 생성하거나, 장판이 없을 경우 플레이어 전방에 생성.
    /// 타겟이 있을 경우 타겟 위치에 폭발을 발생시킴.
    /// </summary>
    public class FlameOfHatredExplosion : Magic {
        [Header("[ Explosion Settings ]")]
        [SerializeField] private GameObject explosionPrefab; // 폭발 이펙트 프리팹
        [SerializeField] private float defaultOffset = 5f;    // 장판이 없을 때 전방 거리

        public override void OnUse() {
            castPlayer.animation.PlayTargetAction("FlameOfHatredExplosion", true, true, false, false);
        }

        public override void OnSuccessfullyCast() {
            BaseEntity caster = castPlayer.GetComponent<BaseEntity>();

            // 1. 장판이 있으면 해당 위치에서 폭발 발생
            if (FlameOfHatredZone.ActiveZones.Count > 0) {
                foreach (var zone in FlameOfHatredZone.ActiveZones.ToArray()) {
                    if (zone == null) continue;

                    SpawnExplosion(zone.transform.position, caster);
                    Destroy(zone.gameObject);
                }

                FlameOfHatredZone.ActiveZones.Clear();
            } 
            // 2. 타겟이 있다면 타겟 위치에서 폭발 발생
            else if (castPlayer.combat.target != null) {
                Vector3 targetPos = castPlayer.combat.target.transform.position;
                SpawnExplosion(targetPos, caster);
            } 
            // 3. 타겟이 없으면 플레이어 전방에 폭발 발생
            else {
                Vector3 position = castPlayer.transform.position + castPlayer.transform.forward * defaultOffset;
                SpawnExplosion(position, caster);
            }
        }

        /// <summary>
        /// 주어진 위치에 폭발을 생성하고 caster는 피해에서 제외함.
        /// </summary>
        private void SpawnExplosion(Vector3 position, BaseEntity caster) {
            if (explosionPrefab == null) return;

            GameObject obj = Instantiate(explosionPrefab, position, Quaternion.identity);

            if (obj.TryGetComponent(out FlameOfHatredExplosionZone explosionZone)) {
                explosionZone.Activate(caster);
            }
        }

        public override void Tick() {
            if (!castPlayer.isPerformingAction) {
                castPlayer.combat.ExitCurrentMagic();
            }
        }

        public override void OnCancel() => castPlayer.combat.ExitCurrentMagic();
        public override void OnExit() { }
        public override void OnReleaseInput() { }
    }
}
