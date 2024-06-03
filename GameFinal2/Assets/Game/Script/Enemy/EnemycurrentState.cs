using UnityEngine;

public class EnemyCurrentState : MonoBehaviour
{
    [SerializeField]
    private EnemyState _state;

    public EnemyState state
    {
        get
        {
            return _state;
        }
        private set
        {
            if (value != _state)
            {
                OnStateChange?.Invoke(_state, value);
            }
            _state = value;
        }
    }
    public delegate void StateChangeEvent(EnemyState Oldstate, EnemyState newstate);
    public StateChangeEvent OnStateChange;

    public void ChangeState(EnemyState newState)
    {
        Debug.Log(newState + " : " + state);
        if (newState != state)
        {
            state = newState;
        }
    }
}
