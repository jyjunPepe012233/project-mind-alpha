using System;
using System.Linq;
using MinD.Runtime.Entity;
using MinD.SO.Item;
using UnityEngine;
using UnityEngine.Events;

public class TorchItemGiver : MonoBehaviour
{
    public event Action torchActivated;
    public event Action finalTorchActivated;
    public event Action itemGived;
    
    [SerializeField] private GimmickTorch[] torches;
    [SerializeField] private Item item;
    [SerializeField] private int itemAmount = 1;

    [SerializeField] private UnityEvent onItemGived;

    private int currentActivatedTorchCount;
    public int CurrentActivatedTorchCount => currentActivatedTorchCount;
    public int AllTorchCount => torches.Length;
    
    private void Awake()
    {
        for (int i = 0; i < torches.Length; i++)
        {
            torches[i].activated += () =>
            {
                currentActivatedTorchCount++;
                if (currentActivatedTorchCount >= torches.Length)
                {
                    finalTorchActivated?.Invoke();
                    GiveItemAndDestroySelf();
                }
                else
                {
                    torchActivated?.Invoke();
                }
            };
        }
    }

    private void GiveItemAndDestroySelf()
    {
        if (item != null)
        {
            Player.player.inventory.AddItem(item.itemId, itemAmount);
        }
        onItemGived?.Invoke();
        itemGived?.Invoke();
    }
}
