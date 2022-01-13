using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class ShoppingList
{

    private List<ItemOnList> _itemList;
    public List<ItemOnList> ItemList => _itemList;

    public ShoppingList()
    {
        _itemList = GenerateItemList();
    }

    public bool IsItemMustBeTaken(Item item)
    {
        ItemOnList itemOnList = ItemInTheList(item);
        return (itemOnList == null) ? false : !itemOnList.IsAllTaken;
    }
    
    public List<ItemOnList> ItemsToTake()
    {
        return _itemList.FindAll(item => !item.IsAllTaken);
    }

    public bool isShoppingFinished()
    {
        /*string text = ""; //DEBUG
        ItemsToTake().ForEach(list => text += list.Item.ItemName + " ");
        Debug.Log(ItemsToTake().Count + "  " + text);*/
        return ItemsToTake().Count == 0;
    }
    public void ItemTaken(Item item)
    {
        if (IsItemOnList(item))
            ItemInTheList(item).takeOne();
    }

    public void ItemRemoved(Item item)
    {
        if (IsItemOnList(item))
            ItemInTheList(item).removeOne();
    }

    private List<ItemOnList> GenerateItemList()
    {
        var list = new List<ItemOnList>();
        var itemsPoints = GameObject.FindGameObjectsWithTag("ItemsPoint");
        int[] randIndexValues = new int[Random.Range(1, itemsPoints.Length)];
        int i = 0;
        int randIndex;
        while (i < randIndexValues.Length)
        {
            randIndex = Random.Range(1, itemsPoints.Length);
            if (!randIndexValues.Contains(randIndex))
            {
                randIndexValues[i] = randIndex;
                i++;
                list.Add(new ItemOnList(itemsPoints[randIndex].GetComponentInChildren<Item>(), nItem:Random.Range(1, 4)));
            }
        }
        return list;
    }

    private ItemOnList ItemInTheList(Item item)
    {
        return _itemList.Find(itemOnList => itemOnList.Item.ItemName.Equals(item.ItemName));
    }
    
    private bool IsItemOnList(Item item)
    {
        return ItemInTheList(item) != null;
    }
    
}