using System;
using System.Collections.Generic;
using System.Diagnostics;
using Attributes;
using JetBrains.Annotations;
using UnityEngine;

public class Person : MonoBehaviour
{
    [SerializeField] private bool _isUserCharacter;
    [ShowIf(ActionOnConditionFail.DontDraw, ConditionOperator.And, 
        nameof(_isUserCharacter)), SerializeField] private Cart _cart;
    [ShowIf(ActionOnConditionFail.DontDraw, ConditionOperator.And, 
         nameof(_isUserCharacter)), SerializeField] private ShoppingList _shoppingList;
    private string _username;
    
    public Cart Cart => _cart;
    public ShoppingList ShoppingList => _shoppingList;

    // Start is called before the first frame update
    void Start()
    {
        _username = "User1";
        if (!_isUserCharacter)
        {
            _shoppingList = new ShoppingList();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
