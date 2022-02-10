using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(NavMeshAgent))]
[RequireComponent (typeof(Person))]

public class Customer : MonoBehaviour
{
    [SerializeField] private float _distanceFromOtherPerson;
    [Range(0,360)][SerializeField] private float _viewAngle;
    [SerializeField] private float _paymentDuration;
    
    private GameObject _target;
    private NavMeshAgent _navMeshAgent;
    private Person _person;
    private List<Person> _otherPeopleList;
    
    private Vector3 _directionToTarget;
    private Vector3 _lookingDirection;
    private Vector3 _velocity;
    
    private ItemOnList _itemsToBuy;
    private CashRegister _assignedCashRegister;
    private float _paymentTime;
    
    private FiniteStateMachine<Customer> _stateMachine;

    private const int NoDetection = -1;

    /* EVENT FUNCTION-------------------------------------------------------------------------------------------------*/
    void Start ()
    {
        _person = GetComponent<Person>();
        _otherPeopleList = GetListOfOtherPeople();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _directionToTarget = new Vector3();
        _lookingDirection = new Vector3();
        
        _velocity = _navMeshAgent.velocity;
        ResetVariables();
        
        
        //_stateMachine = new FiniteStateMachine<Customer>(this, _person.Username, true); //Debug Mode
        _stateMachine = new FiniteStateMachine<Customer>(this);

        //States
        State beginningState = new BeginningState("Entering to the supermarket", this);
        State movingToTargetItemState = new MovingToTargetItemState("Moving towards an item in the list", this);
        State targetItemReachedState = new TargetItemReachedState("Reached the item in the list", this);
        State stopState = new StopState("Stop", this);
        State movingToCashRegisterState = new MovingToCashRegisterState("Moving towards the cash register", this);
        State queuingState = new QueuingState("Queuing", this);
        State paymentState = new PaymentState("Paying", this);
        State exitState = new MovingToExit("Exit from the supermarket", this);
        
        //Transitions
        _stateMachine.AddTransition(beginningState, movingToTargetItemState, IsTargetReached);
        _stateMachine.AddTransition(beginningState, stopState, () => !_person.IsMoving);
        
        _stateMachine.AddTransition(movingToTargetItemState, movingToCashRegisterState, () => _person.ShoppingList.IsShoppingFinished());
        _stateMachine.AddTransition(movingToTargetItemState, targetItemReachedState, IsTargetReached);
        _stateMachine.AddTransition(movingToTargetItemState, stopState, () => !_person.IsMoving);
        
        _stateMachine.AddTransition(targetItemReachedState, movingToCashRegisterState, () => _person.ShoppingList.IsShoppingFinished());
        _stateMachine.AddTransition(targetItemReachedState, movingToTargetItemState, () => IsLooking(_lookingDirection));
        
        _stateMachine.AddTransition(movingToCashRegisterState, queuingState, IsTargetReached);
        
        _stateMachine.AddTransition(queuingState, paymentState, () => !_assignedCashRegister.IsMovingInQueue(GetInstanceID()) && _assignedCashRegister.IsFirst(GetInstanceID()));

        _stateMachine.AddTransition(paymentState, exitState, () => (_paymentDuration - _paymentTime) < 0);
       
        _stateMachine.AddTransition(exitState, beginningState, IsTargetReached);
        
        _stateMachine.AddTransition(stopState, beginningState, () => _person.IsMoving && _stateMachine.PrevState.Equals(beginningState));
        _stateMachine.AddTransition(stopState, movingToTargetItemState, () => _person.IsMoving && _stateMachine.PrevState.Equals(movingToTargetItemState));
        _stateMachine.AddTransition(stopState, movingToCashRegisterState, () => _person.IsMoving && _stateMachine.PrevState.Equals(movingToCashRegisterState));

        //Initial State
        _stateMachine.SetState(beginningState);
    }
	
    void Update() => _stateMachine.Tik();

    
    /* PUBLIC METHODS-------------------------------------------------------------------------------------------------*/

    public GameObject Target { set => _target = value; }
    
    public void StopAgent(bool stop)
    {
        _navMeshAgent.isStopped = stop;
        _navMeshAgent.velocity = stop ? Vector3.zero : _velocity;
    }

    public void Respawn()
    {
        var respawnPos = GameObject.FindWithTag("Respawn").transform.position;
        _person.ShoppingList = new ShoppingList();
        ResetVariables();
        
        _navMeshAgent.enabled = false;
        transform.position = new Vector3(respawnPos.x, transform.position.y, respawnPos.z);
        _navMeshAgent.enabled = true;
    }
    
    public void MoveToSupermarketEntrance() 
    {
        _target = GameObject.FindWithTag("Entrance").gameObject;
        if (_target != null)
            _navMeshAgent.SetDestination(_target.transform.position);
    }

    public void MoveToItemInList()
    {
        _target = GetNewItemTarget();
        if (_target != null)
            _navMeshAgent.SetDestination(_target.transform.position);
    }

    public void MoveToCashRegisterQueuePosition()
    {
        _target = _assignedCashRegister.GetPositionDestination(GetInstanceID());
        _navMeshAgent.SetDestination(_target.transform.position);
    }
    
    public void ScrollQueuePosition()
    {
        _target = _assignedCashRegister.GetPositionDestination(GetInstanceID());
        _navMeshAgent.SetDestination(_target.transform.position);
        if(IsTargetReached()) _assignedCashRegister.PositionReached(GetInstanceID());
    }

    public void CheckQueueDestinationState()
    {
        if (!_target.Equals(_assignedCashRegister.GetPositionDestination(GetInstanceID())))
            MoveToCashRegisterQueuePosition();
    }

    public void AddCustomerToQueue()
    {
        _assignedCashRegister.AddPersonOnList(GetInstanceID(), _target);
    }

    public void LookAtTarget()
    {
        _lookingDirection = Vector3.RotateTowards(transform.forward, _directionToTarget, 2.5f * Time.deltaTime, 0f);
        transform.rotation = Quaternion.LookRotation(_lookingDirection);
    }
    
    public void GrabItems()
    {
        if (_itemsToBuy == null) return;
        while (!_itemsToBuy.IsAllTaken)
        {
            var itemComponent = _target.GetComponentInChildren<Item>();
            if (itemComponent != null && !itemComponent.IsInsideCart) //Nel caso non ci siano oggetti a sufficienza, semplicemente non li prende, e continua
            {
                var item = itemComponent.gameObject;
                item.SetActive(false);
            }
            _itemsToBuy.takeOne();
        }
        _itemsToBuy = null;
    }

    public void CheckPersonObstacle()
    {
        foreach (var otherPerson in _otherPeopleList.Where(p => p.IsMoving))
        {
            var directionToPerson = otherPerson.transform.position - transform.position;
            if (DetectingPersonObstacle(directionToPerson))
            {
                _person.IsMoving = false;
                return;
            }
        }
        if(!_person.IsMoving) 
            _person.IsMoving = true;
    }
    
    public void SetDirectionToTarget()
    {
        SetDirectionToGameObject(_target);
    }
    
    public void SetDirectionToCashRegister()
    {
        SetDirectionToGameObject(_assignedCashRegister.gameObject);
    }
    
    public void AssignCashRegister()
    {
        var cashRegisters = GameObject.FindWithTag("CashRegistersPoint").GetComponentsInChildren<CashRegister>();
        var minQueueCashRegister = cashRegisters[0];
        foreach (var cashRegister in cashRegisters)
        {
            if (cashRegister.PeopleInQueueAndPending() < minQueueCashRegister.PeopleInQueueAndPending())
                minQueueCashRegister = cashRegister;
        }
        _assignedCashRegister = minQueueCashRegister;
        _assignedCashRegister.PendingPerson.Add(GetInstanceID());
    }

    public void Paying()
    {
        _paymentTime += Time.deltaTime;
    }
    
    public void LeaveTheQueue()
    {
        _assignedCashRegister.ScrollList();
    }
    
    public void MoveToSupermarketExit()
    {
        _target = GameObject.FindWithTag("Exit").gameObject;
        if (_target != null)
            _navMeshAgent.SetDestination(_target.transform.position);
    }
    
    /* PRIVATE METHODS------------------------------------------------------------------------------------------------*/
    private GameObject GetNewItemTarget()
    {
        if (_person.ShoppingList.IsShoppingFinished()) return null;
        
        _itemsToBuy = _person.ShoppingList.ItemsToTake()[0];
        var transforms = _itemsToBuy.Item.GetComponentsInParent<Transform>();
        Debug.Log("name" + _person.Username + "   " + transforms[0].gameObject.name);
        return transforms[0].gameObject; //[1] indicate the ItemPoint of the item 
    }

    private bool IsTargetReached()
    {
        if (_target != null && !_navMeshAgent.pathPending)
            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude <= 0f)
                    return true;
        return false;
    }

    private bool IsLooking(Vector3 vector)
    {
        return (vector.x.ToString("0.00") == "0,00" && vector.y.ToString("0.00") == "0,00" && 
                (vector.z.ToString("0.00") == "1,00" || vector.z.ToString("0.00") == "-1,00")) ||
               (vector.y.ToString("0.00") == "0,00" && vector.z.ToString("0.00") == "0,00" && 
                (vector.x.ToString("0.00") == "1,00" || vector.x.ToString("0.00") == "-1,00")) ||
               (vector.z.ToString("0.00") == "0,00" && vector.x.ToString("0.00") == "0,00" && 
                (vector.y.ToString("0.00") == "1,00" || vector.y.ToString("0.00") == "-1,00"));
    }

    private void SetDirectionToGameObject(GameObject directionGameObject)
    {
        if (directionGameObject != null)
        {
            _directionToTarget = directionGameObject.transform.position - transform.position;
            _directionToTarget.y = 0f;
            _directionToTarget.Normalize();
        }
    }
    
    private void ResetVariables()
    {
        _assignedCashRegister = null;
        _paymentTime = 0.0f;
    }
    
    private bool DetectingPersonObstacle(Vector3 directionToTarget)
    {
        //CHECK IF IS WITHIN VIEW DISTANCE
        float squareTargetDistance = (directionToTarget).sqrMagnitude;
        if (!(squareTargetDistance <= _distanceFromOtherPerson * _distanceFromOtherPerson)) return false;
        
        //CHECK IF FALLS WITHIN VIEW ANGLE
        var angleToTarget = Vector3.Angle(transform.forward, directionToTarget.normalized);
        if (!(angleToTarget < _viewAngle * 0.5f)) return false;
        
        //CHECK IF THERE ARE NO OBSTACLES
        var ray = new Ray(transform.position, directionToTarget.normalized);
        if (!Physics.Raycast(ray, out var hit, _distanceFromOtherPerson)) return false;
        var obstacle = hit.transform.GetComponent<Person>();
        return (obstacle != null);
    }

    private List<Person> GetListOfOtherPeople()
    {
        return FindObjectsOfType<Person>().Where(person => person != GetComponent<Person>()).ToList();
    }
    
}



/* STATES-------------------------------------------------------------------------------------------------------------*/

public class BeginningState : State
{
    private readonly Customer _npc;
    public BeginningState(string name, Customer npc) : base(name)
    {
        _npc = npc;
    }

    public override void Enter()
    {
        _npc.StopAgent(false);
        _npc.MoveToSupermarketEntrance();
    }

    public override void Tik()
    {
        _npc.CheckPersonObstacle();
    }

    public override void Exit()
    {
        _npc.Target = null;
    }
}



public class MovingToTargetItemState : State
{
    private readonly Customer _npc;
    public MovingToTargetItemState(string name, Customer npc) : base(name)
    {
        _npc = npc;
    }

    public override void Enter()
    {
        _npc.StopAgent(false);
        _npc.MoveToItemInList();
    }

    public override void Tik()
    {
        _npc.CheckPersonObstacle();
    }

    public override void Exit(){}
}



public class TargetItemReachedState : State
{
    private readonly Customer _npc;
    public TargetItemReachedState(string name, Customer npc) : base(name)
    {
        _npc = npc;
    }

    public override void Enter()
    {
        _npc.SetDirectionToTarget();
        _npc.StopAgent(true);
    }

    public override void Tik()
    {
        _npc.LookAtTarget();
    }

    public override void Exit()
    {
        _npc.GrabItems();
        _npc.Target = null;
    }
}



public class StopState : State
{
    private readonly Customer _npc;
    public StopState(string name, Customer npc) : base(name)
    {
        _npc = npc;
    }

    public override void Enter()
    {
        _npc.StopAgent(true);
    }

    public override void Tik()
    {
        _npc.CheckPersonObstacle();
    }

    public override void Exit()
    {
    }
}



public class MovingToCashRegisterState : State
{
    private readonly Customer _npc;
    public MovingToCashRegisterState(string name, Customer npc) : base(name)
    {
        _npc = npc;
    }

    public override void Enter()
    {
        _npc.StopAgent(false);
        _npc.AssignCashRegister();
        _npc.MoveToCashRegisterQueuePosition();
    }

    public override void Tik()
    {
        _npc.CheckQueueDestinationState();
    }

    public override void Exit()
    {
    }
}



public class QueuingState : State
{
    private readonly Customer _npc;
    public QueuingState(string name, Customer npc) : base(name)
    {
        _npc = npc;
    }

    public override void Enter()
    {
        _npc.AddCustomerToQueue();
    }

    public override void Tik()
    {
        _npc.ScrollQueuePosition();
    }

    public override void Exit()
    {
    }
}



public class PaymentState : State
{
    private readonly Customer _npc;
    public PaymentState(string name, Customer npc) : base(name)
    {
        _npc = npc;
    }

    public override void Enter()
    {
        _npc.StopAgent(true);
        _npc.SetDirectionToCashRegister();
    }

    public override void Tik()
    {
        _npc.Paying();
        _npc.LookAtTarget();
    }

    public override void Exit()
    {
        _npc.LeaveTheQueue();
        _npc.Target = null;
    }
}



public class MovingToExit : State
{
    private readonly Customer _npc;
    public MovingToExit(string name, Customer npc) : base(name)
    {
        _npc = npc;
    }

    public override void Enter()
    {
        _npc.StopAgent(false);
        _npc.MoveToSupermarketExit();
    }

    public override void Tik()
    {
    }

    public override void Exit()
    {
        _npc.Respawn();
    }
}