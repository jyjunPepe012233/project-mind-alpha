using MinD.SO.Item;
using UnityEngine;

namespace MinD.Runtime.Object.Magics {
    /// <summary>
    /// '증오의 불꽃 - 열' 마법을 처리하는 클래스.
    /// 타겟이 존재하면 타겟 위치에, 없으면 플레이어 앞 일정 거리 지점에 장판을 생성함.
    /// </summary>
    public class FlameOfHatred : Magic {
        [Header("[ Flame Settings ]")]
        [SerializeField] private GameObject flameZonePrefab;  // 생성할 장판 프리팹
        [SerializeField] private float distance = 10f;         // 플레이어 기준 앞 거리
        [SerializeField] private float zoneDuration = 10f;     // 장판 지속 시간
        [SerializeField] private float tickInterval = 1f;      // 피해 간격

        public override void OnUse() {
            castPlayer.animation.PlayTargetAction("FlameOfHatred", true, true, false, false);
        }

        public override void OnSuccessfullyCast() {
            Vector3 position;

            // 1. 타겟이 존재하면 타겟 위치에 장판 생성
            if (castPlayer.combat.target != null) {
                position = castPlayer.combat.target.transform.position;
            }
            // 2. 타겟이 없으면 플레이어 기준 앞쪽에 생성
            else {
                position = castPlayer.transform.position + castPlayer.transform.forward * distance;
            }

            SpawnZone(position);
        }

        /// <summary>
        /// 주어진 위치에 장판을 생성하고 효과를 활성화함.
        /// </summary>
        /// <param name="position">장판 생성 위치</param>
        private void SpawnZone(Vector3 position) {
            if (flameZonePrefab == null) return;

            GameObject zoneObj = Instantiate(flameZonePrefab, position, Quaternion.identity);
            if (zoneObj.TryGetComponent(out FlameOfHatredZone zone)) {
                zone.Activate(zoneDuration, tickInterval);
                zone.IgnorePlayerDamage(castPlayer);
            }
        }

        public override void Tick() {
            if (!castPlayer.isPerformingAction) {
                castPlayer.combat.ExitCurrentMagic();
            }
        }

        public override void OnReleaseInput() { }

        public override void OnCancel() => castPlayer.combat.ExitCurrentMagic();

        public override void OnExit() { }
    }
}
