using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUiInput : MonoBehaviour
{
    [SerializeField] private Transform SettingsUi;

    [SerializeField] PlayerInput playerInput;

    [SerializeField] private Transform SavingUi;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    public void ShowSettingsUi(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SettingsUi.gameObject.SetActive(true);
            playerInput.SwitchCurrentActionMap("Menu");
        }
    }

    public void ShowOrhideSave(InputAction.CallbackContext context)
    {
        if (context.performed)
            SavingUi.gameObject.SetActive(!SavingUi.gameObject.activeSelf);
    }

    public void HideSettingsUi(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SettingsUi.gameObject.SetActive(false);
            playerInput.SwitchCurrentActionMap("GamePlay");
        }
    }
}
