using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Item))]
public class ItemGrabbable : Grabbable
{
    private Item _item;

    public override void Grab(GameObject caller)
    {
        _item = caller.GetComponent<Item>();
        if (!_item) return;
        _item.transform.rotation = _item.GetInitialRotation();

        var rigidBody = _item.GetComponent<Rigidbody>();
        if (!rigidBody) return;
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        rigidBody.useGravity = false;
        rigidBody.detectCollisions = false;
    }

    public override void Drop()
    {
        //Drop(false, null, null);
        Drop(null, false);
    }

    public void Drop(Person person, bool toTake)
    {
        if (_item != null)
        {
            var rigidBody = _item.GetComponent<Rigidbody>();
            if (!rigidBody) return;
            rigidBody.useGravity = true;
            rigidBody.detectCollisions = true;

            if (toTake)
            {
                //Debug.Log(person.ShoppingList.IsItemOnList(_item));
                if (person.ShoppingList.IsItemOnList(_item) && person.ShoppingList.IsItemMustBeTaken(_item))
                    person.ShoppingList.ItemTaken(_item);
                else
                    person.AddExtraItem(_item);
                new List<Renderer>(_item.GetComponentsInChildren<Renderer>()).ForEach(r => r.enabled = false);
            }
            else
            {
                _item.transform.position = _item.GetInitialPosition();
                _item.transform.rotation = _item.GetInitialRotation();
                rigidBody.velocity = Vector3.zero;
                rigidBody.angularVelocity = Vector3.zero;
            }
            _item = null;
        }
    }
    
    /*
    public void Drop(bool isFacingCart, Cart cart, ShoppingList shoppingList)
    {
        if (_item != null)
        {
            var rigidBody = _item.GetComponent<Rigidbody>();
            if (!rigidBody) return;
            rigidBody.useGravity = true;
            rigidBody.detectCollisions = true;

            if (shoppingList.IsItemMustBeTaken(_item) && isFacingCart  && !_item.IsInsideCart)
            {
                cart.AddInsideCart(_item); //TODO: verificare se si può elimare Cart, facendo direttamente item.SetInsideCart;
                shoppingList.ItemTaken(_item);
            }
            else
            {
                _item.transform.position = _item.GetInitialPosition();
                _item.transform.rotation = _item.GetInitialRotation();
                if (_item.IsInsideCart)
                {
                    cart.RemoveFromCart(_item); //TODO: verificare se si può elimare Cart, facendo direttamente item.SetInsideCart;
                    shoppingList.ItemRemoved(_item);
                }
            }
            rigidBody.velocity = Vector3.zero;
            rigidBody.angularVelocity = Vector3.zero;
            _item = null;
        }
    }*/
}