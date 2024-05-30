using StarterAssets;
using UnityEngine;

public class UIActive : MonoBehaviour
{
    public static UIActive instance;

    [SerializeField] private Transform Inventory;
    [SerializeField] private Transform Hotbar;
    [SerializeField] private Transform Maketplace;
    [SerializeField] private Transform Mymoney;

    [HideInInspector] public bool isOpenMaket = false;
    [HideInInspector] public bool isOpenInven = false;
    public GameObject PausePanel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }
    void Start()
    {
        Inventory.gameObject.SetActive(false);
        Maketplace.gameObject.SetActive(false);
        Hotbar.gameObject.SetActive(true);
        Mymoney.gameObject.SetActive(true);
        PausePanel.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Advice.Instance.showAdvice == 1)
        {
            isOpenMaket = !isOpenMaket;
            isOpenInven = false;
            InventoryActive();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isOpenInven = !isOpenInven;
            isOpenMaket = false;
            InventoryActive();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PausePanel.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
        }
    }
    public void InventoryActive()
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
