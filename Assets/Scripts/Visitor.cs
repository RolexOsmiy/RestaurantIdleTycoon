using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Visitor : MonoBehaviour
{
    [SerializeField] private List<Product.ProductType> requestedProducts;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;

    private VisitorController _visitorController; 

    private void Start()
    {
        requestedProducts.Add(Product.ProductType.Coffee1);
        requestedProducts.Add(Product.ProductType.Coffee2);
    }
    
    private void FixedUpdate()
    {
        animator.SetFloat("Speed", agent.velocity.sqrMagnitude);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RemoveRequestedProducts(other.GetComponent<PlayerController>().GetProductsStack());
            other.GetComponent<PlayerController>().RemoveProduct(transform, this);
            _visitorController.QueueNext();
            Debug.Log("Игрок в радиусе выдачи!");
        }
    }

    public void SetVisitorController(VisitorController visitorController)
    {
        _visitorController = visitorController;
    }

    private void RemoveRequestedProducts(List<Product.ProductType> productsToRemove)
    {
        for (int i = 0; i < requestedProducts.Count; i++)
        {
            for (int j = 0; j < productsToRemove.Count; j++)
            {
                if (requestedProducts[i] == productsToRemove[j])
                {
                    // Уменьшаем количество или удаляем, в зависимости от вашего случая
                    requestedProducts.RemoveAt(i);
                    i--; // Уменьшаем индекс, так как мы удалили элемент
                    break; // Прерываем внутренний цикл, так как элемент уже найден и обработан
                }
            }
        }
    }

    public void WalkTo(Vector3 position)
    {
        agent.SetDestination(position);
    }

    #region Animator

    public void WithProduct()
    {
        animator.SetBool("WithProduct", true);
    }
    
    public void WithoutProduct()
    {
        animator.SetBool("WithProduct", false);
    }

    #endregion
}
