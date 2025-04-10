using MinD.SO.Item;
using UnityEngine;

public class InventorySlotPresentor : MonoBehaviour
{
    private readonly InventorySlotView _view;
    private readonly InventorySlotModel _model;

    public InventorySlotPresentor(InventorySlotView view, InventorySlotModel model)
    {
        _view = view;
        _model = model;
        
        UpdateView();
    }
    
    public void UpdateView()
    {
        if (_model.Item == null)
        {
            _view.Clear();
        }
        else
        {
            _view.Set(_model.Item, _model.Count, _model.IsEquipped);
        }
    }

    public void SetItem(Item item, int count, bool isEquipped)
    {
        _model.SetItem(item, count, isEquipped);
        UpdateView();
    }

    public void SetSelected(bool isSelected)
    {
        _view.SetSelected(isSelected);
    }

    public void Clear()
    {
        _model.Clear();
        _view.Clear();
    }
}
