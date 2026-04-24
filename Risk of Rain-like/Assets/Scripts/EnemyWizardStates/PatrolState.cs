using UnityEngine;

public class PatrolState : EnemyState
{
    public bool reachedPoint = true;
    public PatrolState(EnemyWizard enemyWizard, EnemyStateMachine esm) : base(enemyWizard, esm)
    {

    }

    public override void Enter()
    {
        base.Enter();
        enemyWizard.animator.SetBool("Walk", true);
    }

    public override void Exit()
    {
        base.Exit();
        enemyWizard.animator.SetBool("Walk", false);
    }

    public override void Update()
    {
        base.Update();
        if (enemyWizard.inView)
        {
            esm.ChangeState(enemyWizard.chaseState);
        }
        if (reachedPoint)
        {
            float x = Random.Range(-90f, 90f);
            float z = Random.Range(-90f, 90f);
            Vector3 randomDirection = new Vector3(x, 0f, z);
            randomDirection.y = enemyWizard.transform.position.y;
            enemyWizard.navMeshAgent.SetDestination(randomDirection + enemyWizard.transform.position);
            reachedPoint = false;
        }
        else if (enemyWizard.navMeshAgent.remainingDistance <= enemyWizard.navMeshAgent.stoppingDistance && !enemyWizard.navMeshAgent.pathPending)
        {
            reachedPoint = true;
        }
    }
}
