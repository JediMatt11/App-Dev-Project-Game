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
        if (reachedPoint)
        {
            Vector3 randomDirection = Random.insideUnitSphere * Random.Range(-490f, 490f);
            randomDirection.y = 0;
            enemyWizard.navMeshAgent.SetDestination(randomDirection + enemyWizard.transform.position);
            reachedPoint = false;
        }
        else if (enemyWizard.navMeshAgent.remainingDistance <= enemyWizard.navMeshAgent.stoppingDistance && !enemyWizard.navMeshAgent.pathPending)
        {
            reachedPoint = true;
        }
    }
}
