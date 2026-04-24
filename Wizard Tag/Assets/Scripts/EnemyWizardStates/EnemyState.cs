using UnityEngine;

public class EnemyState
{
    public EnemyWizard enemyWizard;
    public EnemyStateMachine esm;
    

    public EnemyState(EnemyWizard enemyWizard, EnemyStateMachine esm)
    {
        this.enemyWizard = enemyWizard;
        this.esm = esm;
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }

    public virtual void Update()
    {

    }

    

}
