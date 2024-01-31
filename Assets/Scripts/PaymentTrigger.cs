using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class PaymentTrigger : MonoBehaviour
{
    [SerializeField] private float paymentRate = 1.0f;  // Скорость отдачи денег в единицах в секунду
    [SerializeField] private int targetAmount = 100;  // Целевая сумма, при достижении которой появится рабочий
    [SerializeField] private GameObject workerPrefab;  // Префаб рабочего
    [SerializeField] private int currentAmount = 0;  // Текущая сумма денег
    [SerializeField] private TextMeshProUGUI text;
    private bool isInTrigger = false;  // Флаг, указывающий, находится ли игрок в триггерной зоне
    private Transform myTransform;
    private int _tempCounter = 0;

    private void Awake()
    {
        myTransform = transform;
        text.SetText((targetAmount-currentAmount).ToString());
    }


    // Вызывается при входе игрока в триггерную зону
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = true;
            StartCoroutine(PayMoney());
        }
    }

    // Вызывается при выходе игрока из триггерной зоны
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isInTrigger = false;
    }

    // Корутина для постепенной выплаты денег
    private IEnumerator PayMoney()
    {
        while (currentAmount < targetAmount && isInTrigger)
        {
            _tempCounter = (int) (paymentRate * Time.deltaTime);
            GameManager.instance.CalculateMoney(-_tempCounter);
            currentAmount += _tempCounter;
            text.SetText((targetAmount-currentAmount).ToString());
            yield return null;
        }

        if (currentAmount >= targetAmount && isInTrigger)
        {
            currentAmount = targetAmount;
            ActivateWorker();
        }
    }

    // Активировать префаб рабочего
    private void ActivateWorker()
    {
        Instantiate(workerPrefab, myTransform.position, myTransform.rotation);
        gameObject.SetActive(false);
    }

    // Вызывается для сохранения прогресса
    public void SaveProgress()
    {
        //для префсов
    }
}