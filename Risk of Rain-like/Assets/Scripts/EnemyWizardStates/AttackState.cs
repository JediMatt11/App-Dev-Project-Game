using UnityEngine;

public class AttackState : EnemyState
{
    public AttackState(EnemyWizard enemyWizard, EnemyStateMachine esm) : base(enemyWizard, esm)
    {

    }

    

    public override void Enter()
    {
        base.Enter();
        enemyWizard.animator.SetBool("Attack", true);
        enemyWizard.navMeshAgent.SetDestination(enemyWizard.player.position);
    }

    public override void Exit()
    {
        base.Exit();
        enemyWizard.animator.SetBool("Attack", false);
    }

    public override void Update()
    {
        base.Update();
        if (enemyWizard.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f)
        {
            enemyWizard.navMeshAgent.SetDestination(enemyWizard.transform.position);
            if (Vector3.Distance(enemyWizard.transform.position, enemyWizard.player.position) <= enemyWizard.hitBox)
            {
                //TAG!!
            }
        }



        if (enemyWizard.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            if (enemyWizard.inView)
            {
                esm.ChangeState(enemyWizard.chaseState);
            }
            else
            {
                esm.ChangeState(enemyWizard.patrolState);
            }
        }
    }
}
