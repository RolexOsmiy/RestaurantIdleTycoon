using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Employee : MonoBehaviour
{
    [SerializeField] private StackController stackController;
    [SerializeField] private float initialInterval = 2.0f; // начальный интервал в секундах
    [SerializeField] private float accelerationFactor = 0.9f; // фактор ускорения
    [SerializeField] private float nextProductTime;
    
    private bool isProductionPaused = false;

    private void Start()
    {
        nextProductTime = Time.time + initialInterval;
    }

    private void Update()
    {
        if (Time.time >= nextProductTime)
        {
            nextProductTime += initialInterval;
            AdjustProductionSpeed();
            CreateProduct();
        }
    }

    public void CreateProduct()
    {
        stackController.CreateProduct();
    }

    private void AdjustProductionSpeed()
    {
        initialInterval *= accelerationFactor; // увеличиваем скорость производства
    }
    
    public void PauseProduction()
    {
        isProductionPaused = true;
    }
    
    public void ResumeProduction()
    {
        isProductionPaused = false;
    }
}

