using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuFunctions : MonoBehaviour
{
    public GameObject howToPlayMenu;

    public PlayerInventory playerInventory;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnContinueButtonClick() { 
        Time.timeScale = 1.0f;

        gameObject.SetActive(false);
    }

    public void OnBackToMenuButtonClick() {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
        playerInventory.EmptyAll();
    }

    public void OnHowToPlayButtonClick() { 
        howToPlayMenu.SetActive(true);
    }

    public void OnCloseButtonClick() {
        howToPlayMenu.SetActive(false);
    }
}
