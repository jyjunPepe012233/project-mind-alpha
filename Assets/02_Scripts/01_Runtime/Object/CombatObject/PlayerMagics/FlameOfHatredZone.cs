using System.Collections;
using System.Collections.Generic;
using MinD.Runtime.Entity;
using MinD.Runtime.Utils;
using UnityEngine;

namespace MinD.Runtime.Object.Magics {
    /// <summary>
    /// '증오의 불꽃 - 열' 마법의 장판 객체. 주기적으로 데미지를 주는 영역을 관리함.
    /// </summary>
    public class FlameOfHatredZone : MonoBehaviour {
        [Header("[ VFX / Damage ]")]
        [SerializeField] private ParticleSystem flameEffect;
        [SerializeField] private DamageCollider damageCollider;

        // 현재 씬에서 활성화된 모든 장판 목록 (게임플레이 제어에 유용)
        public static readonly List<FlameOfHatredZone> ActiveZones = new();

        /// <summary>
        /// 장판을 활성화하고 주어진 시간 동안 주기적으로 데미지를 줌.
        /// </summary>
        /// <param name="duration">총 지속 시간</param>
        /// <param name="tickInterval">틱 간격</param>
        public void Activate(float duration, float tickInterval) {
            flameEffect?.Play();
            damageCollider.gameObject.SetActive(true);
            StartCoroutine(DamageOverTime(duration, tickInterval));
        }

        private IEnumerator DamageOverTime(float duration, float tickInterval) {
            float elapsed = 0f;

            while (elapsed < duration) {
                // 데미지 콜라이더를 껐다가 켜서 충돌 재활성
                damageCollider.gameObject.SetActive(false);
                damageCollider.ResetToHitAgain();
                damageCollider.gameObject.SetActive(true);

                yield return new WaitForSeconds(tickInterval);
                elapsed += tickInterval;
            }

            // 장판 종료 처리
            damageCollider.ResetToHitAgain();
            damageCollider.gameObject.SetActive(false);
            Destroy(gameObject);
        }
        
        /// <summary>
        /// 캐스트 플레이어는 피해를 받지 않도록 설정
        /// </summary>
        /// <param name="player">캐스트 플레이어</param>
        public void IgnorePlayerDamage(Player player) {
            if (damageCollider != null && player != null) {
                // 플레이어는 데미지를 받지 않도록 BlackList에 추가
                damageCollider.blackList.Add(player);
            }
        }

        private void OnEnable() {
            if (!ActiveZones.Contains(this)) {
                ActiveZones.Add(this);
            }
        }

        private void OnDisable() {
            ActiveZones.Remove(this);
        }

        private void OnDestroy() {
            ActiveZones.Remove(this);
        }
    }
}