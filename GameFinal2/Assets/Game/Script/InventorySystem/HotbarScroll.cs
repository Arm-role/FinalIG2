using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class HotbarScroll : MonoBehaviour
{
    public static HotbarScroll instance;
    public int min;
    public Color CurrentColor;
    private Color IntititColor;
    public int CurrentTag = 0;
    private List<Transform> Slots = new List<Transform>();

    private StarterAssetsInputs _input;

    private float _BuildingValue;
    public int _IntValue = 0;
    private bool _CheckSelect = true;
    private bool isPress1 = true;
    private bool isPress2 = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        _input = FindAnyObjectByType<StarterAssetsInputs>();

        IntititColor = transform.GetChild(0).GetComponent<Image>().color;
        foreach (Transform slot in transform)
        {
            Slots.Add(slot);
        }
        ShowHoldCurrenSlot();
    }
    void Update()
    {
        scrolling();
        ShowHoldCurrenSlot();
    }
    private void scrolling()
    {
        if (_input != null)
        {
            if (_BuildingValue + _input.ScrollHotbar != _BuildingValue)
            {
                if (!_CheckSelect)
                {
                    _CheckSelect = true;
                    _BuildingValue += _input.ScrollHotbar / 120;

                    if (_BuildingValue > Slots.Count)
                    {
                        _BuildingValue = min;
                    }
                    else if (_BuildingValue < min)
                    {
                        _BuildingValue = Slots.Count;
                    }
                    _IntValue = ((int)_BuildingValue - 1);
                    Debug.Log("changeLocation");
                }
            }
            else
            {
                _CheckSelect = false;
            }
        }
    }
    private void ShowHoldCurrenSlot()
    {
        for (int i = 0; i < Slots.Count; i++)
        {
            var ImageColor = Slots[i].GetComponent<Image>();
            if (i == _IntValue)
            {
                ImageColor.color = CurrentColor;

                if (Slots[i].childCount > 0)
                {
                    ItemDrop drop = Slots[i].GetChild(0).GetComponent<ItemDrop>();
                    LinkInventory(drop);
                }
                else if (Slots[i].childCount == 0)
                {
                    CurrentTag = 0;
                    BuildingManager.Instance.CancelObject();

                    if (_input.Action)
                    {
                        if (isPress2)
                        {
                            isPress2 = false;
                            UsePlant.Instance.isPress = true;
                        }
                    }
                    else
                    {
                        isPress2 = true;
                    }
                }
            }
            else
            {
                ImageColor.color = IntititColor;
            }
        }
    }
    private void LinkInventory(ItemDrop drop)
    {
        switch (drop.Item.itemType)
        {
            case ItemType.Misc:
                CurrentTag = 2;
                BuildingManager.Instance.CancelObject();
                if (_input.Action)
                {
                    if (isPress1)
                    {
                        isPress1 = false;
                        drop.UseItem(1);
                        UsePlant.Instance._placeSeed(drop.Item);
                    }
                }
                else
                {
                    isPress1 = true;
                    UsePlant.Instance.isPress = true;
                }

                break;

            case ItemType.Weapon:
                CurrentTag = 1;
                break;

            case ItemType.Building:
                CurrentTag = 2;
                BuildingManager.Instance.SelecObject(drop.Item);

                if (_input.Action)
                {
                    if (isPress1)
                    {
                        isPress1 = false;
                        drop.UseItem(1);
                        BuildingManager.Instance.PlaceObject();
                    }
                }
                else
                {
                    isPress1 = true;
                }
                break;

            case ItemType.Gun:
                CurrentTag = 3;
                break;
        }
    }
}
