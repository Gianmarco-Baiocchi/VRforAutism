using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cart : MonoBehaviour
{
    void Start()
    {
    }

    public void AddInsideCart(Item item)
    {
        if (!item.IsInsideCart)
        {
            item.IsInsideCart = true;
        }
    }

    public void RemoveFromCart(Item item)
    {
        if (item.IsInsideCart)
        {
            item.IsInsideCart = false;
        }
    }
}
