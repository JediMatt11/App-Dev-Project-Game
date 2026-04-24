using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWizard : MonoBehaviour
{
    public static Material enemyMat;
    public static float hueChangeSpeed = 0.75f;
    public Transform player;
    public bool inView;
    public float attackRange;
    public float hitBox;
    public EnemyStateMachine enemyStateMachine { get; set; }
    public AttackState attackState { get; set; }
    public ChaseState chaseState { get; set; }
    public PatrolState patrolState { get; set; }

    public NavMeshAgent navMeshAgent;

    public Animator animator;
    public AnimationStateSO animationStateSO;
    public AnimatorOverrideController animatorOverrideController;
    public static Dictionary<AnimationStateSO, AnimatorOverrideController> cache = new Dictionary<AnimationStateSO, AnimatorOverrideController>();
    private void Awake()
    {
        enemyMat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        SetAnimationOverrides();
        enemyStateMachine = new EnemyStateMachine();
        attackState = new AttackState(this, enemyStateMachine);
        chaseState = new ChaseState(this, enemyStateMachine);
        patrolState = new PatrolState(this, enemyStateMachine);
    }

    private void Start()
    {
        enemyStateMachine.Initalize(patrolState);
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        enemyStateMachine.EnemyState.Update();
        ChangeMatHue();
    }

    

    public void SetAnimationOverrides()
    {
        if (!cache.TryGetValue(animationStateSO, out var overrideController))
        {
            RuntimeAnimatorController baseController = animator.runtimeAnimatorController;
            overrideController = new AnimatorOverrideController(baseController);

            List<KeyValuePair<AnimationClip, AnimationClip>> overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
            overrideController.GetOverrides(overrides);

            for (int i = 0; i < overrides.Count; i++)
            {
                var original = overrides[i].Key;

                if (original.name == "Walk") overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(original, animationStateSO.patrol);
                if (original.name == "Run") overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(original, animationStateSO.chase);
                if (original.name == "Attack") overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(original, animationStateSO.attack);
            }

            overrideController.ApplyOverrides(overrides);
            cache.Add(animationStateSO, overrideController);
        }

        animator.runtimeAnimatorController = animatorOverrideController;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && enemyStateMachine.EnemyState is PatrolState && PlayerInView())
        {
            enemyStateMachine.ChangeState(chaseState);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && PlayerInView())
        {
            inView = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inView = false;
            if (enemyStateMachine.EnemyState is ChaseState)
            {
                enemyStateMachine.ChangeState(patrolState);
            }
        }
    
    }

    public void ChangeMatHue()
    {
        Color curColor = enemyMat.GetColor("_EmissionColor");
        Color.RGBToHSV(curColor, out float h, out float s, out float v);
        h = (h + Time.deltaTime * hueChangeSpeed) % 1f;
        Color finalColor = Color.HSVToRGB(h, s, 1.0f);
        enemyMat.SetColor("_EmissionColor", finalColor);
    }

    /*public bool PlayerInView()
    {
        Vector3 wiz = transform.position + Vector3.up * 1.5f;
        Debug.DrawRay(wiz, (player.position + Vector3.up - wiz).normalized * GetComponent<SphereCollider>().radius, Color.red, 0.1f);
        if (Physics.Raycast(wiz, (player.position + Vector3.up - wiz).normalized, out RaycastHit hit, GetComponent<SphereCollider>().radius))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }*/

    public bool PlayerInView()
    {
        Vector3 origin = transform.position + Vector3.up * 1.5f;
        Vector3 target = player.position + Vector3.up * 1.5f;

        Vector3 dir = (target - origin);
        float dist = dir.magnitude;
        dir.Normalize();
        if (Physics.Raycast(origin, dir, out RaycastHit hit, dist))
        {
            if (hit.transform != player)
                return false;
        }
        var terrain = Terrain.activeTerrain;
        var trees = terrain.terrainData.treeInstances;

        foreach (var tree in trees)
        {
            Vector3 treePos = Vector3.Scale(tree.position, terrain.terrainData.size)
                              + terrain.transform.position;

            float radius = 1.5f * tree.widthScale;

            Vector3 toTree = treePos - origin;
            float projection = Vector3.Dot(toTree, dir);

            if (projection < 0 || projection > dist)
                continue;

            Vector3 closest = origin + dir * projection;

            if (Vector3.Distance(treePos, closest) < radius)
                return false;
        }

        return true;
    }

}


public enum EnemyWizardAnimationTrigger
{
    insideView,
    outsideView,
    insideAttackRange,
    outsideAttackRange,
}