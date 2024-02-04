using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private DynamicJoystick joystick;
    [SerializeField] private Animator animator;
    [SerializeField] private  float speed = 5f;
    [SerializeField] private  float rotationSpeed = 10f;
    [SerializeField] private List<GameObject> stack;
    [SerializeField] private List<Product.ProductType> stackProducts;
    [SerializeField] private int currentStackSize;
    [SerializeField] private int maxStackSize;
    [SerializeField] private  Transform stackSpawnPoint; // Точка создания продуктов
    [SerializeField] private Ease easing;
    
    private Rigidbody rb;
    private Vector3 pos;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
        
        animator.SetFloat("Speed", rb.velocity.sqrMagnitude);
    }

    private void Move()
    {
        Vector3 movement = new Vector3(joystick.Horizontal, 0f, joystick.Vertical);
        rb.velocity = new Vector3(movement.x * speed, rb.velocity.y, movement.z * speed);
    }
    
    private void Rotate()
    {
        if (Input.GetMouseButton(0))
        {
            float targetAngle = Mathf.Atan2(joystick.Horizontal, joystick.Vertical) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    
    public void GetProduct(GameObject obj)
    {
        if (currentStackSize < maxStackSize)
        {
            currentStackSize++;
            
            pos = stackSpawnPoint.localPosition + new Vector3(0, 0.35f * currentStackSize, 0);
            obj.transform.SetParent(stackSpawnPoint);
            obj.transform.DOLocalMove(pos, 0.5f).SetEase(easing);
            stack.Add(obj);
            stackProducts.Add(obj.GetComponent<Product>().type);
            
            animator.SetBool("WithProduct", currentStackSize != 0);
        }
    }

    public void RemoveProduct(Transform tableTransform, Visitor visitor)
    {
        if (currentStackSize > 0)
        {
            for (int i = 0; i < stack.Count; i++)
            {
                if (stack[i].transform.TryGetComponent(out Product product))
                {
                    GameManager.instance.CalculateMoney(product.income);
                    product.transform.DOMove(tableTransform.position, 1.5f).SetEase(easing);
                    product.transform.SetParent(tableTransform);
                    stack.RemoveAt(i);
                    --currentStackSize;
                    print(currentStackSize);
                    animator.SetBool("WithProduct", currentStackSize != 0);
                    
                    Debug.Log("Отдаю продукт посетителю!");
                }
            }

            RecalculateStackPositions();
        }
    }

    public List<Product.ProductType> GetProductsStack()
    {
        return stackProducts;
    }
    
    private void RecalculateStackPositions()
    {
        for (int i = 0; i < stack.Count; i++)
        {
            pos = stackSpawnPoint.localPosition + new Vector3(0, 0.35f * i, 0);
            stack[i].transform.DOLocalMove(pos, 0.5f).SetEase(Ease.Linear);
        }
    }
}