using UnityEngine;

public class DetectPeople : MonoBehaviour
{
    [SerializeField] private SphereCollider _detectionZone;
    [SerializeField] private FloatReference _detectionDistance;

    [SerializeField] private GameEvent _peopleNearby;
    [SerializeField] private GameEvent _noPeopleNearby;

    public bool IsNearOtherHuman;
    private GameObject _human;
    // Start is called before the first frame update
    private void Start()
    {
        _detectionZone.radius = _detectionDistance.value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            IsNearOtherHuman = true;
            _human = other.gameObject;
            Debug.Log("found human");
        }
        if (other.tag != "Player") return;
        if (IsNearOtherHuman == false) return;

        _peopleNearby.Raise();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Player") return;
        if (IsNearOtherHuman == false) return;

        _peopleNearby.Raise();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            IsNearOtherHuman = false;
            _human = null;
            _noPeopleNearby.Raise();
        }
        if (other.tag != "Player") return;
        _noPeopleNearby.Raise();
    }
}
