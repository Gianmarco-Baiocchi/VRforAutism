using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class FPSInteractionManager : MonoBehaviour
{
    [SerializeField] private Transform _fpsCameraT;
    [SerializeField] private Transform _grabPoint;
    [SerializeField] private bool _debugRay;
    [SerializeField] private float _interactionDistance;
    [SerializeField] private float _cartDistance;

    [SerializeField] private Image _target;
    [SerializeField] private Image _infoLabel;

    private Interactable _pointingInteractable;
    private Grabbable _pointingGrabbable;

    private CharacterController _fpsController;
    private Vector3 _rayOrigin;

    private Person _person;
    private Grabbable _grabbedObject = null;
    private Grabbable _grabbedList = null;
    private Cart _userCart;
    private ShoppingList _userShoppingList;



    void Start()
    {
        _fpsController = GetComponent<CharacterController>();
        _person = GetComponent<Person>();
        _userCart = _person.Cart;
        _userShoppingList = _person.ShoppingList;
    }

    void Update()
    {
        _rayOrigin = _fpsCameraT.position + _fpsController.radius * _fpsCameraT.forward;

        if (_grabbedList == null)
        {
            if (Input.GetKey(KeyCode.E) && _grabbedObject == null)
            {
                _grabbedList = GetComponentInChildren<ShoppingListGUI>().GetComponent<Grabbable>();
                if (_grabbedList != null)
                {
                    _grabbedObject = _grabbedList;
                    _grabbedObject.Grab(_grabbedObject.gameObject);
                    Grab(_grabbedObject);
                }
            }
            else
            {
                if (_grabbedObject == null)
                    CheckInteraction();

                if (_grabbedObject != null && Input.GetMouseButtonDown(0))
                    Drop();

                UpdateUITarget();

                if (_debugRay)
                    DebugRaycast();
            }
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            Drop();
            _grabbedList = null;
        }
    }

    private void CheckInteraction()
    {
        Ray ray = new Ray(_rayOrigin, _fpsCameraT.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _interactionDistance))
        {
            //Check if is interactable
            _pointingInteractable = hit.transform.GetComponent<Interactable>();
            if (_pointingInteractable)
            { 
                if(Input.GetMouseButtonDown(0))
                    _pointingInteractable.Interact(hit.transform.gameObject);
            }

            //Check if is grabbable
            _pointingGrabbable = hit.transform.GetComponent<Grabbable>();
            if (_grabbedObject == null && _pointingGrabbable)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    _pointingGrabbable.Grab(hit.transform.gameObject);
                    _pointingGrabbable.transform.position = _grabPoint.position;
                    _pointingGrabbable.transform.rotation = _grabPoint.rotation;
                    Grab(_pointingGrabbable);
                }
                    
            }
        }
        else
        {
            _pointingInteractable = null;
            _pointingGrabbable = null;
        }
    }

    private void UpdateUITarget()
    {
        if (_pointingInteractable)
            _target.color = Color.green;
        else if (_pointingGrabbable)
            _target.color = Color.yellow;
        else
            _target.color = Color.red;
    }

    private void Drop()
    {
        if (_grabbedObject == null)
            return;
        _grabbedObject.transform.parent = _grabbedObject.OriginalParent;
        if (_grabbedObject.GetComponent<ItemGrabbable>() != null)
        {
            ItemGrabbable grabbedItem = _grabbedObject.GetComponent<ItemGrabbable>();
            grabbedItem.Drop(IsFacingUserCart(), _userCart, _userShoppingList);
            grabbedItem = null;
            ShowLabelInfo(false);
        }
        else _grabbedObject.Drop();

        _target.enabled = true;
        _grabbedObject = null;
    }

    private void Grab(Grabbable grabbable)
    {
        _grabbedObject = grabbable;
        if (_grabbedObject.GetComponent<ItemGrabbable>() != null)
        {
            Item item = _grabbedObject.GetComponent<Item>();
            SetLabelInfoText(item.ItemName,  "Price: " + item.Price.ToString("0.00") + "€");
            ShowLabelInfo(true);
        }
        grabbable.transform.SetParent(_fpsCameraT);
        _target.enabled = false;
    }

    private void DebugRaycast()
    {
        Debug.DrawRay(_rayOrigin, _fpsCameraT.forward * _interactionDistance, Color.red);
    }

    private bool IsFacingUserCart()
    {
        Ray ray = new Ray(_rayOrigin, _fpsCameraT.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _cartDistance))
        {
            Cart facingCart = hit.transform.gameObject.GetComponent<Cart>();
            return facingCart == _userCart;
        }
        return false;
    }
    
    private void ShowLabelInfo(bool state)
    {
        _infoLabel.GetComponent<Image>().enabled = state;
        var textsComponents = _infoLabel.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var textComponent in textsComponents)
        {
            textComponent.enabled = state;
        }
    }
    
    private void SetLabelInfoText(string mainText, string secondaryText)
    {
        var textsComponents = _infoLabel.GetComponentsInChildren<TextMeshProUGUI>();
        textsComponents[0].text = mainText;
        textsComponents[1].text = secondaryText;
    }
    
}
