using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(NavMeshAgent))]
[RequireComponent (typeof(Person))]
public class NPCController : MonoBehaviour
{
    private GameObject _target;
    private NavMeshAgent _navMeshAgent;
    private Person _person;
    private bool _isItemParent; //se gli oggetti hanno un parent di riferimento allora la variabile sarà true
    private ItemOnList _itemsToBuy;
    private Vector3 _directionToTarget;
    private bool _isDirectionToTargetSet;
    private bool _isLookingAtTarget;

    private int _state;

    
    void Start ()
    {
        _person = GetComponent<Person>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        if (_target != null)
            _target.SetActive(false);
        _isDirectionToTargetSet = false;
        _isLookingAtTarget = false;
        _state = 0;
    }
	
	
    void Update ()
    {
        switch (_state)
        {
            case 0:
                if (_target == null)
                {
                    _isDirectionToTargetSet = false;
                    _isLookingAtTarget = false;
                    Debug.Log(_person.ShoppingList.ItemsToTake().Count);
                    if (_person.ShoppingList.isShoppingFinished())
                    {
                        _state = 1;
                        break;
                    }
                    _target = GetNewItemTarget();
                    _navMeshAgent.SetDestination(_target.transform.position);
                }

                if (_target != null && IsTargetReached())
                {
                    if (!_isDirectionToTargetSet)
                    {
                        _directionToTarget = _target.transform.position - transform.position;
                        _directionToTarget.y = 0f;
                        _directionToTarget.Normalize();
                    }
                    if (!_isLookingAtTarget) 
                    {
                        Vector3 newDir = Vector3.RotateTowards(transform.forward, _directionToTarget, 2 * Time.deltaTime, 0f);
                        transform.rotation = Quaternion.LookRotation(newDir);
                        if (isLooking(newDir))
                            _isLookingAtTarget = true;
                    }
                    else
                    {
                        GrabItems();
                        _target = null;
                    }

                }
                break;
            case 1:
                if (_target == null)
                {
                    _isDirectionToTargetSet = false;
                    _isLookingAtTarget = false;
                    _target = GetNewCashRegisterTarget();
                    _navMeshAgent.stoppingDistance = 5f;
                    _navMeshAgent.SetDestination(_target.transform.position);
                }
                
                if (_target != null && IsTargetReached())
                {
                    if (!_isDirectionToTargetSet)
                    {
                        _directionToTarget = _target.transform.position - transform.position;
                        _directionToTarget.y = 0f;
                        _directionToTarget.Normalize();
                    }
                    if (!_isLookingAtTarget) 
                    {
                        Vector3 newDir = Vector3.RotateTowards(transform.forward, _directionToTarget, 2 * Time.deltaTime, 0f);
                        transform.rotation = Quaternion.LookRotation(newDir);
                        if (isLooking(newDir))
                            _isLookingAtTarget = true;
                    }
                    else
                    {
                        _state = 2;
                    }
                }
                break;
            case 2:
                break;
        }
    }

    private GameObject GetNewItemTarget()
    {
        _itemsToBuy = _person.ShoppingList.ItemsToTake()[0];
        var transforms = _itemsToBuy.Item.GetComponentsInParent<Transform>();
        if (transforms.Length == 1)
        {
            _isItemParent = false;
            return transforms[0].gameObject;
        }
        _isItemParent = true;
        return transforms[1].gameObject;                        //caso in cui ho gli elementi con un empty padre
    }

    private GameObject GetNewCashRegisterTarget()
    {
        return GameObject.FindWithTag("CashRegistersPoint").GetComponentInChildren<CashRegister>().gameObject;
    }

    private bool IsTargetReached()
    {
        if (!_navMeshAgent.pathPending)
            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude <= 0f)
                    return true;
        return false;    
    }
    
    private void GrabItems()
    {
        if (_itemsToBuy == null) return;
        GameObject item = null;
        Item itemComponent;
        while (!_itemsToBuy.IsAllTaken)
        {
            if (_isItemParent)
            {
                itemComponent = _target.GetComponentInChildren<Item>();
            }
            else
            {
                itemComponent = _target.GetComponent<Item>();
            }
            if (itemComponent != null && !itemComponent.IsInsideCart)//così, nel caso non ci siano oggetti a sufficienza, semplicemente non li prende, e continua
            {
                item = itemComponent.gameObject;
                item.SetActive(false);
            }
            _itemsToBuy.takeOne();
        }
        _itemsToBuy = null;
    }

    private bool isLooking(Vector3 vector)
    {
        return (vector.x.ToString("0.00") == "0,00" && vector.y.ToString("0.00") == "0,00" && 
                (vector.z.ToString("0.00") == "1,00" || vector.z.ToString("0.00") == "-1,00")) ||
               (vector.y.ToString("0.00") == "0,00" && vector.z.ToString("0.00") == "0,00" && 
                (vector.x.ToString("0.00") == "1,00" || vector.x.ToString("0.00") == "-1,00")) ||
               (vector.z.ToString("0.00") == "0,00" && vector.x.ToString("0.00") == "0,00" && 
                (vector.y.ToString("0.00") == "1,00" || vector.y.ToString("0.00") == "-1,00"));
    }
}