using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    private bool gameIsPaused;
    private GameObject currentOpenMenu;
    private Controls controls;

    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject IngameMenu;
    //[SerializeField] private GameObject LevelSelection;
    //[SerializeField] private GameObject Settings;
    //[SerializeField] private GameObject Credits;

    private void Awake()
    {
        controls = Keybindinputmanager.inputActions;
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            MainMenu.SetActive(true);
        }


    }
    private void OnEnable()
    {
        controls.Enable();
    }

    void Update()
    {
        if (controls.Menu.MenuEsc.WasPerformedThisFrame())
        {
            HandleMenu();
        }

    }
    public void HandleMenu()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (MainMenu.activeSelf == true) return;
            else CloseSelectedMenu(MainMenu);
        }
        else
        {
            if (IngameMenu.activeSelf == false)
            {
                if (gameIsPaused == false)
                {
                    PauseGame();
                    IngameMenu.SetActive(true);
                }
                else CloseSelectedMenu(IngameMenu);
            }
            else
            {
                IngameMenu.SetActive(false);
                EndPause();
            }
        }
    }

    public void OpenSelection(GameObject currentMenu)
    {
        {
            currentOpenMenu = currentMenu;
            currentMenu.SetActive(true);
            //LevelSelection.SetActive(currentMenu == LevelSelection);
            //Settings.SetActive(currentMenu == Settings);
            //Credits.SetActive(currentMenu == Credits);

            MainMenu.SetActive(false);
            IngameMenu.SetActive(false);

            AudioController.Instance.PlaySoundOneshot((int)AudioController.Sounds.menuButton);
        }
    }
    public void ResumeGame()
    {
        IngameMenu.SetActive(false);
        EndPause();
    }
    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void CloseSelectedMenu(GameObject mainMenu)
    {
        if (currentOpenMenu != null)
        {
            currentOpenMenu.SetActive(false);
            currentOpenMenu = null; // Clear previous menu after returning
            mainMenu.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No previous menu to return to. Going back to inGameMenu.");
            mainMenu.SetActive(true);
        }
        AudioController.Instance.PlaySoundOneshot((int)AudioController.Sounds.menuButton);
    }

    private void PauseGame()
    {
        gameIsPaused = true;
        Time.timeScale = 0;
    }
    private void EndPause()
    {
        gameIsPaused = false;
        Time.timeScale = 1;
    }
    //private void SelectMenu(GameObject menu)
    //{
    //    currentOpenMenu = currentMenu;
    //    LevelSelection.SetActive(menu == LevelSelection);
    //    Settings.SetActive(menu == Settings);
    //    Credits.SetActive(menu == Credits);
    //}
}
