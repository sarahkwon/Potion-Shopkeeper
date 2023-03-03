using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotionCraftingUI : MonoBehaviour
{
    [SerializeField] private InventoryUI ingredInventoryUI;
    private bool uiOpened = false;

    private List<PotionItem> potions;
    private PlayerInventory playerInventory;
    [SerializeField] private UIController uiController;
    [SerializeField] private PlayerBank playerBank;

    [SerializeField] private Button brewPotionButton; //this is for improvising
    [SerializeField] private Button recipeBrewButton; //this is for brewing recipes (quick crafting)
    [SerializeField] private Transform craftedBoxUI;
    [SerializeField] private GameObject improvMenu;
    public GameObject recipeMenu;
    [SerializeField] private RectTransform improvButton;
    [SerializeField] private RectTransform recipeButton;
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private Transform selRecipeBox;
    [SerializeField] private TMP_InputField qtyInputText;

    public AudioClip brewPotionSound;

    [SerializeField] private GameObject totalPriceSection;

    public bool menuOpenFlag = false; //improvise = 0/false, recipe = 1/true


    public float buttonSelectedPosX = -200f;
    public float buttonUnselectedPosX = -232.69f;

    public int numSlotsFilled = 0;

    private Item currItem;

    public bool UIOpened
    {
        get { return uiOpened; }
        set { uiOpened = value; }
    }
    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        
    }


    public void OpenUI() {
        if (!uiOpened) {
            playerInventory = FindObjectOfType<PlayerInventory>();
            potions = Resources.LoadAll<PotionItem>("").ToList();

            brewPotionButton.onClick.AddListener(OnBrewPotionButtonClick);
            recipeBrewButton.onClick.AddListener(OnClickRecipeBrewButton);
            ingredInventoryUI = GetComponentInChildren<InventoryUI>();
        }
        
        ResetCraftedUI();
        SendRecipeData(null);
        uiOpened = true;
    }

    public void CloseUI() { 
        
    }

    public void ResetTotalPriceSection() {
        totalPriceSection.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
        totalPriceSection.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "$0";
    }

    public void UpdateTotalPriceSection() {
        List<GameObject> icons = GetChildIcons();
        bool full = false;
        if (icons.Count == 4) {
            full = true;
        }
        Debug.Log(icons.Count);
        int price = 0;
        foreach (GameObject icon in icons)
        {
            IngredientItem item = ingredInventoryUI.GetInventoryItem(icon).item as IngredientItem;
            price += ingredInventoryUI.getIngredientPrice(item);

        }

        if (full == true)
        { //if all 4 slots are filled
            List<IngredientType> types = GetIngredientTypes(icons);
            PotionItem potionToBrew = GetPotionFromRecipe(types);

            if (potionToBrew != null)
            {
                bool hasRecipe = playerInventory.HasRecipe(potionToBrew);

                if (hasRecipe)
                { //player crafted potion before already
                    totalPriceSection.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = potionToBrew.itemName;
                }
                else
                {
                    totalPriceSection.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Unknown Recipe!";
                }

            }
            else
            {
                totalPriceSection.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Nonexistent Recipe";
            }
        }
        else {
            totalPriceSection.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
        }

        totalPriceSection.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "$" + price.ToString();
    }

    private void OnBrewPotionButtonClick() {
        List<GameObject> icons = GetChildIcons();

        if (icons.Count < 4)
        {
            ErrorCraftedUI("Place an ingredient in all 4 slots!");
        }
        else {
            List<IngredientType> types = GetIngredientTypes(icons);
            PotionItem brewedPotion = GetPotionFromRecipe(types);

            if (brewedPotion != null)
            {
                playerInventory.AddPotionItem(brewedPotion, 1);
                UpdateCraftedUI(brewedPotion);
                if (!playerInventory.HasRecipe(brewedPotion)) {
                    playerInventory.AddRecipe(brewedPotion);
                }
                PurchaseIngredients();
                ResetTotalPriceSection();
                uiController.fxAudioSource.clip = brewPotionSound;
                uiController.fxAudioSource.Play();
            }
            else {
                ErrorCraftedUI("No known recipe.");
            }
            

            //clear the slots
            ClearIngredientIcons(icons);
        }
    }

    public void PurchaseIngredients()
    {
        int price;
        bool success = int.TryParse(totalPriceSection.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text.Substring(1),out price);

        if (success && price <= playerBank.Coins)
        {
            playerBank.RemoveCoins(price);
        }
        else {
            ErrorCraftedUI("You do not have enough coins.");
        }
        
    }

    private void UpdateCraftedUI(PotionItem potion) {
        craftedBoxUI.GetChild(1).GetComponent<Image>().sprite = potion.icon;
        craftedBoxUI.GetChild(1).gameObject.SetActive(true);
        craftedBoxUI.GetChild(2).GetComponent<TextMeshProUGUI>().text = potion.name;
        craftedBoxUI.GetChild(3).GetComponent<TextMeshProUGUI>().text = potion.description;

    }

    private void ResetCraftedUI() {
        craftedBoxUI.GetChild(1).gameObject.SetActive(false);
        craftedBoxUI.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Brew any potion!";
        craftedBoxUI.GetChild(3).GetComponent<TextMeshProUGUI>().text = "";
    }

    private void ErrorCraftedUI(string errorText) {
        craftedBoxUI.GetChild(1).gameObject.SetActive(false);
        craftedBoxUI.GetChild(2).GetComponent<TextMeshProUGUI>().text = errorText;
        craftedBoxUI.GetChild(3).GetComponent<TextMeshProUGUI>().text = "";
    }



    private void ClearIngredientIcons(List<GameObject> icons) {
        foreach (GameObject icon in icons) {
            ingredInventoryUI.RemoveInventoryIcon(icon);
            Destroy(icon);
        }
    }

    private List<GameObject> GetChildIcons() {
        List<GameObject> icons = new List<GameObject>();
        foreach (Transform child in improvMenu.transform) {
            if (child.GetComponent<ItemUI>() != null) {
                icons.Add(child.gameObject);
            }
            
        }

        return icons;
    }

    private List<IngredientType> GetIngredientTypes(List<GameObject> icons) { 
        List<IngredientType> ingredientTypes = new List<IngredientType>();


        foreach (GameObject icon in icons) {
            IngredientItem item = ingredInventoryUI.GetInventoryItem(icon).item as IngredientItem;
            ingredientTypes.Add(item.ingredientType);
        }

        return ingredientTypes;
    }

    private PotionItem GetPotionFromRecipe(List<IngredientType> types) {

        for (int i = 0; i < potions.Count; i++)
        {
            List<IngredientType> currentRecipe = potions[i].recipe;

            if (IsSame(currentRecipe, types))
            {
                //brew potion
                Debug.Log(potions[i].name + " created!");
                return potions[i];
                

            }

        }

        return null; //potion was not found

       
    }

    bool IsSame(List<IngredientType> A, List<IngredientType> B)
    {
        if (A.Count != B.Count)
        {
            return false;
        }

        List<IngredientType> ASort = A.ToList();
        ASort.Sort();
        List<IngredientType> BSort = B.ToList();
        BSort.Sort();

        for (int i = 0; i < ASort.Count; i++)
        {
            if (ASort[i] != BSort[i])
            {
                return false;
            }
        }

        return true;
    }

    public void OnClickImproviseButton() {
        menuOpenFlag = false;
        recipeMenu.SetActive(false);
        improvMenu.SetActive(true);
        uiController.TogglePotionInventory(false);
        recipeButton.anchoredPosition = new Vector3(buttonUnselectedPosX, 234.1f, 0);
        improvButton.anchoredPosition = new Vector3(buttonSelectedPosX, 323f, 0);

        //change label to Ingredients
        label.text = "Ingredients";

    }

    public void OnClickRecipesButton() {
        menuOpenFlag = true;
        improvMenu.SetActive(false);
        recipeMenu.SetActive(true);
        uiController.TogglePotionInventory(true);
        recipeButton.anchoredPosition = new Vector3(buttonSelectedPosX, 234.1f, 0);
        improvButton.anchoredPosition = new Vector3(buttonUnselectedPosX, 323f, 0);

        //change label to Recipes
        label.text = "Recipes";

    }

    public void SendRecipeData(Item item) {
        
        if (item == null) {
            currItem = null;
            selRecipeBox.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Select a recipe"; //name

            //icon
            selRecipeBox.GetChild(3).GetComponent<PotionItemDataHolder>().Potion = null; //cheap way of storing the potion data that the player selected
            selRecipeBox.GetChild(3).gameObject.SetActive(false);

            //description
            selRecipeBox.GetChild(4).GetComponent<TextMeshProUGUI>().text = "";

            //quantity text and input
            selRecipeBox.GetChild(5).gameObject.SetActive(false);
            selRecipeBox.GetChild(6).gameObject.SetActive(false);

            //brew button
            selRecipeBox.GetChild(9).gameObject.SetActive(false);


            selRecipeBox.GetChild(10).gameObject.SetActive(false);
            selRecipeBox.GetChild(11).gameObject.SetActive(false);
            selRecipeBox.GetChild(11).GetComponent<TextMeshProUGUI>().text = "$0";
            return;
        }

        currItem = item;

        //name
        selRecipeBox.GetChild(2).GetComponent<TextMeshProUGUI>().text = item.name;

        //icon
        selRecipeBox.GetChild(3).gameObject.SetActive(true);
        selRecipeBox.GetChild(3).GetComponent<PotionItemDataHolder>().Potion = item; //cheap way of storing the potion data that the player selected
        selRecipeBox.GetChild(3).GetComponent<Image>().sprite = item.icon;

        //description
        selRecipeBox.GetChild(4).GetComponent<TextMeshProUGUI>().text = item.description;

        //quantity text and input
        selRecipeBox.GetChild(5).gameObject.SetActive(true);
        selRecipeBox.GetChild(6).gameObject.SetActive(true);
        selRecipeBox.GetChild(6).GetComponent<TMP_InputField>().text = "0";

        //brew button
        selRecipeBox.GetChild(9).gameObject.SetActive(true);

        selRecipeBox.GetChild(10).gameObject.SetActive(true);
        selRecipeBox.GetChild(11).gameObject.SetActive(true);

        //total cost text


    }

    public void UpdateSelRecipeCost() {
        //we get the item's recipe and sum up the price and get qty and then multipple it 
        if (currItem != null)
        {
            int qty;
            int.TryParse(selRecipeBox.GetChild(6).GetComponent<TMP_InputField>().text, out qty);
            //get recipe and cost of one
            PotionItem pItem = (PotionItem)currItem;
            List<IngredientType> recip = pItem.recipe;
            int cost = 0;
            for (int i = 0; i < recip.Count; i++)
            {
                cost += ingredInventoryUI.getIngredientPrice(recip[i]);
            }
            selRecipeBox.GetChild(11).GetComponent<TextMeshProUGUI>().text = "$" + (cost * qty).ToString();
        }
        else {
 
        }
        
    }

    //on click of a recipe brew bvutton
    //get the quantity input 
    //playerInventory.AddPotionItem(
    public void OnClickRecipeBrewButton() {
        //if quantity input == null || == 0 
        if (qtyInputText.text == "0" || qtyInputText.text == "" || qtyInputText.text.Substring(0,1) == "-") {
            Debug.Log("Please input a quantity greater than 0");
            return;
        }

        //get the qty from the input field
        int qty;
        int.TryParse(qtyInputText.text, out qty);

        //get the potion data from the recipeBox
        Item pot = selRecipeBox.GetChild(3).GetComponent<PotionItemDataHolder>().Potion;
        uiController.fxAudioSource.clip = brewPotionSound;
        uiController.fxAudioSource.Play();
        playerInventory.AddPotionItem(pot, qty);

        int cost;
        bool success = int.TryParse(selRecipeBox.GetChild(11).GetComponent<TextMeshProUGUI>().text.Substring(1), out cost);

        if (success) {
            if (cost <= playerBank.Coins) {
                playerBank.RemoveCoins(cost);
            }
            
        }
        
    }
}
