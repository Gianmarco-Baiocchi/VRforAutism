using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rack : SupermarketContainer
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
        return this.GetComponent<BoxCollider>().size.x;
    }

    protected override void FillRack()
    {
        if (base.obj_ == null)
            return;

        var shelves = this.transform.GetChild(1);
        int n = Mathf.FloorToInt(this.GetComponents<BoxCollider>()[1].size.x 
                                 / ((base.obj_.GetComponent<CapsuleCollider>().radius + base.distance_) * 2));

        for (int i = 0; i < shelves.childCount; i++)
        {
            var shelf = shelves.GetChild(i);
            for (int j = 0; j < n; j++)
            {
                Vector3 start_pos = shelf.position;
                Vector3 rotation = this.transform.rotation.eulerAngles;
                //Front shelf
                //First row
                var bottle = Instantiate<GameObject>(obj_);
                Vector3 pos = start_pos + new Vector3(((base.obj_.GetComponent<CapsuleCollider>().radius + base.distance_) * 2 * j), 0, 0);
                pos.x -= (this.GetComponents<BoxCollider>()[1].size.x / 2 - base.obj_.GetComponent<CapsuleCollider>().radius - base.distance_);
                pos.z += this.GetComponents<BoxCollider>()[1].size.z / 8;
                bottle.transform.position = pos;
                bottle.transform.RotateAround(start_pos, new Vector3(0, 1, 0), rotation.y);
                //Second row
                bottle = Instantiate<GameObject>(obj_);
                pos.z += this.GetComponents<BoxCollider>()[1].size.z / 4;
                bottle.transform.position = pos;
                bottle.transform.RotateAround(start_pos, new Vector3(0, 1, 0), rotation.y);

                //Back shelf
                //First row
                bottle = Instantiate<GameObject>(obj_);
                pos.z = start_pos.z - this.GetComponents<BoxCollider>()[1].size.z / 8;
                bottle.transform.position = pos;
                bottle.transform.RotateAround(start_pos, new Vector3(0, 1, 0), rotation.y);
                //Second row
                bottle = Instantiate<GameObject>(obj_);
                pos.z -= this.GetComponents<BoxCollider>()[1].size.z / 4;
                bottle.transform.position = pos;
                bottle.transform.RotateAround(start_pos, new Vector3(0, 1, 0), rotation.y);
            }
        }
        base.isFill = true;
    }
}
