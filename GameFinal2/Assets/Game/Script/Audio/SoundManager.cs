using UnityEngine;
public enum SoundType
{
    Sword,
    Gun,
    Player,
    Plant,
    Misc
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

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

    public void PlayerSound(SoundType sound, float value)
    {

    }
}
