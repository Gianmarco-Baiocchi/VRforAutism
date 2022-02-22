using UnityEngine;
using UnityEngine.SceneManagement;


public class OptionMenu : MonoBehaviour
{
    [SerializeField] private Canvas optionMainMenu;
    [SerializeField] private Canvas commandsMenu;
    [SerializeField] private string gameScene;

    void Start()
    {
        GetComponent<Canvas>().enabled = false;
    }

    public void ShowOptionMenu()
    {
        GetComponent<Canvas>().enabled = true;
        ShowOptionMainMenu();
        Cursor.lockState = CursorLockMode.None;
        FindObjectOfType<FirstPersonCharacterController>().enabled = false;
    }

    public void ShowMainMenu()
    {
        SceneManager.LoadScene(gameScene);
    }
    
    public void ShowCommandsMenu()
    {
        optionMainMenu.enabled = false;
        commandsMenu.enabled = true;
    }
    
    public void ShowOptionMainMenu()
    {
        optionMainMenu.enabled = true;
        commandsMenu.enabled = false;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        GetComponent<Canvas>().enabled = false;
        FindObjectOfType<FirstPersonCharacterController>().enabled = true;
    }


}