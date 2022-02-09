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
        return this.transform.GetChild(1).childCount;
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
            if (objectSize == 0)
                break;
            
            var distance = base.objectsList_.ElementAt(i).Item2;
            var shelf = shelves.GetChild(i);
            int n = Mathf.FloorToInt(shelf.GetComponent<BoxCollider>().size.x / ((objectSize + distance) * 2));
            Vector3 start_pos = shelf.position;
            Vector3 rotation = this.transform.rotation.eulerAngles;

            //Front shelf point
            var frontItemsPoint = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/ItemsPointEmpty"),
                    start_pos + new Vector3(shelf.GetComponent<BoxCollider>().size.x / 2, 0, 0), this.transform.rotation);
            frontItemsPoint.name = obj.name;
            //Back shelf point
            var backItemsPoint = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/ItemsPointEmpty"),
                    start_pos - new Vector3(shelf.GetComponent<BoxCollider>().size.x / 2, 0, 0), this.transform.rotation);
            backItemsPoint.name = obj.name;

            for (int j = 0; j < n; j++)
            {
                //Front shelf
                //First row
                var bottle = Instantiate<GameObject>(obj);
                Vector3 pos = start_pos + new Vector3(((objectSize + distance) * 2 * j), 0, 0);
                pos.x -= (shelf.GetComponent<BoxCollider>().size.x / 2 - objectSize - distance);
                pos.z += shelf.GetComponent<BoxCollider>().size.z / 8;
                bottle.transform.position = pos;
                bottle.transform.RotateAround(start_pos, new Vector3(0, 1, 0), rotation.y);
                bottle.transform.SetParent(frontItemsPoint.transform);
                //Second row
                bottle = Instantiate<GameObject>(obj);
                pos.z += shelf.GetComponent<BoxCollider>().size.z / 4;
                bottle.transform.position = pos;
                bottle.transform.RotateAround(start_pos, new Vector3(0, 1, 0), rotation.y);
                bottle.transform.SetParent(frontItemsPoint.transform);


                //Back shelf
                //First row
                bottle = Instantiate<GameObject>(obj);
                pos.z = start_pos.z - shelf.GetComponent<BoxCollider>().size.z / 8;
                bottle.transform.position = pos;
                bottle.transform.RotateAround(start_pos, new Vector3(0, 1, 0), rotation.y);
                bottle.transform.SetParent(backItemsPoint.transform);
                //Second row
                bottle = Instantiate<GameObject>(obj);
                pos.z -= shelf.GetComponent<BoxCollider>().size.z / 4;
                bottle.transform.position = pos;
                bottle.transform.RotateAround(start_pos, new Vector3(0, 1, 0), rotation.y);
                bottle.transform.SetParent(backItemsPoint.transform);
            }
        }
        base.isFill = true;
    }
}
