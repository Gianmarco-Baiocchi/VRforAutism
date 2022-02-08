using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class SupermarketContainer : MonoBehaviour, ISupermarketContainer
{
    protected List<Tuple<GameObject, float>> objectsList_ = new List<Tuple<GameObject, float>>();
    //[SerializeField] protected GameObject obj_;
    //[SerializeField] protected float distance_ = 0;
    protected bool isFill = false;

    public virtual void SetObject(GameObject obj, int index)
    {
        if (this.objectsList_.ElementAt(index) != null && index >= this.GetShelvesNumber()) //Shelves are already fill
            return;

        this.objectsList_[index] = new Tuple<GameObject, float>(obj, obj.GetComponent<CapsuleCollider>().radius);
        this.FillRack();
    }

    public void SetObjects(List<GameObject> objectsList)
    {
        for(int i = 0; i < Mathf.Min(objectsList.Count, this.GetShelvesNumber()); i++)
        {
            var obj = objectsList.ElementAt<GameObject>(i);
            this.objectsList_.Add(new Tuple<GameObject, float>(obj, obj.GetComponent<CapsuleCollider>().radius * 
                Mathf.Max(obj.transform.localScale.x, obj.transform.localScale.z)));
        }
    }

    public abstract int GetShelvesNumber();

    public abstract float GetLength();

    protected void Start()
    {
        if (this.objectsList_.Count != 0 && !this.isFill)
            this.FillRack();
    }

    protected void Update() { }

    protected abstract void FillRack();
}
