using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CashRegister : MonoBehaviour
{
    private class QueuePosition
    {
        public GameObject QueuePoint { get; }
        public int PersonId { get; set; }
        public bool IsMoving { get; set; }
        public bool IsPlayer { get; set; }
        public QueuePosition(GameObject queuePoint, int id = -1, bool isMoving = false, bool isPlayer = false)
        {
            QueuePoint = queuePoint;
            PersonId = id;
            IsMoving = false;
            IsPlayer = isPlayer;
        }
    }
    
    
    [SerializeField] private GameObject queuePoint;
    private List<QueuePosition> _queuePositions;

    private const int FreePosition = -1;

    public List<int> PendingPerson { get; private set; }

    public void Start()
    {
        _queuePositions = new List<QueuePosition>(){new QueuePosition(queuePoint)};
        PendingPerson = new List<int>();
    }


    /* PUBLIC METHODS-------------------------------------------------------------------------------------------------*/

    /*Funzione che aggiunge un utente in una data posizione in coda, e crea, nel caso in cui sia l'ultimo,
     una nuova posizione vuota dietro di lui*/
    public void AddPersonOnList(int personInstanceId, GameObject point, bool isUserPlayer = false)
    {
        var queuePosition = _queuePositions.First(position => position.QueuePoint.Equals(point));
        if (queuePosition.PersonId == FreePosition) //se la posizione non è occupata da una persona
        {
            if(!_queuePositions.Any(position => position.PersonId.Equals(personInstanceId)))  //se si tratta di un nuovo membro in lista
            {
                var position = _queuePositions.Last().QueuePoint.transform.position;
                var newQueuePoint = Instantiate(_queuePositions.Last().QueuePoint, new Vector3(position.x - 3f, position.y, position.z), Quaternion.identity);
                _queuePositions.Add(new QueuePosition(newQueuePoint));
                PendingPerson.Remove(personInstanceId);
            }
            queuePosition.PersonId = personInstanceId;
            queuePosition.IsPlayer = isUserPlayer;
        }
        Debug.Log("ADD-> " + QueuePositionDebug());
    }

    /*Funzione che aggiunge un utente nell'ultima posizone della lista, e crea una nuova posizione vuota dietro di lui*/
    public void AddPersonAtBottom(int personInstanceID, bool isPlayer = false)
    {
        AddPersonOnList(personInstanceID, _queuePositions.Last().QueuePoint, isPlayer);
    }

    public void RemovePlayerAndScroll()
    {
        var index = GetPlayerIdInQueue();
        if (index == -1) return;
        _queuePositions[index].IsPlayer = false;
        _queuePositions[index].PersonId = FreePosition;
        if (_queuePositions[PositionInQueue() - 1].PersonId != FreePosition) //caso dove non ho una catena di posizione libere fino alla fine, come (A, B, -1, -1, C, -1), dove la posizione prima di C era la posizione che occupava il giocatore
        {
            index = _queuePositions.IndexOf(GetFirstFreeQueuePosition());
            var stepLength = GetNumberConsecutiveFreePosition();
            Debug.Log("rimozione! " + index + "  " + (PositionInQueue() - (index +stepLength)) + "  " + stepLength);
            ScrollListFromIndex(index, PositionInQueue() -(index + stepLength), stepLength);
            //Adesso ho (A, B, C, -1, -1, -1)
        }
        for (int i = PositionInQueue(); i > _queuePositions.IndexOf(GetFirstFreeQueuePosition()); i--) //vado a rimuovere tutte le posizione libere, tranne la prima. Es (A, B, C, -1)
        {
            Destroy(_queuePositions[i].QueuePoint);
            _queuePositions.RemoveAt(i);
        }
    }

    public void ScrollList()
    {
        if (IsPlayerInQueue())
        {
            ScrollListFromIndex(0, _queuePositions.IndexOf(GetFirstFreeQueuePosition()) < GetPlayerIdInQueue() - 1 ? _queuePositions.IndexOf(GetFirstFreeQueuePosition()) : GetPlayerIdInQueue() - 1);
        }
        else
        {
            ScrollListFromIndex(0, PositionInQueue());
        }
    }

    /*Funzione che ritorna il numero di persone in fila sommato a quelle che la stanno raggiungendo*/
    public int PeopleInQueueAndPending()
    {
        return PendingPerson.Count + _queuePositions.Count(position => position.PersonId != FreePosition);
    }
    
    public GameObject GetPositionDestination(int personInstanceId) //TODO check
    {
        if(!_queuePositions.Any(position => position.PersonId.Equals(personInstanceId)))  //se si tratta di un nuovo membro in lista
        {
            return _queuePositions.Last().QueuePoint;
        }
        var index = GetPositionOnList(personInstanceId);
        return _queuePositions[index].QueuePoint;
    }

    public bool IsFirst(int personInstanceId)
    {
        return GetPositionOnList(personInstanceId) == 0;
    }
    
    public void PositionReached(int personInstanceId)
    {
        GetQueuePositionByPersonId(personInstanceId).IsMoving = false;
    }

    public bool IsMovingInQueue(int personInstanceId)
    {
        return GetPositionOnList(personInstanceId) != -1 && GetQueuePositionByPersonId(personInstanceId).IsMoving;
    }

    public bool ContainQueuePoint(GameObject point)
    {
        return _queuePositions.FindIndex(position => position.QueuePoint == point) != -1;
    }
    
    /*Funzione che torna lo stato della coda*/
    public string QueuePositionDebug()
    {
        string t = "";
        _queuePositions.ForEach(position => t += "(ID " + position.PersonId + ",  INDEX " + _queuePositions.IndexOf(position) + ",  IS_M " + position.IsMoving + ",  IS_P " + position.IsPlayer + ") -- ");
        return t;
    }
    

    
    /* PRIVATE METHODS------------------------------------------------------------------------------------------------*/
    private int GetPositionOnList(int personInstanceId)
    {
        return _queuePositions.IndexOf(GetQueuePositionByPersonId(personInstanceId));
    }
    
    /*Funzione che ritorna il numero di posizioni nella fila, non considerando l'ultima, che è sempre vuota*/
    private int PositionInQueue()
    {
        return _queuePositions.Count - 1;
    }

    private QueuePosition GetQueuePositionByPersonId(int personInstanceId)
    {
        return _queuePositions.Find(position => position.PersonId.Equals(personInstanceId));
    }
    
    /* Funzione che ritorna la prima posizione libera nella fila. E' utile quando ho il giocatore in fila e devono scorrere
     le persone davanti a lui*/
    private QueuePosition GetFirstFreeQueuePosition()
    {
        return _queuePositions.First(position => position.PersonId.Equals(-1));
    }
    
    private void ScrollListFromIndex(int startIndex, int step, int stepLength = 1)
    {
        int j;
        var endIndex = startIndex + step;
        if (startIndex == step)
        {
            _queuePositions[startIndex].PersonId = FreePosition;
            return;
        }
        for (j = startIndex; j < endIndex; j++)
        {
            _queuePositions[j].PersonId = _queuePositions[j + stepLength].PersonId;
            if(stepLength > 1)
                _queuePositions[j + stepLength].PersonId = FreePosition;
            _queuePositions[j].IsMoving = true;
        }
        if (PositionInQueue() == endIndex && !IsPlayerInQueue())
        {
            Destroy(_queuePositions[j].QueuePoint); //Rimuovo l'ultima posizione in lista
            _queuePositions.RemoveAt(j);
        }
        else
        {
            _queuePositions[j].PersonId = FreePosition;
        }
        Debug.Log("Scroll (" + startIndex+"" + endIndex +") -> " +QueuePositionDebug());
    }

    /*Funzione che ritorna true se tra le persone in coda c'è anche il giocatore*/
    private bool IsPlayerInQueue()
    {
        return _queuePositions.Find(position => position.IsPlayer == true) != null;
    }
    
    /*Funzione che ritorna, se è in coda, l'id del giocatore*/
    private int GetPlayerIdInQueue()
    {
        return IsPlayerInQueue() ? GetPositionOnList(_queuePositions.Find(position => position.IsPlayer == true).PersonId) : -1;
    }
    
    /*Funzione che la lunghezza delle posizioni libere consecutive*/
    private int GetNumberConsecutiveFreePosition()
    {
        var n = 0;
        foreach (var position in _queuePositions)
        {
            if (position.PersonId == FreePosition)
                n++;
            else if (n != 0)
                return n; //esempio situazione (A, -1, -1, -1, B, B, -1) dove l'ultimo -1 è la posizione che occupava il giocatore

        }
        return n;
    }

}
