using UnityEngine;
using System.Linq;
using TMPro;

public class ShoppingListGUI : MonoBehaviour
{
    void Start()
    {
        GetComponent<Renderer>().enabled = false;
        GetComponentInChildren<Canvas>().enabled = false;
    }
    public void WriteText()
    {
        var list = GameObject.FindWithTag("Player").GetComponent<Person>().ShoppingList;
        GetComponentsInChildren<TextMeshProUGUI>()[1].text = list.ItemList.Aggregate("", (text, itemOnList) => text + ("- " + itemOnList.Info + "\n"));
    }
    
    public void SetState(bool state)
    {
        GetComponent<Renderer>().enabled = state;
        GetComponentInChildren<Canvas>().enabled = state;
    }
    
}