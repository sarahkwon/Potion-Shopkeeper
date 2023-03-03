using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    public GameObject potionInventoryMenu;
    public GameObject itemSaleMenu;
    public GameObject potionMakingMenu;
    public GameObject ingredientShopMenu;
    public GameObject improviseMenu;
    public GameObject recipeMenu;
    public GameObject pauseMenu;
    public GameObject loseMenu;

    public TextMeshProUGUI priceInputText;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI phaseText;
    public TextMeshProUGUI remainingDaysText;
    public TextMeshProUGUI currentDayText;

    public TextMeshProUGUI phase2TempText;

    public TextMeshProUGUI coinsText;

    public GameObject winText;

    [SerializeField] private CharacterMovement characterMovement;
    [SerializeField] private CinemachineFreeLook freeLook; //x = 450, y = 2

    public AudioSource fxAudioSource;
    public AudioSource tableAudioSource;
    public AudioSource customerAudioSource;
    public AudioClip openMenuSound;
    public AudioClip closeMenuSound;
    public AudioClip fred1;
    public AudioClip fred2;
    public AudioClip fred3;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Keyboard.current.iKey.wasPressedThisFrame && !itemSaleMenu.activeInHierarchy && !potionMakingMenu.activeInHierarchy)
        {
            TogglePotionInventory(!potionInventoryMenu.activeInHierarchy);
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame) {
            TogglePauseMenu(!pauseMenu.activeInHierarchy);
        }

    }

    public void TogglePauseMenu(bool value) {
        if (value == true) //open pause menu
        {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
        }
        else { //close pause menu
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
        }
    }


    public void TogglePotionInventory(bool value) {
        potionInventoryMenu.SetActive(value);

        PlayOpenCloseMenuSound(value);

        TogglePlayerMouseControl(!value);
    }

    public void ToggleItemSaleMenu(DisplayTable displayTable, bool value) {
        itemSaleMenu.SetActive(value);
        itemSaleMenu.GetComponent<ItemSaleUI>().TogglePotionSaleSection(displayTable, value); //in case i forget, potion sale section is the a game object holding the icon of the potion in the purple sale slot

        SetPlayerControl(value);



    }

    public void PlayOpenCloseMenuSound(bool value) {
        if (value == true)
        {
            fxAudioSource.clip = openMenuSound;
            fxAudioSource.loop = false;
            fxAudioSource.Play();
        }
        else {
            fxAudioSource.clip = closeMenuSound;
            fxAudioSource.loop = false;
            fxAudioSource.Play();
        }
    }

    public void ToggleIngredientShopMenu(bool value, bool closingAll) {
        ingredientShopMenu.SetActive(value);

        if (value == true)
        {
            PlayFredSound(0);
        }
        else if (value == false && closingAll == false){
            PlayFredSound(2);
        }
        
        SetPlayerControl(value);
    }

    public void PlayFredSound(int num) {
        if (num == 0)
        { //open shop
            fxAudioSource.clip = fred1;
            fxAudioSource.Play();
        }
        else if (num == 1) //purchase bundle
        {
            fxAudioSource.clip = fred2;
            fxAudioSource.Play();
        }
        else if (num == 2) { //close shop
            fxAudioSource.clip = fred3;
            fxAudioSource.Play();
        }


    }

    public void TogglePotionMakingMenu(bool value) {
        potionMakingMenu.SetActive(value);
        if (potionMakingMenu.GetComponent<PotionCraftingUI>().menuOpenFlag == false && value == true)
        {
            potionMakingMenu.GetComponent<PotionCraftingUI>().OnClickImproviseButton();
            fxAudioSource.clip = openMenuSound;
            fxAudioSource.loop = false;
            fxAudioSource.Play();
        }
        else if (potionMakingMenu.GetComponent<PotionCraftingUI>().menuOpenFlag == true && value == true) {
            potionMakingMenu.GetComponent<PotionCraftingUI>().OnClickRecipesButton();
        } else if (value == false) {
            TogglePotionInventory(false);
        }
        
        potionMakingMenu.GetComponent<PotionCraftingUI>().OpenUI();

        SetPlayerControl(value);

    }

    private void SetPlayerControl(bool value) {
        if (value == true)
        {
            characterMovement.playerCanControlCharacter = false;
            freeLook.m_YAxis.m_MaxSpeed = 0;
            freeLook.m_XAxis.m_MaxSpeed = 0;
        }
        else
        {
            characterMovement.playerCanControlCharacter = true;
            freeLook.m_YAxis.m_MaxSpeed = characterMovement.maxSens_y;
            freeLook.m_XAxis.m_MaxSpeed = characterMovement.maxSens_x;
        }
    }

    //true == enabled
    //false == disabled
    private void TogglePlayerMouseControl(bool value) {
        if (value == false)
        {
            freeLook.m_YAxis.m_MaxSpeed = 0;
            freeLook.m_XAxis.m_MaxSpeed = 0;
        }
        else if (!itemSaleMenu.activeInHierarchy && !potionMakingMenu.activeInHierarchy){ //only let the player move around camera IF no other menus are open
            freeLook.m_YAxis.m_MaxSpeed = characterMovement.maxSens_y;
            freeLook.m_XAxis.m_MaxSpeed = characterMovement.maxSens_x;
        }
    }

    public void DisplayTimer(float timeToDisplay) {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void DisplayCurrentPhase(int phase) {
        if (phase == 1)
        {
            phaseText.text = "Setting Up Shop!";
        }
        else {
            phaseText.text = "Shop is OPEN!";
        }
    }

    public void DisplayRemainingDaysText(int numDays) {
        if (numDays > 0)
        {
            remainingDaysText.text = numDays.ToString() + " Days Remaining";
        }
        else {
            remainingDaysText.text = "Final Day!!";
        }
        
    }

    public void DisplayCurrentDayText(int currentDay) {
        currentDayText.text = "Day: " + currentDay.ToString();

    }

    public void ToggleTimePhaseDayTexts(bool value) {
        timerText.gameObject.SetActive(value);
        phaseText.gameObject.SetActive(value); ;
        remainingDaysText.gameObject.SetActive(value); ;
        currentDayText.gameObject.SetActive(value); ;
    }

    public void TogglePhaseTwoTempText(bool value) {
        phase2TempText.gameObject.SetActive(value);
    }

    public void ToggleAllMenus(bool value) {
        TogglePotionInventory(value);
        itemSaleMenu.GetComponent<ItemSaleUI>().closeDisplayTableUI();
        TogglePotionMakingMenu(value);
        ToggleIngredientShopMenu(value, true);
    }

    public void UpdateCoinsText(int value) {
        string formattedText = value.ToString("n2");
        coinsText.text = "$" + formattedText;
    }

    public void DisplayWinText() {
        ToggleAllMenus(false);
        winText.SetActive(true);

        SetPlayerControl(true);
    }

    public void DisplayLoseMenu() {
        ToggleAllMenus(false);
        loseMenu.SetActive(true);

        SetPlayerControl(true);
    }


}
