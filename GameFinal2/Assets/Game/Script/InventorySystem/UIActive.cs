using StarterAssets;
using UnityEngine;

public class UIActive : MonoBehaviour
{
    public static UIActive instance;

    [SerializeField] private Transform Inventory;
    [SerializeField] private Transform Scene;
    [SerializeField] private Transform Maketplace;
    [SerializeField] private Transform Mymoney;
    [SerializeField] private Transform PausePanel;

    public Transform GameOver;

    [HideInInspector] public bool isOpenMaket = false;
    [HideInInspector] public bool isOpenInven = false;
    [HideInInspector] public bool isOpenPause = false;

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
        Scene.gameObject.SetActive(true);
        Mymoney.gameObject.SetActive(true);
        PausePanel.gameObject.SetActive(false);
        GameOver.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Advice.Instance.showAdvice == 1)
        {
            isOpenMaket = !isOpenMaket;
            isOpenInven = false;
            isOpenPause = false;
            InventoryActive();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isOpenInven = !isOpenInven;
            isOpenMaket = false;
            isOpenPause = false;
            InventoryActive();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            isOpenPause = !isOpenPause;
            isOpenInven = false;
            isOpenMaket = false;
            InventoryActive();
        }
        if (GameOver.gameObject.activeSelf)
        {
            isOpenMaket = false;
            isOpenPause = false;
            isOpenInven = false;

            PausePanel.gameObject.SetActive(false);
            Inventory.gameObject.SetActive(false);
            Maketplace.gameObject.SetActive(false);
            Scene.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
        }
    }
    private void LateUpdate()
    {
        pauseTime();
    }
    public void InventoryActive()
    {
        if (!isOpenMaket && !isOpenInven && isOpenPause)
        {
            UIactive(UIOpensystem.PauseMenu);
            Cursor.lockState = CursorLockMode.None;
        }
        else if (isOpenMaket && !isOpenInven && !isOpenPause)
        {
            UIactive(UIOpensystem.Maketplace);
            Cursor.lockState = CursorLockMode.None;
        }
        else if (!isOpenMaket && isOpenInven && !isOpenPause)
        {
            UIactive(UIOpensystem.Inventory);
            Cursor.lockState = CursorLockMode.None;
        }
        else if (!isOpenMaket && !isOpenInven && !isOpenPause)
        {
            UIactive(UIOpensystem.Hotbar);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    private void UIactive(UIOpensystem uIOpensystem)
    {
        switch (uIOpensystem)
        {
            case UIOpensystem.PauseMenu:

                Inventory.gameObject.SetActive(false);
                Scene.gameObject.SetActive(false);
                Mymoney.gameObject.SetActive(false);
                Maketplace.gameObject.SetActive(false);
                PausePanel.gameObject.SetActive(true);

                MaketplaceManager.Instance.ListItem();
                uIOpensystem = UIOpensystem.nullMode;
                break;

            case UIOpensystem.Maketplace:

                Inventory.gameObject.SetActive(false);
                Scene.gameObject.SetActive(false);
                Mymoney.gameObject.SetActive(false);
                Maketplace.gameObject.SetActive(true);
                PausePanel.gameObject.SetActive(false);

                MaketplaceManager.Instance.ListItem();
                uIOpensystem = UIOpensystem.nullMode;
                break;

            case UIOpensystem.Inventory:

                Inventory.gameObject.SetActive(true);
                Scene.gameObject.SetActive(false);
                Mymoney.gameObject.SetActive(false);
                Maketplace.gameObject.SetActive(false);
                PausePanel.gameObject.SetActive(false);

                InventoryManager.Instance.ListItem();
                uIOpensystem = UIOpensystem.nullMode;
                break;
            
            case UIOpensystem.Hotbar:
                Inventory.gameObject.SetActive(false);
                Scene.gameObject.SetActive(true);
                Mymoney.gameObject.SetActive(true);
                Maketplace.gameObject.SetActive(false);
                PausePanel.gameObject.SetActive(false);

                MyMoneySystem.instance.ShowOnInventory();
                uIOpensystem = UIOpensystem.nullMode;
                break;
            
        }
    }
    private void pauseTime()
    {
        Time.timeScale = (PausePanel.gameObject.activeSelf == true) ? 0 : 1;
    }
}
