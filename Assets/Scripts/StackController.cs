using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class StackController : MonoBehaviour
{
    [SerializeField] private  GameObject productPrefab;
    [SerializeField] private  Transform stackSpawnPoint; // Точка создания продуктов
    [SerializeField] private  int maxStackSize = 5;
    [SerializeField] private int currentStackSize = 0;
    [SerializeField] private Employee employee;
    [SerializeField] private float timeInterval = 5f;
    [SerializeField] private List<GameObject> stack;

    private float timer = 0f;
    private bool playerInsideTrigger = false;
    private Vector3 pos;
    private PlayerController player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        // Проверяем, находится ли игрок в триггере
        if (playerInsideTrigger && currentStackSize > 0)
        {
            // Обновляем таймер
            timer += Time.deltaTime;

            // Если прошло достаточно времени, создаем продукт
            if (timer >= timeInterval)
            {
                RemoveProduct();
                timer = 0f;
            }
        }
    }

    public void CreateProduct()
    {
        if (currentStackSize < maxStackSize)
        {
            currentStackSize++;

            pos = stackSpawnPoint.position + new Vector3(0, 0.35f * currentStackSize, 0);
            var newProduct = Instantiate(productPrefab, pos, Quaternion.identity);
            newProduct.transform.SetParent(transform);
            stack.Add(newProduct);

            if (currentStackSize == maxStackSize)
            {
                employee.PauseProduction();
            }
        }
    }

    private void RemoveProduct()
    {
        player.GetProduct(stack[currentStackSize-1]);
        stack.RemoveAt(currentStackSize-1);
        --currentStackSize;

        if (currentStackSize == maxStackSize - 1)
        {
            employee.ResumeProduction();
            Debug.Log("Возобновляем готовку");
        }

        Debug.Log("Игрок забрал продукт!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = true;
            Debug.Log("Вошел в триггер");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = false;
            Debug.Log("Вышел из триггера");
        }
    }
}