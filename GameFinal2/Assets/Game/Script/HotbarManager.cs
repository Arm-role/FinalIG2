using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class HotbarManager : MonoBehaviour
{
    public int min;
    public TextMeshProUGUI CurrentSlot;
    public TextMeshProUGUI CurrentMode;

    private string[] Slots = new string[9];
    public string[] TagOnHotbar = new string[3];

    public int CurrentTag;

    private StarterAssetsInputs _input;

    private float _BuildingValue;
    private int _IntValue;
    private bool _CheckSelect = true;   

    public void ImportStarterAssetsInputs(StarterAssetsInputs input)
    {
        _input = input;
    }
    void Start()
    {
        Debug.Log(Slots.Length);
        Slots[1] = "Weapon";
        Slots[5] = "Constuction";
        Slots[7] = "Gun";
    }

    // Update is called once per frame
    void Update()
    {
        if( _input != null )
        {
            if (_BuildingValue + _input.ScrollHotbar != _BuildingValue)
            {
                if (!_CheckSelect)
                {
                    _CheckSelect = true;
                    _BuildingValue += _input.ScrollHotbar / 120;

                    if ( _BuildingValue > Slots.Length)
                    {
                        _BuildingValue = min;
                    }else if ( _BuildingValue < min )
                    {
                        _BuildingValue = Slots.Length;
                    }

                    _IntValue = (int)_BuildingValue - 1;
                    CurrentSlot.text = _IntValue.ToString();
                    CurrentTag = CheckSlots(_IntValue);
                }
            }
            else
            {
                _CheckSelect = false;
            }
            CurrentMode.text = "Mode : " + CurrentTag;
        }
    }

    private int CheckSlots(int CurrentSlot)
    {
        if (Slots[CurrentSlot] != null)
        {
            for (int i = 0; i < TagOnHotbar.Length; i++)
            {
                if (Slots[CurrentSlot] == TagOnHotbar[i])
                {
                    //Debug.Log($"Slot:{CurrentSlot} Tag:{TagOnHotbar[i]} Number:{i}");
                    return i;
                }
            }
        }
        return 0;
    }
}
