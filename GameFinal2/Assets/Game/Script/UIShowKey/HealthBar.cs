using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Slider healthSlider;
    void Start()
    {
        healthSlider.maxValue = health.health;

    }

    void Update()
    {

        healthSlider.value = health.health;
    }
}
