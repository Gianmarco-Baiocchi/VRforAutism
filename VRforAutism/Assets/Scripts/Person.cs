using System.Collections.Generic;
using System.Linq;
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
        foreach (var item in ExtraTakenItems.Keys)
        {
            if (item.ItemName.Equals(newItem.ItemName))
            {
                ExtraTakenItems[item]++;
                return;
            }
        }
        //Debug.Log(newItem.ItemName + "   " + ExtraTakenItems.Count);
        ExtraTakenItems.Add(newItem, 1);
    }
    
    public void RemoveExtraItem(Item newItem)
    {
        //Debug.Log(ExtraTakenItems.Keys.First(item => item.ItemName.Equals(newItem.ItemName)));
        foreach (var item in ExtraTakenItems.Keys.Where(item => item.ItemName.Equals(newItem.ItemName)))
        {
            if (ExtraTakenItems[item] == 1)
                ExtraTakenItems.Remove(item);
            else
                ExtraTakenItems[item]--;
            return;
        }
    }

    public bool IsItemInExtraItem(Item item)
    {
        foreach (var key in ExtraTakenItems.Keys)
        {
            if (key.ItemName.Equals(item.ItemName))
                return true;
        }
        return false;
    }
}
