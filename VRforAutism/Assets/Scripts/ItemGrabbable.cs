using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Item))]
public class ItemGrabbable : Grabbable
{
    private Item _item;

    private IEnumerator SetIsKinematic(bool isKinematic)
    {
        yield return new WaitForSeconds(0.5f);
        this.GetComponent<Rigidbody>().isKinematic = isKinematic;
    }

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

        _item.GetComponent<Collider>().enabled = false;
    }

    public override void Drop()
    {
        Drop(false, null, null);
        //Drop(null, false);
    }

    /*public void Drop(Person person, bool toTake)
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
                _item.GetComponent<Collider>().enabled = true;
            }
            _item = null;
        }
    }*/
    
   
    public void Drop(bool isFacingCart, Cart cart, Person person)
    {
        if (_item != null)
        {
            var rigidBody = _item.GetComponent<Rigidbody>();
            if (!rigidBody) return;
            rigidBody.useGravity = true;
            rigidBody.detectCollisions = true;

            if (isFacingCart  && !_item.IsInsideCart)
            {
                _item.transform.position = cart.GetInsideCartPosition();
                Debug.Log(cart.GetInsideCartPosition());
                _item.transform.parent = cart.gameObject.transform;
                _item.IsInsideCart = true;
                if (person.ShoppingList.IsItemOnList(_item) && person.ShoppingList.IsItemMustBeTaken(_item))
                    person.ShoppingList.ItemTaken(_item);
                else
                    person.AddExtraItem(_item);
                StartCoroutine("SetIsKinematic", true);
                //new List<Renderer>(_item.GetComponentsInChildren<Renderer>()).ForEach(r => r.enabled = false);;
            }
            else if(!isFacingCart)
            {
                _item.transform.position = _item.GetInitialPosition();
                _item.transform.rotation = _item.GetInitialRotation();
                if (_item.IsInsideCart)
                {
                    if (person.IsItemInExtraItem(_item))
                        person.RemoveExtraItem(_item);
                    else 
                        person.ShoppingList.ItemRemoved(_item);
                    _item.IsInsideCart = false;
                    StartCoroutine("SetIsKinematic", false);
                }
                
            }
            _item.GetComponent<Collider>().enabled = true;
            rigidBody.velocity = Vector3.zero;
            rigidBody.angularVelocity = Vector3.zero;
            _item = null;
        }
    }
}