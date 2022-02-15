using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OneSideRack : SupermarketContainer
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
        return this.transform.GetComponentsInChildren<BoxCollider>().Select(e => e.size.x).Max();
    }

    protected override void FillRack()
    {
        if (base._objectsList.Count == 0 || base._isFill )
            return;

        var shelves = this.transform.GetChild(1);
        Vector3 rotation = this.transform.rotation.eulerAngles;

        for (int i = 0; i < base._objectsList.Count; i++)
        {
            var obj = base._objectsList.ElementAt(i).Item1;
            var objectSize = base._objectsList.ElementAt(i).Item2;
            if (objectSize == Vector3.zero)
                break;

            var shelf = shelves.GetChild(i);
            int n = Mathf.FloorToInt(shelf.GetComponent<BoxCollider>().size.x 
                                     / (objectSize.x + base._distance.x * 2));
            int m = Mathf.FloorToInt(shelf.GetComponent<BoxCollider>().size.z / (objectSize.z + base._distance.z * 2));
            Vector3 start_pos = shelf.position;

            var itemsPoint = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Containers/ItemsPointEmpty"));
            itemsPoint.transform.position = start_pos - new Vector3(0, 0, shelf.GetComponent<BoxCollider>().size.z);
            itemsPoint.transform.RotateAround(start_pos, new Vector3(0, 1, 0), rotation.y);
            itemsPoint.name = obj.name + "s";
            for (int j = 0; j < n; j++)
            {
                for(int l = 0; l < m; l++)
                {
                    var item = Instantiate<GameObject>(obj);
                    Vector3 pos = start_pos + new Vector3(((objectSize.x + base._distance.x * 2) * j), 0, -(objectSize.z + base._distance.z * 2) * l);
                    pos.x -= (shelf.GetComponent<BoxCollider>().size.x / 2) - (objectSize.x / 2) - base._distance.x;
                    pos.z -= (objectSize.z / 2) + base._distance.z;
                    item.transform.position = pos;
                    item.transform.RotateAround(start_pos, new Vector3(0, 1, 0), rotation.y);
                    item.transform.SetParent(itemsPoint.transform);
                }
            }
        
        }
        base._isFill = true;
    }
}
