using System.Collections;
using TMPro;
using UnityEngine;

public class PopupUIController : MonoBehaviour
{
    [SerializeField] public CanvasGroup popupCanvasGroup;
    [SerializeField] public TextMeshProUGUI popupText;

    private void Awake()
    {
        popupCanvasGroup.alpha = 0f;
        popupCanvasGroup.gameObject.SetActive(false);
    }
}