using System.Collections;
using TMPro;
using UnityEngine;

public class HospitalManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _incomeText;
    [SerializeField] private TextMeshProUGUI _ambulanceCounterText;
    [SerializeField] private float _ambulanceCooldown;
    [SerializeField] private FloatReference _money;
    [SerializeField] private FloatReference _maxAmbulances;

    [SerializeField] private GameEvent _lostAllMoney;

    private float _ambulanceCounter;
    private bool _peopleSaw;

    private void Start()
    {
        _incomeText.enabled = false;
        _ambulanceCounter = _maxAmbulances.value;
        UpdateMoney();
    }
    public void MoneyGained()
    {
        if (_ambulanceCounter == 0) return;
        _peopleSaw = false;
        int income = RandomMoney(150, 360);
        _money.variable.value += income;
        UpdateMoney();

        _incomeText.text = $"+ ${income}";
        _incomeText.color = Color.green;
        _incomeText.enabled = true;
        StartCoroutine(IncomeTimer(0.7f));

        _ambulanceCounter -= 1;
        _ambulanceCounterText.text = $"{_ambulanceCounter} / {_maxAmbulances.value}";
        StartCoroutine(AmbulanceTimer(_ambulanceCooldown));
    }

    public void MoneyLost()
    {
        if (_peopleSaw) return;
        _peopleSaw = true;
        int income = RandomMoney(-580, -270);
        _money.variable.value += income;
        UpdateMoney();

        _incomeText.text = $"- ${Mathf.Abs(income)}";
        _incomeText.color = Color.red;
        _incomeText.enabled = true;
        StartCoroutine(IncomeTimer(0.7f));
    }

    private int RandomMoney(int min, int max)
    {
        return (int)Random.Range(min, max + 1);
    }

    private IEnumerator IncomeTimer(float timer)
    {
        yield return new WaitForSeconds(timer);
        _incomeText.enabled = false;
    }

    private IEnumerator AmbulanceTimer(float timer)
    {
        yield return new WaitForSeconds(timer);
        if (_ambulanceCounter == _maxAmbulances.value) yield break;
        _ambulanceCounter += 1;
        _ambulanceCounterText.text = $"{_ambulanceCounter} / {_maxAmbulances.value}";
    }

    public void BoughtAmbulance()
    {
        _ambulanceCounter += 1;
        _maxAmbulances.variable.value += 1;
        _ambulanceCounterText.text = $"{_ambulanceCounter} / {_maxAmbulances.value}";
    }

    public void UpdateMoney()
    {
        _moneyText.text = $"${_money.value}";
        if (_money.value <= 0) _lostAllMoney.Raise();
    }
}
