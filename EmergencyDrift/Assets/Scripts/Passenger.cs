using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passenger : MonoBehaviour
{
    [SerializeField] private GameObject _tooCloseMessage;

    private void Start()
    {
        _tooCloseMessage.SetActive(false);
    }

    public void ActivateWarning()
    {
        _tooCloseMessage.SetActive(true);
    }

    public void DisableWarning()
    {
        _tooCloseMessage.SetActive(false);
    }
}
