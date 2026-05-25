using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementBehavior : MonoBehaviour
{

    [SerializeField] private Transform target;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float minEnemySpeed, maxEnemySpeed;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent.speed = Random.Range(minEnemySpeed, maxEnemySpeed);
        agent.baseOffset = Random.Range(0.25f, 1.5f);
    }

    void Update()
    {
        agent.SetDestination(target.position);    
    }

    public void ModifyAgentSpeed(float multiplier)
    {
        agent.speed *= 1 + multiplier;
    }
}
