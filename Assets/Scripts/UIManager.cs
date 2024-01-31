using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    
    [SerializeField] private TextMeshProUGUI cashText;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateCashText(); //Инициализуем текущее количество денег
    }

    public void UpdateCashText()
    {
        int number = SecurePlayerPrefs.GetInt(Constants.k_cashCount);
        cashText.text = FormatNumberWithSuffix(number);
    }
    
    private string FormatNumberWithSuffix(float number)
    {
        if (number < 1000f)
        {
            return number.ToString("F0");
        }
        else if (number < 1000000f)
        {
            return (number / 1000f).ToString("F2") + "k";
        }
        else if (number < 1000000000f)
        {
            return (number / 1000000f).ToString("F2") + "M";
        }
        else
        {
            return (number / 1000000000f).ToString("F2") + "B";
        }
    }
}
