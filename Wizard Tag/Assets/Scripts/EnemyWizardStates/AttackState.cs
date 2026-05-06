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
        if (enemyWizard.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
        {
            if (Vector3.Distance(enemyWizard.transform.position, enemyWizard.player.position) <= enemyWizard.hitBox)
            {
                //TAG!!
                enemyWizard.levelManager.Lose();
            }
        }
    }
}
