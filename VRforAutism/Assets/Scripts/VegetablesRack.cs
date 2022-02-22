using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VegetablesRack : SupermarketContainer
{
    [SerializeField] private int maxObjectsNumber = 8;
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
            Vector3 start_pos = shelf.position;

            var itemsPoint = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Containers/ItemsPointEmpty"));
            itemsPoint.transform.position = start_pos - new Vector3(0, 0, shelf.GetComponent<BoxCollider>().size.z/2);
            itemsPoint.transform.RotateAround(start_pos, new Vector3(0, 1, 0), rotation.y);
            itemsPoint.name = obj.name + "s";
            for(int j = 0; j < this.ObjectsNumber((int) (shelf.GetComponent<BoxCollider>().size.x / objectSize.x * 2)); j++)
            {
                var item = Instantiate<GameObject>(obj);
                Vector3 itemPos = start_pos;
                itemPos.x += base.GenerateOffset(shelf.GetComponent<BoxCollider>().size.x / 4);
                itemPos.z += base.GenerateOffset(shelf.GetComponent<BoxCollider>().size.z / 4);
                Vector3 itemRot = new Vector3(0, base.GenerateRotation(45f), 0);

                item.transform.position = itemPos;
                item.transform.eulerAngles = itemRot;
                item.transform.RotateAround(start_pos, new Vector3(0, 1, 0), rotation.y);
                item.transform.SetParent(itemsPoint.transform);
            }
        }
        base._isFill = true;
    }

    private int ObjectsNumber(int nObjectRow)
    {
        var random = new System.Random();
        return new System.Random().Next(Mathf.Min(this.maxObjectsNumber, nObjectRow));
    }
}
