using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance;

    private Controls controls;
    private GameObject currentOpenMenu;
    [NonSerialized] public bool gameIsPaused;
    private bool levelIsFinished;

    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject IngameMenu;
    [SerializeField] private GameObject ResultScreen;

    [SerializeField] private GameObject levelSelectionBackButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            MainMenu.SetActive(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    private void Start()
    {
        controls = Keybindinputmanager.inputActions;
        controls.Enable();
    }

    void Update()
    {
        if (controls.Menu.MenuEsc.WasPerformedThisFrame())
        {
            if (levelIsFinished) return;
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
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
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
            ResultScreen.SetActive(false);

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
        AudioController.Instance.PlaySoundOneshot((int)AudioController.Sounds.menuButton);

        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void BackToMainMenu()
    {
        AudioController.Instance.PlaySoundOneshot((int)AudioController.Sounds.menuButton);

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

        AudioController.Instance.PlaySoundOneshot((int)AudioController.Sounds.menuButton);
    }
    private void EndPause()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameIsPaused = false;
        Time.timeScale = 1;

        AudioController.Instance.PlaySoundOneshot((int)AudioController.Sounds.menuButton);
    }

    public void OnLevelComplete(float finalTime)
    {
        levelSelectionBackButton.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameIsPaused = true;
        levelIsFinished = true;
        Time.timeScale = 0;

        ResultScreen.SetActive(true);
        ResultScreen.GetComponent<ResultScreen>().SetGameoverScreen(finalTime);
    }
}
