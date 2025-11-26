using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent _agent;
    private GameObject _target;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }
    public void SetTarget(Component sender, object obj)
    {
        SpawnPatientEventArgs patient = obj as SpawnPatientEventArgs;
        if (patient == null) return;
        _agent.SetDestination(patient.SpawnLocation.transform.position);
    }
}
