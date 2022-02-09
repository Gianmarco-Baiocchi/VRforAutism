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

        this.objectsList_[index] = new Tuple<GameObject, float>(obj, this.GetObjectSize(obj));
        this.FillRack();
    }

    public void SetObjects(List<GameObject> objectsList)
    {
        for(int i = 0; i < Mathf.Min(objectsList.Count, this.GetShelvesNumber()); i++)
        {
            var obj = objectsList.ElementAt<GameObject>(i);
            this.objectsList_.Add(new Tuple<GameObject, float>(obj, this.GetObjectSize(obj)));
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
    
    protected float GetObjectSize(GameObject obj)
    {
        if (obj.GetComponent<CapsuleCollider>())
        {
            return obj.GetComponent<CapsuleCollider>().radius * obj.transform.localScale.x;
        }
        else if (obj.GetComponent<BoxCollider>())
        {
            return obj.GetComponent<BoxCollider>().size.x * obj.transform.localScale.x;
        }
        //No capsule or box collider
        return 0;
    }
}
