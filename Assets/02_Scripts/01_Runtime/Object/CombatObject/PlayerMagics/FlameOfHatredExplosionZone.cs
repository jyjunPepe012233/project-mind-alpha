using System.Collections;
using MinD.Runtime.Entity;
using MinD.Runtime.Utils;
using UnityEngine;

namespace MinD.Runtime.Object.Magics {
    /// <summary>
    /// '증오의 불꽃 - 폭' 마법의 폭발 영역.
    /// 생성 시 즉시 데미지를 주고, 일정 시간 후 자동 제거됨.
    /// </summary>
    public class FlameOfHatredExplosionZone : MonoBehaviour {
        [Header("[ VFX / Damage ]")]
        [SerializeField] private ParticleSystem explosionEffect;   // 폭발 이펙트
        [SerializeField] private DamageCollider damageCollider;    // 피해 영역
        [SerializeField] private float activeDuration = 1.5f;      // 유지 시간

        /// <summary>
        /// 폭발 이펙트 및 피해를 시작하고, 일정 시간 후 자동 종료됨.
        /// </summary>
        /// <param name="caster">피해에서 제외할 시전자</param>
        public void Activate(BaseEntity caster) {
            explosionEffect?.Play();

            if (damageCollider != null) {
                if (caster != null) {
                    damageCollider.blackList.Add(caster);
                }

                damageCollider.gameObject.SetActive(true);
            }

            StartCoroutine(DeactivateAfterDelay());
        }

        /// <summary>
        /// 일정 시간이 지난 뒤 데미지를 비활성화하고 객체 제거.
        /// </summary>
        private IEnumerator DeactivateAfterDelay() {
            yield return new WaitForSeconds(activeDuration);

            if (damageCollider != null) {
                damageCollider.ResetToHitAgain();
                damageCollider.gameObject.SetActive(false);
            }

            Destroy(gameObject);
        }
    }
}