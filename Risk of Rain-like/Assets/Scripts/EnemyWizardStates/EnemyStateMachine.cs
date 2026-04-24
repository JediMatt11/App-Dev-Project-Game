using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState EnemyState { get; set; }

    public void Initalize(EnemyState startingState)
    {
        EnemyState = startingState;
        EnemyState.Enter();
    }

    public void ChangeState(EnemyState newState)
    {
            EnemyState.Exit();
            EnemyState = newState;
            EnemyState.Enter();
    }
}