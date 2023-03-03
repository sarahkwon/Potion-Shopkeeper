using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public bool isInteractable { get; set; }
    public string InteractionPrompt { get; }
    public bool Interact(Interactor interactor);
}
