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
    [SerializeField] PutState inputState;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        inputState = PutState.Idle;
    }

    public void ShowSettingsUi(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SettingsUi.gameObject.SetActive(true);
            inputState = PutState.InUi;
            playerInput.SwitchCurrentActionMap("Menu");
        }
    }

    public void ShowOrhideSave(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (inputState == PutState.Idle)
            {
                SavingUi.gameObject.SetActive(!SavingUi.gameObject.activeSelf);
            }
        }
    }

    public void HideSettingsUi(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SettingsUi.gameObject.SetActive(false);
            inputState = PutState.Idle;
            playerInput.SwitchCurrentActionMap("GamePlay");
        }
    }
}


public enum PutState
{
    Idle,
    InUi
}
