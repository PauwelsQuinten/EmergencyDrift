using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireSystem : MonoBehaviour
{
    [SerializeField] private GameObject _heatBarObject;
    [SerializeField] private float _heatFactor = 2f;
    [SerializeField] private float _heatTimeScale;
    [SerializeField] private float _timeToReset;

    [SerializeField] private GameEvent _yourFired;

    private Slider _heatBarSlider;
    private float _currentHeat;
    private float _timeBetweenHits;
    private float _resetTimer;
    private bool _doResetTimer = true;

    private void Start()
    {
        _heatBarSlider = _heatBarObject.GetComponent<Slider>();
        _heatBarSlider.value = 0;
    }
    private void Update()
    {
        _timeBetweenHits += Time.deltaTime / _heatTimeScale;
        if(_doResetTimer) _resetTimer += Time.deltaTime;

        ReduceHeat();
        _heatBarSlider.value = _currentHeat;
    }

    public void HitPerson()
    {
        _timeBetweenHits = Mathf.Clamp01(_timeBetweenHits);
        _currentHeat += _heatFactor * _timeBetweenHits;
        _timeBetweenHits = 0;
        _resetTimer = 0;
        _doResetTimer = true;
        _heatBarSlider.value = _currentHeat;

        if(_currentHeat >= 100) _yourFired.Raise();
    }

    public void PeopleSpottedYou()
    {
        _currentHeat += _heatFactor * 2;
        _timeBetweenHits = 0;
        _resetTimer = 0;
        _doResetTimer = true;
        _heatBarSlider.value = _currentHeat;
        if (_currentHeat >= 100) _yourFired.Raise();
    }

    private void ReduceHeat()
    {
        if (_resetTimer < _timeToReset) return;
        _doResetTimer = false;
        if (_currentHeat == 0) return;
        _currentHeat -=  5 * Time.deltaTime;
    }
}
