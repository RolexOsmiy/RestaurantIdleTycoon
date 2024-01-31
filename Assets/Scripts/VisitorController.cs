using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class VisitorController : MonoBehaviour
{
    public Transform startPoint;
    public Transform cashRegister;
    public Transform outPoint;
    public float queueSpacing = 1.5f;
    public int numberOfVisitors = 5;
    public GameObject visitorPrefab;

    private Transform[] queue;
    private int currentVisitorIndex = 0;

    private void Start()
    {
        CreateQueue();
        //StartCoroutine(ProcessQueue());
    }

    private void CreateQueue()
    {
        queue = new Transform[numberOfVisitors];

        // Создаем очередь
        for (int i = 0; i < numberOfVisitors; i++)
        {
            Vector3 queuePosition = startPoint.position + Vector3.right * i * queueSpacing;
            queue[i] = InstantiateVisitor(queuePosition);
        }

        // Перемещаем первого посетителя к кассе
        MoveVisitorToCashRegister(queue[0]);
    }

    /*private IEnumerator ProcessQueue()
    {
        while (true)
        {
            // Ожидаем, пока текущий посетитель не достигнет кассы
            yield return new WaitUntil(() => Vector3.Distance(queue[currentVisitorIndex].position, cashRegister.position) < 0.1f);

            // Вызываем метод для перемещения текущего посетителя к OutPoint и продвижения очереди
            QueueNext();

            // Ждем, пока очередь полностью обновится
            yield return new WaitForSeconds(1f); // Можно изменить время в зависимости от вашего времени обслуживания
        }
    }*/

    public void QueueNext()
    {
        // Перемещаем текущего посетителя к OutPoint
        MoveVisitorToOutPoint(queue[currentVisitorIndex]);

        // Увеличиваем индекс текущего посетителя
        currentVisitorIndex = (currentVisitorIndex + 1) % numberOfVisitors;

        // Перемещаем следующего посетителя к кассе
        MoveVisitorToCashRegister(queue[currentVisitorIndex]);
    }

    private Transform InstantiateVisitor(Vector3 position)
    {
        GameObject visitorGO = Instantiate(visitorPrefab, position, Quaternion.identity);
        visitorGO.GetComponent<Visitor>().SetVisitorController(this);
        return visitorGO.transform;
    }

    private void MoveVisitorToCashRegister(Transform visitor)
    {
        // Перемещаем посетителя к кассе
        visitor.GetComponent<Visitor>().WithoutProduct();
        visitor.GetComponent<NavMeshAgent>().SetDestination(cashRegister.position);
    }

    private void MoveVisitorToOutPoint(Transform visitor)
    {
        // Перемещаем посетителя к OutPoint
        visitor.GetComponent<Visitor>().WithProduct();
        visitor.GetComponent<NavMeshAgent>().SetDestination(outPoint.position);
    }
}
