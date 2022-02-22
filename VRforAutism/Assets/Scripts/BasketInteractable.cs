using System;
using UnityEngine;

[RequireComponent(typeof(Cart))]
public class BasketInteractable : Interactable
{
    private bool _isGrabbed= false;
    private bool _firstTime= true;
    private Vector3 _initialScale;
    public bool IsGrabbed => _isGrabbed;

    public override void Interact(GameObject caller)
    {
        _isGrabbed = !_isGrabbed;
        var o = gameObject;
        if (_firstTime)
        {
            _firstTime = false;
            _initialScale = o.transform.localScale;
        }
        if (!_isGrabbed)
        {
            var position = o.transform.position;
            o.transform.parent = null;
            position = new Vector3(position.x, 0.23f, position.z);
            o.transform.position = position;
            o.transform.localScale = _initialScale;
        }
        else
        {
            var playerTransform = caller.transform;
            o.transform.position = playerTransform.position;
            o.transform.parent = playerTransform;
            o.transform.rotation = playerTransform.rotation;
        }
    }
}