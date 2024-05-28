using UnityEngine;

public class AdviceShowUI : MonoBehaviour
{
    public static AdviceShowUI instance;
    public bool isActive;
    public GameObject adviceOB;
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
        adviceOB.SetActive(false);
    }
    public void IsActiveOnScene(bool isActive)
    {
        //Debug.Log(isActive);
        adviceOB.SetActive(isActive);
    }
}
