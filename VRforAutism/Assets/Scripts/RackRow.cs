using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RackRow : MonoBehaviour
{
    [SerializeField] private string _type;
    [SerializeField] private int _nRacks = 0;
    [SerializeField] private GameObject _rackPrefab;
    [SerializeField] private float _distance = 0.05f;
    [SerializeField] private float _lateralObjectDistance = 0.01f;
    [SerializeField] private float _frontObjectDistance = 0.01f;
    private List<GameObject> _objects;

    // Start is called before the first frame update
    void Awake()
    {
        this._objects = new List<GameObject>(Resources.LoadAll<GameObject>("Prefabs/Items/" + this._type + "/"));
        if (this._rackPrefab == null)
            this._rackPrefab = (GameObject) Resources.Load("Prefabs/SupermarketRack");
        this.createRacks();
    }

    // Update is called once per frame
    void Update() { }

    private void createRacks()
    {
        for(int i = 0; i < this._nRacks; i++)
        {
            var rack = Instantiate<GameObject>(this._rackPrefab);
            Vector3 pos = this.transform.position;
            if (i < this._nRacks / 2) //Left racks
                pos.x -= (rack.GetComponent<ISupermarketContainer>().GetLength() + this._distance) * (this._nRacks / 2 - i);
            else //Right racks
                pos.x += (rack.GetComponent<ISupermarketContainer>().GetLength() + this._distance) * (i - this._nRacks / 2);
            rack.transform.position = pos;
            rack.transform.RotateAround(this.transform.position, new Vector3(0, 1, 0), this.transform.rotation.eulerAngles.y);
            //int count = rack.GetComponent<ISupermarketContainer>().SetObjects(this._objects.ConvertAll(s => (GameObject)Resources.Load(s, typeof(GameObject))));
            int count = rack.GetComponent<ISupermarketContainer>().SetObjects(this._objects);
            this._objects.RemoveRange(0, count);
            rack.GetComponent<ISupermarketContainer>().SetDistanceObject(this._lateralObjectDistance, this._frontObjectDistance);
            rack.GetComponent<ISupermarketContainer>().Fill();
        }
    }
}
