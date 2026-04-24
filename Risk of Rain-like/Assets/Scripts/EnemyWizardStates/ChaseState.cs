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
            if (!enemyWizard.PlayerInView())
            {
                esm.ChangeState(enemyWizard.patrolState);
                Debug.Log("Patrol");
                return;
            }
            enemyWizard.navMeshAgent.SetDestination(enemyWizard.player.position);
            if (Vector3.Distance(enemyWizard.transform.position, enemyWizard.player.position) <= enemyWizard.attackRange)
            {
                    esm.ChangeState(enemyWizard.attackState);
                }
        }
    }
}
