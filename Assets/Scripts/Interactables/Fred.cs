using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Fred : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    private UIController uiController;

    private bool uiOpen = false;

    public bool UIOpen { get { return uiOpen; } private set { uiOpen = value; } }

    private bool _interactable = true;

    public bool isInteractable
    {
        get { return _interactable; }
        set { _interactable = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        uiController = FindObjectOfType<UIController>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public string InteractionPrompt => _prompt;
    public bool Interact(Interactor interactor)
    {
        if (isInteractable)
        {
            uiOpen = !uiOpen;
            uiController.ToggleIngredientShopMenu(uiOpen, false);

            return true;
        }
        else
        {
            return false;
        }

    }
}
