using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    [SerializeField] private Canvas mainMenu;
    [SerializeField] private Canvas commandsMenu;
    [SerializeField] private string gameScene;

    void Start()
    {
        commandsMenu.enabled = false;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameScene);
    }
    
    public void ShowCommandsMenu()
    {
        mainMenu.enabled = false;
        commandsMenu.enabled = true;
    }
    
    public void ShowMainMenu()
    {
        mainMenu.enabled = true;
        commandsMenu.enabled = false;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }


}