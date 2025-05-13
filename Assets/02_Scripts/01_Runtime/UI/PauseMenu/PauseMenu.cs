using UnityEngine;
using System.Collections;
using MinD.Runtime.UI;

public class PauseMenu : PlayerMenu
{
    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 0.5f;

    private Coroutine fadeCoroutine;

    public override void Open()
    {
        Cursor.visible = true;
    }

    public override void Close()
    {
        Cursor.visible = false;
    }
}