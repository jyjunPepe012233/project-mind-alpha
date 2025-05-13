using System.Collections;
using MinD.Runtime.Utils;
using UnityEngine;

namespace MinD.Runtime.Object.Magics {
    /// <summary>
    /// DarkTalon 마법에서 생성되는 가시 오브젝트.
    /// 일정 시간 동안 데미지를 주고, 이후 파괴됨.
    /// </summary>
    public class DarkTalonThorn : MonoBehaviour {
        [Header("[ FX ]")]
        [SerializeField] private ParticleSystem thornFx;              // 가시 이펙트

        [Header("[ Damage Collider ]")]
        [SerializeField] private DamageCollider damageCollider;       // 데미지 판정용 콜라이더

        [Header("[ Life Settings ]")]
        [SerializeField] private float activeDuration = 0.5f;         // 콜라이더 활성 시간
        [SerializeField] private float totalLifetime = 5f;            // 총 수명

        private Coroutine lifetimeCoroutine;

        private void OnEnable() {
            ApplyVisualScale();

            thornFx?.Play();

            if (lifetimeCoroutine != null) {
                StopCoroutine(lifetimeCoroutine);
            }

            lifetimeCoroutine = StartCoroutine(LifetimeRoutine());
        }

        /// <summary>
        /// 데미지 활성 → 일정 시간 후 파괴
        /// </summary>
        private IEnumerator LifetimeRoutine() {
            damageCollider?.gameObject.SetActive(true);

            if (activeDuration > 0f)
                yield return new WaitForSeconds(activeDuration);

            damageCollider?.gameObject.SetActive(false);

            float remainingLifetime = totalLifetime - activeDuration;
            if (remainingLifetime > 0f)
                yield return new WaitForSeconds(remainingLifetime);

            Destroy(gameObject);
        }

        /// <summary>
        /// 비주얼 및 이펙트의 크기를 설정된 scale로 조정
        /// </summary>
        private void ApplyVisualScale() {
            float scale = transform.localScale.x;

            Transform visual = transform.Find("FX_DarkTalon_Thorn");
            if (visual != null)
                visual.localScale = Vector3.one * scale;

            if (thornFx != null)
                thornFx.transform.localScale = Vector3.one * scale;
        }

        /// <summary>
        /// 외부에서 크기 조정 시 사용
        /// </summary>
        public void Initialize(float scale) {
            transform.localScale = Vector3.one * scale;
            ApplyVisualScale();
        }
    }
}
