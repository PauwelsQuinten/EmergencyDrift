using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Human : MonoBehaviour
{
    [SerializeField] private float _WalkDistance = 3f;
    [SerializeField] private LayerMask _obstacleMask;

    [SerializeField] private FloatReference _lookDistance;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private GameEvent _hit;
    [SerializeField] private GameEvent _Saw;

    private NavMeshAgent _agent;
    private DetectPeople _detectPeople;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _detectPeople = GetComponentInChildren<DetectPeople>();
    }

    void Update()
    {
        if (_agent.remainingDistance <= _agent.stoppingDistance + 0.2f)
        {
            Vector3 point;
            if(RandomPoint(transform.position + new Vector3(0,0.5f,0), _WalkDistance, out  point)) 
            {
                _agent.SetDestination(point);
            }
        }
    }

    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if(NavMesh.SamplePosition(randomPoint, out hit,_agent.radius, NavMesh.AllAreas)) 
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player") return;

        if (_detectPeople.IsNearOtherHuman) _Saw.Raise();
        else _hit.Raise(this, this);
        Destroy(gameObject);
    }
    public void CheckAllerted(Component sender, object obj)
    {
        //List<Transform> targets = GetTargets(sender.gameObject);

        //if (targets.Count <= 0) return;
        //foreach (Transform target in targets)
        //{
        //    if (IsInLineOfSight(target))
        //    {
        //        _Saw.Raise();
        //        Debug.Log("OMG he hit a person");
        //    }
        //}
    }

    private List<Transform> GetTargets(GameObject sender)
    {
        List<Collider> colliders =  Physics.OverlapSphere(transform.position, _lookDistance.value).ToList();
        List<Transform> targets = new List<Transform>();
        foreach (Collider collider in colliders)
        {
            if(collider.gameObject == sender) targets.Add(collider.transform);
        }
        return targets;
    }

    private bool IsInLineOfSight(Transform target)
    {
        // Calculate the direction from this object to the target
        Vector3 directionToTarget = target.position - transform.position;

        // Get the distance to the target
        float distanceToTarget = directionToTarget.magnitude;

        // Normalize the direction to use it in the raycast
        directionToTarget.Normalize();

        // Perform the raycast
        if (Physics.Raycast(transform.position, directionToTarget, out RaycastHit hitInfo, _lookDistance.value, _layerMask))
        {
            // Check if the object hit by the raycast is the target
            if (hitInfo.transform == target)
            {
                // The target is in line of sight
                return true;
            }
        }

        // The target is not in line of sight
        return false;
    }
}
