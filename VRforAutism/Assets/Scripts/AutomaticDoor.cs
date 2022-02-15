using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDoor : MonoBehaviour
{
    private List<DoorLeaf> _doors = new List<DoorLeaf>();

    // Start is called before the first frame update
    void Start()
    {
        var doors = this.transform.GetComponentsInChildren<DoorLeaf>();
        this._doors.AddRange(doors);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        this._doors.ForEach(d => d.OpenEntrance());
    }

    private void OnTriggerExit(Collider other)
    {
        this._doors.ForEach(d => d.CloseEntrance());
    }
}
