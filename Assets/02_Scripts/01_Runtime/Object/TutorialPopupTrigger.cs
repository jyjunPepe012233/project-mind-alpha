using MinD.Runtime.Managers;
using UnityEngine;

public class TutorialPopupTrigger : MonoBehaviour
{
    [Header("Tutorial Data")]
    [SerializeField] private Sprite tutorialImage;
    [SerializeField] private string tutorialTitle;
    [TextArea]
    [SerializeField] private string tutorialContent;
    
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        hasTriggered = true;

        PlayerHUDManager.Instance.ShowTutorialPopup(tutorialImage, tutorialTitle, tutorialContent);
    }
}