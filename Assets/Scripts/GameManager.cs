using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private int cash = 0;

    private void Awake()
    {
        Init();
        
        SecurePlayerPrefs.Init();
    }

    private void Init()
    {
        instance = this;
    }

    public void CalculateMoney(int number)
    {
        cash += number;
        SecurePlayerPrefs.SetInt(Constants.k_cashCount, cash);
        SecurePlayerPrefs.Save();
        UIManager.instance.UpdateCashText();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        SecurePlayerPrefs.Save();
    }

    private void OnApplicationQuit()
    {
        SecurePlayerPrefs.Save();
    }
}
