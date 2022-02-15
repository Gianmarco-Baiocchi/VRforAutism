using System;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

[RequireComponent (typeof(AudioSource))]

public class WinMenuGUI : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TextMeshProUGUI shoppingListText;
    [SerializeField] private TextMeshProUGUI extraItemsText;
    [SerializeField] private TextMeshProUGUI totalText;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private string menuScene;

    private Person _player;
    void Start()
    {
        _player = playerObject.GetComponent<Person>();
        SetState(false);
    }

    public void ShowMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        SetState(true);
        shoppingListText.text = ShoppingListDescription();
        extraItemsText.text = ExtraItemsDescription();
        totalText.text = TotalDescription();
    }
    
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(menuScene);
    }

    private void SetState(bool state)
    {
        if(state)
            GetComponent<AudioSource>().Play();
        winPanel.SetActive(state);
    }
    
    private string ShoppingListDescription()
    {
        return _player.ShoppingList.ItemList.Aggregate("", (text, itemOnList) => text + ("- " + itemOnList.Info + "\n"));
    }

    private string ExtraItemsDescription()
    {
        return _player.ExtraTakenItems == null ? "" : _player.ExtraTakenItems.Aggregate("", (current, pair) => current + ("- " + pair.Key.ItemName + " " + pair.Value + "\n"));
    }

    private string TotalDescription()
    {
        float total = 0.0f;
        Debug.Log("item:" + _player.ShoppingList.ItemList[0].Item.Price + "   " + _player.ShoppingList.ItemList[0].NTaken);
        _player.ShoppingList.ItemList.ForEach(item => total += item.Item.Price * item.NTaken);
        _player.ExtraTakenItems?.Keys.ToList().ForEach(k => total += k.Price * _player.ExtraTakenItems[k]);
        Debug.Log(total);
        return total + " $";
    }
}