using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fridge : SupermarketContainer
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    public override float GetLength()
    {
        return this.transform.GetChild(1).GetChild(0).GetComponent<BoxCollider>().size.x;
    }

    protected override void FillRack()
    {
        if (base.obj_ == null || base.isFill )
            return;

        var shelves = this.transform.GetChild(1);
        Vector3 rotation = this.transform.rotation.eulerAngles;
        for (int i = 0; i < shelves.childCount; i++)
        {
            var shelf = shelves.GetChild(i);
            int n = Mathf.FloorToInt(shelf.GetComponent<BoxCollider>().size.x 
                                     / ((obj_.GetComponent<CapsuleCollider>().radius + base.distance_) * 2));
            for (int j = 0; j < n; j++)
            {
                Vector3 start_pos = shelf.position;

                //First row
                var bottle = Instantiate<GameObject>(obj_);
                Vector3 pos = start_pos + new Vector3(((obj_.GetComponent<CapsuleCollider>().radius + base.distance_) * 2 * j), 0, 0);
                pos.x -= shelf.GetComponent<BoxCollider>().size.x / 2 - base.obj_.GetComponent<CapsuleCollider>().radius - base.distance_;
                pos.z -= shelf.GetComponent<BoxCollider>().size.z / 4;
                bottle.transform.position = pos;
                bottle.transform.RotateAround(start_pos, new Vector3(0, 1, 0), rotation.y);

                //Second row
                bottle = Instantiate<GameObject>(obj_);
                pos.z -= shelf.GetComponent<BoxCollider>().size.z / 2;
                bottle.transform.position = pos;
                bottle.transform.RotateAround(start_pos, new Vector3(0, 1, 0), rotation.y);
            }
        }

        base.isFill = true;
    }
}
