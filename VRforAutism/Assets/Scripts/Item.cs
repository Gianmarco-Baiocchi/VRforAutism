using UnityEngine;
using System;

public class Item : MonoBehaviour
{
    [SerializeField] private  float _price;
    [SerializeField] private string _itemName;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private bool _insideCart;
    private enum Categories {Fruit, Vegetable, Meat, Fish, Beverage, Fresh, Frozen};
    [SerializeField] private Categories category;

    public float Price => _price;
    public string ItemName => _itemName;
    public bool IsInsideCart { get => _insideCart; set => _insideCart = value; }
    
    void Start()
    {
        _insideCart = false;
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
    }

    public Vector3 GetInitialPosition()
    {
        return _initialPosition;
    }

    public Quaternion GetInitialRotation()
    {
        return _initialRotation;
    }

}

