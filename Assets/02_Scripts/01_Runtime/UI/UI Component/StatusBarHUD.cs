using UnityEngine;
using UnityEngine.UI;

namespace MinD.Runtime.UI {

public class StatusBarHUD : MonoBehaviour {

    [Header("[ UI Elements ]")]
    [SerializeField] private Slider fillSlider;
    [SerializeField] private Slider fillTrailSlider;
    
    [Space(10)]
    [SerializeField] private RectTransform fillTransform;
    [SerializeField] private RectTransform fillTrailTransform;
    [SerializeField] private RectTransform backgroundTransform;
    [SerializeField] private RectTransform barFloorTransform;

    [Header("[ Settings ]")]
    [SerializeField] private float widthMultiplier = 1;

    private float trailDampingSpeed = 100;

    private int currentValue;
    private int maxValue;




    public void SetMaxValue(float maxValue) {

        fillSlider.maxValue = maxValue;
        fillTrailSlider.maxValue = maxValue;

        maxValue *= widthMultiplier;
        fillTransform.sizeDelta = new Vector2(maxValue, fillTransform.sizeDelta.y);
        fillTrailTransform.sizeDelta = new Vector2(maxValue, fillTrailTransform.sizeDelta.y);
        backgroundTransform.sizeDelta = new Vector2(maxValue, backgroundTransform.sizeDelta.y);
        barFloorTransform.sizeDelta = new Vector2(maxValue, barFloorTransform.sizeDelta.y);
    }

    public void SetValue(int value) {

        fillSlider.value = value;

    }

    public void HandleTrailFollowing() {

        // IF NEED DRAIN THE TRAIL
        if (fillTrailSlider.value > fillSlider.value) {
            fillTrailSlider.value -= trailDampingSpeed * Time.deltaTime;
            
            // CLAMP MINIMUM
            fillTrailSlider.value = Mathf.Max(fillTrailSlider.value, fillSlider.value);
        }

        // IF NEED FILL THE TRAIL
        if (fillTrailSlider.value < fillSlider.value) {
            fillTrailSlider.value += trailDampingSpeed * Time.deltaTime;
            
            // CLAMP MAXIMUM
            fillTrailSlider.value = Mathf.Min(fillTrailSlider.value, fillSlider.value);
        }

    }


}

}