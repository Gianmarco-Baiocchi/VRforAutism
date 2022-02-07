using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class SupermarketContainer : MonoBehaviour, ISupermarketContainer
{
    [SerializeField] protected GameObject obj_;
    [SerializeField] protected float distance_ = 0;
    protected bool isFill = false;

    public virtual void SetObject(GameObject obj)
    {
        if (this.isFill) //Shelves are already fill
            return;

        this.obj_ = obj;
        if (this.obj_ != null)
        {
            this.distance_ = obj_.GetComponent<CapsuleCollider>().radius;
            this.FillRack();
        }
    }

    public abstract float GetLength();

    protected void Start()
    {
        if (this.obj_ != null && !this.isFill)
            this.FillRack();
    }

    protected void Update() { }

    protected abstract void FillRack();
}
