using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashRegister : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    private bool _interactable = true;
    public bool isInteractable
    {
        get { return _interactable; }
        set { _interactable = value; }
    }

    [SerializeField] private TransactionManager transactionManager;

    public string InteractionPrompt => _prompt;
    public bool Interact(Interactor interactor)
    {
        transactionManager.MakePurchaseAndLeave();
        return true;
    }
}
