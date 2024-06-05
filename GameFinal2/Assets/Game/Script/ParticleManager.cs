using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager instance;

    [Header("Gun")]
    public ParticleSystem Flash;
    public ParticleSystem MuzzleFlash;

    [Header("Action")]
    public ParticleSystem Hit;
    public ParticleSystem HitBlock;
    public ParticleSystem BockDestroy;
    public ParticleSystem Death;

    [Header("Player Action")]
    public ParticleSystem Dust;

    [Header("Plant")]
    public ParticleSystem PlantBoot;
    public ParticleSystem PlantGrow;

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
}
