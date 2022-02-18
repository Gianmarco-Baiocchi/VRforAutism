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

    public bool IsShoppingFinished()
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

    public void TakeNItem(Item item)
    {
        if (IsItemOnList(item))
            ItemInTheList(item).takeAll();
    }

    private List<ItemOnList> GenerateItemList()
    {
        var list = new List<ItemOnList>();
        
        //prendo tutti gli ItemsPoint dove il numero di prodotti non è zero
        var itemsPoints = GameObject.FindGameObjectsWithTag("ItemsPoint").Where(o => o.GetComponentInChildren<Item>() != null).ToList();
        
        var randIndexValues = new int[Random.Range(1, 6)];
        var i = 0;
        while (i < randIndexValues.Length)
        {
            var randIndex = Random.Range(1, itemsPoints.Count);
            if (!randIndexValues.Contains(randIndex))
            {
                randIndexValues[i] = randIndex;
                i++;
                list.Add(new ItemOnList(itemsPoints[randIndex].GetComponentInChildren<Item>(), nItem:Random.Range(1, 4)));
            }
        }
        return list; //TODO: se finiscono tutti gli oggetti all'interno del supermercato, il programma va in errore
    }

    private ItemOnList ItemInTheList(Item item)
    {
        return _itemList.Find(itemOnList => itemOnList.Item.ItemName.Equals(item.ItemName));
    }
    
    public bool IsItemOnList(Item item)
    {
        return ItemInTheList(item) != null;
    }
    
}