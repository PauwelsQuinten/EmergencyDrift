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

    public void ActivateWarning(Component sender, object obj)
    {
        _tooCloseMessage.SetActive(true);
    }

    public void DisableWarning(Component sender, object obj)
    {
        _tooCloseMessage.SetActive(false);
    }
}
