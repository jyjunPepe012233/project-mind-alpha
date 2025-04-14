using System.Collections;
using MinD.Runtime.Utils;
using UnityEngine;

namespace MinD.Runtime.Object.Magics
{
    public class DarkTalonThorn : MonoBehaviour
    {
        [Header("[ Optional FX ]")]
        [SerializeField] private ParticleSystem spawnFx;
        [SerializeField] private ParticleSystem disappearFx;

        [Header("[ Damage Collider ]")]
        [SerializeField] private DamageCollider damageCollider;

        [Header("[ Life Settings ]")]
        [SerializeField] private float activeDuration = 0.3f;
        [SerializeField] private float totalLifetime = 2f;

        private Coroutine lifetimeCoroutine;

        private void OnEnable()
        {
            ApplyVisualScale();

            if (spawnFx != null)
                spawnFx.Play();

            if (lifetimeCoroutine != null)
            {
                StopCoroutine(lifetimeCoroutine);
                lifetimeCoroutine = null;
            }

            lifetimeCoroutine = StartCoroutine(LifetimeRoutine());
        }

        private IEnumerator LifetimeRoutine()
        {
            damageCollider.gameObject.SetActive(true);

            if (activeDuration > 0f)
                yield return new WaitForSeconds(activeDuration);

            damageCollider.gameObject.SetActive(false);

            if (disappearFx != null)
                disappearFx.Play();

            float remainingLifetime = totalLifetime - activeDuration;
            if (remainingLifetime > 0f)
                yield return new WaitForSeconds(remainingLifetime);

            Destroy(gameObject);
        }

        private void ApplyVisualScale()
        {
            Transform visual = transform.Find("FX_DarkTalon_Thorn");
            if (visual != null)
            {
                visual.localScale = Vector3.one * transform.localScale.x;
            }
        }

        public void Initialize(float scale)
        {
            transform.localScale = Vector3.one * scale;
            ApplyVisualScale();
        }
    }
}