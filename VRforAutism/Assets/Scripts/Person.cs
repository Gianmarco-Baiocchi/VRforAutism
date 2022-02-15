using System.Collections.Generic;
using Attributes;
using UnityEngine;

public class Person : MonoBehaviour
{
//    [SerializeField] private bool _isUserCharacter;
    //[ShowIf(ActionOnConditionFail.DontDraw, ConditionOperator.And, 
    //    nameof(_isUserCharacter)), SerializeField] private Cart _cart;
    [SerializeField] private string _username;
    private bool _isMoving;
    private ShoppingList _shoppingList;

    //public bool IsUserCharacter => _isUserCharacter;
    //public Cart Cart => _cart;

    public string Username => _username;
    public ShoppingList ShoppingList { get => _shoppingList; set => _shoppingList = value; }

    public Dictionary<Item, int> ExtraTakenItems { get; set; }

    public bool IsMoving  { get => _isMoving; set => _isMoving = value; }

    // Start is called before the first frame update
    void Start()
    {
        ShoppingList = new ShoppingList();
        ExtraTakenItems = new Dictionary<Item, int>();
        _isMoving = true;
    }

    public void AddExtraItem(Item newItem)
    {
        //Debug.Log(ExtraTakenItems.Keys.First(item => item.ItemName.Equals(newItem.ItemName)));
        foreach (var item in ExtraTakenItems.Keys)
        {
            if (item.ItemName.Equals(newItem.ItemName))
            {
                ExtraTakenItems[item]++;
                return;
            }
        }
        Debug.Log(newItem.ItemName + "   " + ExtraTakenItems.Count);
        ExtraTakenItems.Add(newItem, 1);
    }
}
