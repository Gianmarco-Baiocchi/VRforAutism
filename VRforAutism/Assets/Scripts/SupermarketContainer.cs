using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class SupermarketContainer : MonoBehaviour, ISupermarketContainer
{
    protected List<Tuple<GameObject, Vector3>> _objectsList = new List<Tuple<GameObject, Vector3>>();
    [SerializeField] protected Vector3 _distance = new Vector3(0.03f, 0.01f, 0.03f);
    protected bool _isFill = false;
    private System.Random _random = new System.Random();

    public void SetDistanceObject(float lateralOffset, float frontOffset)
    {
        this._distance.x = lateralOffset;
        this._distance.z = frontOffset;
    }

    public virtual void SetObject(GameObject obj, int index)
    {
        if (this._objectsList.ElementAt(index) != null && index >= this.GetShelvesNumber()) //Shelves are already fill
            return;

        this._objectsList[index] = new Tuple<GameObject, Vector3>(obj, this.GetObjectSize(obj));
        this.FillRack();
    }

    public int SetObjects(List<GameObject> objectsList)
    {
        int i;
        for(i = 0; i < Mathf.Min(objectsList.Count, this.GetShelvesNumber()); i++)
        {
            var obj = objectsList.ElementAt<GameObject>(i);
            this._objectsList.Add(new Tuple<GameObject, Vector3>(obj, this.GetObjectSize(obj)));
        }
        return i;
    }

    public void Fill()
    {
        if (this._objectsList.Count != 0 && !this._isFill)
            this.FillRack();
    }

    public abstract int GetShelvesNumber();

    public abstract float GetLength();

    protected void Start()
    {
        if (this._objectsList.Count != 0 && !this._isFill)
            this.FillRack();
    }

    protected void Update() { }

    protected abstract void FillRack();
    
    protected Vector3 GetObjectSize(GameObject obj)
    {
        if (obj.GetComponent<CapsuleCollider>())
        {
            return new Vector3(obj.GetComponent<CapsuleCollider>().radius * 2 * obj.transform.localScale.x,
                               obj.GetComponent<CapsuleCollider>().height * obj.transform.localScale.y,
                               obj.GetComponent<CapsuleCollider>().radius * 2 * obj.transform.localScale.z);
        }
        else if (obj.GetComponent<BoxCollider>())
        {
            return new Vector3(obj.GetComponent<BoxCollider>().size.x * obj.transform.localScale.x,
                               obj.GetComponent<BoxCollider>().size.y * obj.transform.localScale.y,
                               obj.GetComponent<BoxCollider>().size.z * obj.transform.localScale.z);
        }
        //No capsule or box collider
        return Vector3.zero;
    }

    protected bool GenerateProbability(float probability)
    {
        return this._random.NextDouble() < probability;
    }

    protected float GenerateRotation(float max)
    {
        return (float)(this._random.Next(-1, 2) * this._random.NextDouble() * max);
    }

    protected float GenerateOffset(float max)
    {
        return (float)(this._random.Next(-1, 2) * this._random.NextDouble() * max);
    }
}
