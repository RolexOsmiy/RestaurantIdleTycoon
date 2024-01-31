using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product : MonoBehaviour
{
    public ProductType type;
    public int income = 1;

    public enum ProductType
    {
        Coffee1,
        Coffee2,
        Coffee3
    }
}
