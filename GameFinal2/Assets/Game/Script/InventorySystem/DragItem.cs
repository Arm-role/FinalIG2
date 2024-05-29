using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using StarterAssets;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Image image;
    [HideInInspector] public Transform parentAfterDrag;

    private ItemDrop ItemDrop;
    private Transform parentBeforeDrag;

    private bool _UseShortCut = false;
    private bool _OnHot;
    private void Start()
    {
        ItemDrop = GetComponent<ItemDrop>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if(transform.parent.transform.parent.name == "InventoryFrame")
            {
                _UseShortCut = true;
                _OnHot = true;
                UseShortcut("Hotbar");
            }else if (transform.parent.transform.parent.name == "Hotbar")
            {
                _UseShortCut = true;
                _OnHot = false;
                UseShortcut("InventoryFrame");
            }
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        parentBeforeDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
        
        for (int i = 0; i < InventoryManager.Instance.items.Count; i++)
        {
            if (InventoryManager.Instance.items[i].Item3 == parentBeforeDrag)
            {
                if(parentAfterDrag.parent.name == "Hotbar")
                {
                    InventoryManager.Instance.SetItem(i, parentAfterDrag, true);
                    ItemDrop._isHotbar = true;

                }else
                {
                    InventoryManager.Instance.SetItem(i, parentAfterDrag, false);
                    ItemDrop._isHotbar = false;
                }
            }
        }
    }
    private void UseShortcut(string parentSlot)
    {
        if (_UseShortCut)
        {
            bool _IschangeParent = false;
            foreach (Transform slot in InventoryManager.Instance.Slots)
            {
                var items = InventoryManager.Instance.items;
                for (int i = 0; i < items.Count; i++)
                {
                    if (slot.parent.name == parentSlot)
                    {
                        if (slot.childCount == 0)
                        {
                            if (items[i].Item3 == transform.parent)
                            {
                                if (!_IschangeParent)
                                {
                                    transform.SetParent(slot);
                                    InventoryManager.Instance.SetItem(i, slot, _OnHot);
                                    _IschangeParent = true;
                                    _UseShortCut = false;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
