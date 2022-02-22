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
        return this.GetComponent<BoxCollider>().size.x * this.transform.localScale.x;
    }

    protected override void FillRack()
    {
        if (base._objectsList.Count == 0)
            return;

        var shelves = this.transform.GetChild(1);

        for (int i = 0; i < base._objectsList.Count; i++)
        {
            var obj = base._objectsList.ElementAt(i).Item1;
            var objectSize = base._objectsList.ElementAt(i).Item2;
            if (objectSize == Vector3.zero)
                break;
            
            var shelf = shelves.GetChild(i % shelves.childCount);
            var shelf_size_x = shelf.GetComponent<BoxCollider>().size.x * this.transform.localScale.x;
            var shelf_size_z = shelf.GetComponent<BoxCollider>().size.z * this.transform.localScale.z;
            int n = Mathf.FloorToInt(shelf_size_x / (objectSize.x + base._distance.x * 2));
            int m = Mathf.FloorToInt( shelf_size_z / ((objectSize.z + base._distance.z * 2) * 2));
            Vector3 start_pos = shelf.position;
            Vector3 rotation = this.transform.rotation.eulerAngles;

            GameObject itemsPoint;
            if(i < shelves.childCount)
            {
                //Front shelf point
                itemsPoint = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Containers/ItemsPointEmpty"),
                        start_pos - new Vector3(0, 0, shelf.GetComponent<BoxCollider>().size.z / 2), this.transform.rotation);
                itemsPoint.name = obj.name + "s";

            } 
            else
            {
                //Back shelf point
                itemsPoint = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Containers/ItemsPointEmpty"),
                        start_pos + new Vector3(0, 0, shelf.GetComponent<BoxCollider>().size.z / 2), this.transform.rotation);
                itemsPoint.name = obj.name + "s";
            }

            for (int j = 0; j < n; j++)
            {
                for(int l = 0; l < m; l++)
                {
                    if (base.GenerateProbability(0.7f))
                    {
                        var item = Instantiate<GameObject>(obj);
                        Vector3 itemPos = start_pos;
                        if(i < shelves.childCount)
                        {
                            //Front shelf
                            itemPos += new Vector3(((objectSize.x + base._distance.x * 2) * j), 0, (objectSize.z + base._distance.z * 2) * l);
                            itemPos.z += objectSize.z + base._distance.z;
                        }
                        else
                        {
                            //Back shelf
                            itemPos += new Vector3(((objectSize.x + base._distance.x * 2) * j), 0, -(objectSize.z + base._distance.z * 2) * l);
                            itemPos.z -= objectSize.z + base._distance.z;
                        }
                        itemPos.x -= ((shelf_size_x / 2) - objectSize.x/2 - base._distance.x);
                        //Randomizing position and rotation
                        itemPos.x += base.GenerateOffset(base._distance.x / 2);
                        itemPos.z += base.GenerateOffset(base._distance.z / 3);
                        Vector3 itemRot = new Vector3(0, base.GenerateRotation(45f), 0);
                    
                        item.transform.position = itemPos;
                        item.transform.eulerAngles = itemRot;
                        item.transform.RotateAround(start_pos, new Vector3(0, 1, 0), rotation.y);
                        item.transform.SetParent(itemsPoint.transform);
                    }
                }
            }
        }
        base._isFill = true;
    }
}
