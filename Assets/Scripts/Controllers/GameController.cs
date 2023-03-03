using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private float phaseOneDuration;
    private float currPhaseOneDuration = 0;
    [SerializeField] private float phaseTwoDuration;
    private float currPhaseTwoDuration = 0;
    private bool phaseOneTimerActive = false;
    private bool phaseTwoTimerActive = false;

    public float customerSpawnRate = 10f; //one customer every 10 seconds
    private float customerSpawnTimer;

    public int maxCustomers = 7;
    public int currCustomers = 0;

    [SerializeField] private int numberOfDays;
    private int currentDay = 1;

    [SerializeField] private Cauldron cauldron;
    //    [SerializeField] private CashRegister cashRegister;

    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private Transform playerPos;
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private TransactionManager transactionManager;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private PlayerBank playerBank;
    [SerializeField] private ReputationSystem repSystem;
    [SerializeField] private AudioSource audioSource;

    public AudioClip phaseOneBg;
    public AudioClip phaseTwoBg;

    public AudioClip customerEnterStoreSound;
    public AudioClip customerLeaveStoreSound;

    private List<DisplayTable> tables = new List<DisplayTable>();
    private List<Transform> customersActive = new List<Transform>();

    public List<IngredientItem> ingredients = new List<IngredientItem>();

    public UIController uiController;

    [SerializeField] private Item winCondition;

    private bool bundle1Purchased = false;
    private bool bundle2Purchased = false;
    private bool bundle3Purchased = false;

    public Button endPhaseButton;


    // Start is called before the first frame update
    void Start()
    {
        uiController = FindObjectOfType<UIController>();
        StartPhaseOne();
        uiController.DisplayRemainingDaysText(numberOfDays - currentDay);
        uiController.DisplayCurrentDayText(currentDay);

        foreach (DisplayTable obj in FindObjectsOfType<DisplayTable>()) {
            tables.Add(obj);
        }

        customerSpawnTimer = customerSpawnRate;

       
    }

    // Update is called once per frame
    void Update()
    {
        if (currentDay <= numberOfDays) // || player does not have final ingredient
        {
            uiController.TogglePhaseTwoTempText(false);
            if (phaseOneTimerActive)
            {
                if (currPhaseOneDuration > 0)
                {
                    currPhaseOneDuration -= Time.deltaTime;
                    uiController.DisplayTimer(currPhaseOneDuration);
                }
                else
                {
                    Debug.Log("Ran out of time.");
                    EndPhaseOne();
                    StartPhaseTwo();
                }

            }
            else if (phaseTwoTimerActive)
            {
                uiController.TogglePhaseTwoTempText(true);
                if (currPhaseTwoDuration > 0)
                {
                    currPhaseTwoDuration -= Time.deltaTime;
                    uiController.DisplayTimer(currPhaseTwoDuration);

                    //spawn customers at a controlled rate
                    if (currCustomers < maxCustomers && customerSpawnTimer <= 0f)
                    {
                        SpawnCustomer();
                        customerSpawnTimer = customerSpawnRate;
                    }
                    else {
                        customerSpawnTimer -= Time.deltaTime;
                    }

                    
                }
                else
                {
                    Debug.Log("Ran out of time.");
                    EndPhaseTwo();
                    currentDay += 1;
                    uiController.DisplayRemainingDaysText(numberOfDays - currentDay);
                    uiController.DisplayCurrentDayText(currentDay);
                    DestroyCustomers(); //destroy customers in the world
                    customerSpawnRate -= 1f; //each day, the customers spawn a second faster
                    if (customerSpawnRate < 5f) {
                        customerSpawnRate = 5f;
                    }

                    StartPhaseOne();
                    
                }

            }
        }
        else {
            Debug.Log("Game End.");
            uiController.ToggleTimePhaseDayTexts(false); //hide time texts
            uiController.TogglePhaseTwoTempText(false); //temporary
            LoseGame();
            //if player has enough money, give them one last chance to purchase the ingredient 
            //if player has ingredient, win
            //else, lose
        }

       
        
    }

    public void OnEndPhaseButtonClick() {
        currPhaseOneDuration = 0f;
    }

    public void StartPhaseOne() {
        Debug.Log("Phase 1 started");
        if (currentDay == 1) {
            RespawnPlayer();
        }
        phaseOneTimerActive = true;
        currPhaseOneDuration = phaseOneDuration;
        //disable cash register
        ToggleCashRegister(false);

        //enable cauldron
        ToggleCauldron(true);

        //display phase text for user
        uiController.DisplayCurrentPhase(1);

        //change music
        audioSource.clip = phaseOneBg;
        audioSource.Play();
        audioSource.loop = true;

        endPhaseButton.gameObject.SetActive(true);

    }

    public void EndPhaseOne() {
        Debug.Log("End Phase 1");
        phaseOneTimerActive = false;
        currPhaseOneDuration = 0;

        //close cauldron menu, if open
        if (uiController.potionMakingMenu.activeInHierarchy) {
            uiController.TogglePotionMakingMenu(false);
        }

        endPhaseButton.gameObject.SetActive(false);

    }

    public void StartPhaseTwo() {
        Debug.Log("Phase 2 started");
        currPhaseTwoDuration = phaseTwoDuration;
        phaseTwoTimerActive = true;

        //enable cash register
        ToggleCashRegister(true);

        //disable cauldron
        ToggleCauldron(false);

        // display phase text for user
        uiController.DisplayCurrentPhase(2);

        //change bg music
        audioSource.clip = phaseTwoBg;
        audioSource.Play();
        audioSource.loop = true;
    }

    public void EndPhaseTwo() {
        Debug.Log("End Phase 2");
        phaseTwoTimerActive = false;
        currPhaseTwoDuration = 0;

        //close all open menus
        uiController.ToggleAllMenus(false);

        //make sure all display tables are unlocked
        foreach (DisplayTable t in tables) {
            t.IsLocked = false;
        }
    }

    private void ToggleCauldron(bool value) {
        cauldron.isInteractable = value;
    }

    private void ToggleCashRegister(bool value) { 
        
    }

    private void RespawnPlayer() {
        playerPos.position = new Vector3(playerSpawnPoint.position.x, playerSpawnPoint.position.y, playerSpawnPoint.position.z);
    }

    private void ResetGame() {
        currentDay = 1;
        StartPhaseOne();

    }

    private void SpawnCustomer() {
        GameObject newCustomer = Instantiate(customerPrefab, spawnPoint.position, spawnPoint.rotation);
        CustomerMovement mov = newCustomer.GetComponent<CustomerMovement>();
        mov.spawnPoint = spawnPoint;
        mov.currWayPoint = 0;
        mov.gameController = this;

        mov.transactionManager = transactionManager;
        mov.repSystem = repSystem;

        newCustomer.GetComponent<AudioSource>().clip = customerEnterStoreSound;
        newCustomer.GetComponent<AudioSource>().Play();

        //shuffle the displaytable list and then add them one by one as waypoints
        Shuffle(tables);

        foreach (DisplayTable table in tables) {
            mov.wayPoints.Add(table.transform);
        }

        Customer cust = newCustomer.GetComponent<Customer>();
        cust.playerInventory = playerInventory;
        cust.playerBank = playerBank;

        customersActive.Add(newCustomer.transform);

        currCustomers += 1;


    }

    private void DestroyCustomers() {
        for (int i = 0; i < customersActive.Count; i++) {
            if (customersActive[i] != null) {
                transactionManager.customers.Remove(customersActive[i].GetComponent<Customer>());
                customersActive[i].gameObject.GetComponent<ReactionUI>().DestroyReactionUI();
                Destroy(customersActive[i].gameObject);
                
            } 
            
        }

        customerSpawnTimer = customerSpawnRate;

        currCustomers = 0;
    }

    public void UpdateSpawnRate(float delta) {
        customerSpawnRate += delta;

        if (customerSpawnRate < 5f) {
            customerSpawnRate = 5f;
        }

        if (customerSpawnRate > 15f) {
            customerSpawnRate = 15f;
        }

        if (customerSpawnRate >= 7f && customerSpawnRate <= 8f) {
            maxCustomers = 8;
        }

        if (customerSpawnRate >= 5f && customerSpawnRate <= 6f)
        {
            maxCustomers = 9;
        }




    }


    public void Shuffle(List<DisplayTable> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    public void PurchaseAndWin() {
        if (playerBank.Coins >= 10000) {
            WinGame();
            playerBank.RemoveCoins(10000);
        }
    }

    public void WinGame() {
        
        uiController.DisplayWinText();
    }

    public void LoseGame() {
        uiController.DisplayLoseMenu();
    }

    public void OnClickBundle1()
    {
        if (bundle1Purchased == true) return;

        if (playerBank.Coins < 500) return;
        //purchase red, blue and white ingredients
        for (int i = 0; i < 12; i++) {
            playerInventory.AddIngredientItem(ingredients[i]);
        }

        playerBank.RemoveCoins(500);

        bundle1Purchased = true;
    }

    public void OnClickBundle2()
    {
        if (bundle2Purchased == true) return;

        if (playerBank.Coins < 1000) return;
        //purchase yellow and green
        for (int i = 12; i < 20; i++)
        {
            playerInventory.AddIngredientItem(ingredients[i]);
        }

        playerBank.RemoveCoins(1000);

        bundle2Purchased = true;
    }

    public void OnClickBundle3() //purchase purple
    {
        if (bundle3Purchased == true) return;

        if (playerBank.Coins < 1500) return;

        //purchase yellow
        for (int i = 20; i < 24; i++)
        {
            playerInventory.AddIngredientItem(ingredients[i]);
        }

        playerBank.RemoveCoins(1500);

        bundle3Purchased = true;
    }

    public void OnWinButtonClick() {
        SceneManager.LoadScene(3);
    }

    //game phases:

    //phase 1: 
    // - 1.5 minutes
    // - player has access to: potion making, display tables, ugprades menu (if ever implemented) 
    // - phase where player tries to stock up asap and craft as many potions as possible (and also experiment with new recipes) 

    //phase 2:
    // - 1.5 minutes 
    // - player has access to: display tables, cash register, ugprades menu (upgrades will come on the next day)
    // - player must pay attention to the customers' reactions to potion prices 


}

