using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour
{
    public GameObject howToPlayMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayButtonClick() {
        SceneManager.LoadScene("SampleScene");
        //empty out inventories
    }

    public void OnQuitButtonClick() { 
        Application.Quit();
    }

    public void OnHowToPlayButtonClick() {
        howToPlayMenu.SetActive(true);
    }

    public void OnXButtonClick() {
        howToPlayMenu.SetActive(false);
    }
}
