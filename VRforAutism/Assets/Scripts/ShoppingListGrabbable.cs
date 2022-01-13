using UnityEngine;
[RequireComponent (typeof(ShoppingListGUI))]
public class ShoppingListGrabbable : Grabbable
{
    private ShoppingListGUI _listGUI;
    
    public override void Grab(GameObject caller)
    {
        if (_listGUI == null)
        {
            _listGUI = caller.GetComponent<ShoppingListGUI>();
        }
        _listGUI.SetState(true);
        _listGUI.WriteText();
    }

    public override void Drop()
    {
        _listGUI.SetState(false);
    }
    
}