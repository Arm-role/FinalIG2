using TMPro;
using UnityEngine;

public class MyMoneySystem : MonoBehaviour
{
    public static MyMoneySystem instance;
    public TextMeshProUGUI TextCoin;
    public int Money;

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
    private void Start()
    {
        TextCoin.text = Money.ToString();
    }
    public void ShowOnInventory(TextMeshProUGUI textMesh = null)
    {
        if (textMesh == null)
        {
            TextCoin.text = Money.ToString();
        }else
        {
            textMesh.text = Money.ToString();
        }
    }
    public void resetMyCoin(int CoinSell)
    {
        Money -= CoinSell;
        ShowOnInventory(TextCoin);
    }
}
