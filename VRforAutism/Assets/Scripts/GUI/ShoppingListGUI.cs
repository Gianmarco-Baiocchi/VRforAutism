using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;
public class ShoppingListGUI : MonoBehaviour
{
    private ShoppingList _shoppingList;
    void Start()
    {
        _shoppingList = new ShoppingList();
        GetComponent<Renderer>().enabled = false;
        GetComponentInChildren<Canvas>().enabled = false;
    }
    public void WriteText()
    {
        //_shoppingList.ItemList.ForEach(itemOnList => { text += ("- " + itemOnList.Info) + "\n";});
        GetComponentsInChildren<TextMeshProUGUI>()[1].text = _shoppingList.ItemList.Aggregate("", (text, itemOnList) => text + ("- " + itemOnList.Info + "\n"));
    }
    
    public void SetState(bool state)
    {
        GetComponent<Renderer>().enabled = state;
        GetComponentInChildren<Canvas>().enabled = state;
    }
    
}