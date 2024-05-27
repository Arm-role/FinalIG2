using StarterAssets;
using UnityEngine;

public class UIActive : MonoBehaviour
{
    [SerializeField] private Transform Inventory;
    [SerializeField] private Transform Hotbar;
    [SerializeField] private Transform Maketplace;
    [SerializeField] private Transform Mymoney;

    private bool isOpenMaket = false;
    private bool isOpenInven = false;

    private StarterAssetsInputs _input;

    void Start()
    {
        _input = FindAnyObjectByType<StarterAssetsInputs>();
        Inventory.gameObject.SetActive(false);
        Maketplace.gameObject.SetActive(false);
        Hotbar.gameObject.SetActive(true);
        Mymoney.gameObject.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isOpenMaket = !isOpenMaket;
            isOpenInven = false;
            InventoryActive();
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            isOpenInven = !isOpenInven;
            isOpenMaket = false;
            InventoryActive();
        }
    }
    private void InventoryActive()
    {
        if (isOpenMaket && !isOpenInven)
        {
            UIactive(UIOpensystem.Maketplace);
            Cursor.lockState = CursorLockMode.None;
        }
        else if (!isOpenMaket && isOpenInven)
        {
            UIactive(UIOpensystem.Inventory);
            Cursor.lockState = CursorLockMode.None;
        }
        else if (!isOpenMaket && !isOpenInven)
        {
            UIactive(UIOpensystem.Hotbar);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    private void UIactive(UIOpensystem uIOpensystem)
    {
        switch (uIOpensystem)
        {
            case UIOpensystem.Maketplace:

                Inventory.gameObject.SetActive(false);
                Hotbar.gameObject.SetActive(false);
                Mymoney.gameObject.SetActive(false);
                Maketplace.gameObject.SetActive(true);

                MaketplaceManager.Instance.ListItem();
                uIOpensystem = UIOpensystem.nullMode;
                break;

            case UIOpensystem.Inventory:

                Inventory.gameObject.SetActive(true);
                Hotbar.gameObject.SetActive(false);
                Mymoney.gameObject.SetActive(false);
                Maketplace.gameObject.SetActive(false);

                InventoryManager.Instance.ListItem();
                uIOpensystem = UIOpensystem.nullMode;
                break;
            
            case UIOpensystem.Hotbar:
                Inventory.gameObject.SetActive(false);
                Hotbar.gameObject.SetActive(true);
                Mymoney.gameObject.SetActive(true);
                Maketplace.gameObject.SetActive(false);

                MyMoneySystem.instance.ShowOnInventory();
                uIOpensystem = UIOpensystem.nullMode;
                break;
            
        }
    }
}
