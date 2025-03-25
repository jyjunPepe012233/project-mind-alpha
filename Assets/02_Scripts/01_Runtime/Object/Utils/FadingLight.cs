using System.Collections;
using UnityEngine;


namespace MinD.Runtime.Object.Utils {

[RequireComponent(typeof(Light))]
public class FadingLight : MonoBehaviour {

	public float minIntensityValue = 0;
	public float maxIntensityValue = 1;
	
	[HideInInspector] public Light light;
	private Coroutine currentFade;


	
	public void OnEnable() {
		if (light == null) {
			light = GetComponent<Light>();
		}
		
		light.intensity = minIntensityValue;
		if (minIntensityValue == 0) {
			light.enabled = false;
		}
	}

	public void FadeIn(float duration) {
		if (light == null) {
			light = GetComponent<Light>();
		}
		
		if (currentFade != null) {
			StopCoroutine(currentFade);
		}
		
		StartCoroutine(FadeInCoroutine(Mathf.Max(duration, 0.01f)));
	}

	public void FadeOut(float duration, bool destroyWithEnd = false) {
		if (light == null) {
			light = GetComponent<Light>();
		}
		
		if (currentFade != null) {
			StopCoroutine(currentFade);
		}
		
		StartCoroutine(FadeOutCoroutine(Mathf.Max(duration, 0.01f), destroyWithEnd));
	}
	
	private IEnumerator FadeInCoroutine(float duration) {

		light.intensity = minIntensityValue;
		light.enabled = true;
		
		while (true) {
			
			light.intensity += Time.deltaTime / duration * (maxIntensityValue - minIntensityValue); // BLAST LIGHT IS GOING BRIGHTNESS IN DURATION

			if (light.intensity >= maxIntensityValue) {
				light.intensity = maxIntensityValue; // CLAMP
				break;
			}
			
			yield return null;
		}
	}
	private IEnumerator FadeOutCoroutine(float duration, bool destroyWithEnd) {
		
		light.intensity = maxIntensityValue;
		light.enabled = true;
		
		while (true) {

			light.intensity -= Time.deltaTime / duration * (maxIntensityValue - minIntensityValue); // BLAST LIGHT IS GOING BRIGHT DURING 0.5 SECOND
			yield return null;

			if (light.intensity <= minIntensityValue) {
				break;
			}
		}
		
		// TODO: CHECK THIS OBJECT IS POOLED,
		// TODO: IF THIS IS POOLED OBJECT, DESTROY WITH OBJECT POOLING
		
		if (destroyWithEnd) {
			Destroy(gameObject);
		} else {
			light.intensity = minIntensityValue;
			if (minIntensityValue == 0) {
				light.enabled = false;
			}
		}
		
	}
}

}