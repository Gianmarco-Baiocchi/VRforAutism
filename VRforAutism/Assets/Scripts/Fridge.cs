using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public override int GetShelvesNumber()
    {
        return this.transform.GetChild(1).childCount;
    }

    public override float GetLength()
    {
        return this.transform.GetChild(1).GetChild(0).GetComponent<BoxCollider>().size.x;
    }

    protected override void FillRack()
    {
        if (base.objectsList_.Count == 0 || base.isFill )
            return;

        var shelves = this.transform.GetChild(1);
        Vector3 rotation = this.transform.rotation.eulerAngles;

        for (int i = 0; i < base.objectsList_.Count; i++)
        {
            var obj = base.objectsList_.ElementAt(i).Item1;
            var distance = base.objectsList_.ElementAt(i).Item2;
            var shelf = shelves.GetChild(i);
            var objectSize = obj.GetComponent<CapsuleCollider>().radius * obj.transform.localScale.x;
            int n = Mathf.FloorToInt(shelf.GetComponent<BoxCollider>().size.x 
                                     / ((objectSize + distance) * 2));
            Vector3 start_pos = shelf.position;
            //Items point
            var itemsPoint = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/ItemsPointEmpty"),
                    start_pos + new Vector3(0, 0, shelf.GetComponent<BoxCollider>().size.z), shelf.transform.rotation);
            itemsPoint.name = obj.name;
            for (int j = 0; j < n; j++)
            {

                //First row
                var item = Instantiate<GameObject>(obj);
                Vector3 pos = start_pos + new Vector3(((objectSize + distance) * 2 * j), 0, 0);
                pos.x -= shelf.GetComponent<BoxCollider>().size.x / 2 - objectSize - distance;
                pos.z -= shelf.GetComponent<BoxCollider>().size.z / 4;
                item.transform.position = pos;
                item.transform.RotateAround(start_pos, new Vector3(0, 1, 0), rotation.y);
                item.transform.SetParent(itemsPoint.transform);

                //Second row
                item = Instantiate<GameObject>(obj);
                pos.z -= shelf.GetComponent<BoxCollider>().size.z / 2;
                item.transform.position = pos;
                item.transform.RotateAround(start_pos, new Vector3(0, 1, 0), rotation.y);
                item.transform.SetParent(itemsPoint.transform);
            }
        }

        base.isFill = true;
    }
}
