using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ChaseState : EnemyState
{
    public ChaseState(EnemyWizard enemyWizard, EnemyStateMachine esm) : base(enemyWizard, esm)
    {

    }

    public override void Enter()
    {
        base.Enter();
        enemyWizard.animator.SetBool("Chase", true);
    }

    public override void Exit()
    {
        base.Exit();
        enemyWizard.animator.SetBool("Chase", false);
    }

    public override void Update()
    {
        base.Update();
        if (enemyWizard.player != null)
        {
            enemyWizard.navMeshAgent.SetDestination(enemyWizard.player.position);
        }
    }
}
