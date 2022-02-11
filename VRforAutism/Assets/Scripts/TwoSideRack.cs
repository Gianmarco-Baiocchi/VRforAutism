using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TwoSideRack : SupermarketContainer
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

    public override int GetShelvesNumber()
    {
        return this.transform.GetChild(1).childCount * 2;
    }

    public override float GetLength()
    {
        return this.GetComponent<BoxCollider>().size.x;
    }

    protected override void FillRack()
    {
        if (base.objectsList_.Count == 0)
            return;

        var shelves = this.transform.GetChild(1);

        for (int i = 0; i < base.objectsList_.Count; i++)
        {
            var obj = base.objectsList_.ElementAt(i).Item1;
            var objectSize = base.objectsList_.ElementAt(i).Item2;
            if (objectSize == Vector3.zero)
                break;
            
            var shelf = shelves.GetChild(i % shelves.childCount);
            int n = Mathf.FloorToInt(shelf.GetComponent<BoxCollider>().size.x / (objectSize.x + base._distance.x * 2));
            int m = Mathf.FloorToInt(shelf.GetComponent<BoxCollider>().size.z / ((objectSize.z + base._distance.z * 2) * 2));
            Vector3 start_pos = shelf.position;
            Vector3 rotation = this.transform.rotation.eulerAngles;

            GameObject itemsPoint;
            if(i < shelves.childCount)
            {
                //Front shelf point
                itemsPoint = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/ItemsPointEmpty"),
                        start_pos + new Vector3(0, 0, shelf.GetComponent<BoxCollider>().size.z / 2), this.transform.rotation);
                itemsPoint.name = obj.name + "s";

            } 
            else
            {
                //Back shelf point
                itemsPoint = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/ItemsPointEmpty"),
                        start_pos - new Vector3(0, 0, shelf.GetComponent<BoxCollider>().size.z / 2), this.transform.rotation);
                itemsPoint.name = obj.name + "s";
            }

            for (int j = 0; j < n; j++)
            {
                for(int l = 0; l < m; l++)
                {
                    var bottle = Instantiate<GameObject>(obj);
                    Vector3 pos = start_pos;
                    if(i < shelves.childCount)
                    {
                        //Front shelf
                        pos += new Vector3(((objectSize.x + base._distance.x * 2) * j), 0, (objectSize.z + base._distance.z * 2) * l);
                        pos.z += objectSize.z + base._distance.z;
                    }
                    else
                    {
                        //Back shelf
                        pos += new Vector3(((objectSize.x + base._distance.x * 2) * j), 0, -(objectSize.z + base._distance.z * 2) * l);
                        pos.z -= objectSize.z + base._distance.z;
                    }
                    pos.x -= ((shelf.GetComponent<BoxCollider>().size.x / 2) - objectSize.x/2 - base._distance.x);
                    bottle.transform.position = pos;
                    bottle.transform.RotateAround(start_pos, new Vector3(0, 1, 0), rotation.y);
                    bottle.transform.SetParent(itemsPoint.transform);
                }
            }
        }
        base.isFill = true;
    }
}
