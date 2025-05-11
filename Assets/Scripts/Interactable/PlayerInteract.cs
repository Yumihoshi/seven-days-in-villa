using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] InteractableItem curInteract;

    public void Interact()
    {
        curInteract?.Interact();
    }
}
