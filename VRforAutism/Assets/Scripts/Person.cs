using Attributes;
using UnityEngine;

public class Person : MonoBehaviour
{
    [SerializeField] private bool _isUserCharacter;
    [ShowIf(ActionOnConditionFail.DontDraw, ConditionOperator.And, 
        nameof(_isUserCharacter)), SerializeField] private Cart _cart;
    [ShowIf(ActionOnConditionFail.DontDraw, ConditionOperator.And, 
         nameof(_isUserCharacter)), SerializeField] private ShoppingList _shoppingList;
    [SerializeField] private string _username;
    private bool _isMoving;
    public Cart Cart => _cart;

    public string Username => _username;
    public ShoppingList ShoppingList { get => _shoppingList; set => _shoppingList = value; }
    
    public bool IsMoving  { get => _isMoving; set => _isMoving = value; }

    // Start is called before the first frame update
    void Start()
    {
        if (!_isUserCharacter)
        {
            ShoppingList = new ShoppingList();
        }
        _isMoving = true;
    }
}
