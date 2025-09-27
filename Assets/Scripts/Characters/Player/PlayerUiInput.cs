using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUiInput : cjr.Single.SingleMon<PlayerUiInput>
{
    [SerializeField] private Transform SettingsUi;

    public PlayerInput playerInput;

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
            
            
            if (PopUiPanelController.Instance.PopStack.Count > 0)
            {
                PopUiPanelController.Instance.CloseCurrentPopUiPanel();
                return;
            }
            SettingsUi.gameObject.SetActive(false);
            inputState = PutState.Idle;
            playerInput.SwitchCurrentActionMap("GamePlay");
        }
    }

    public void SpeakingSpeeding(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
           DialogueSystemManager.Instance.DoSpeedUp();
           ToolDialogueSkin.Instance.DoSpeedUp();
        }
    }


    public void ShopSwitching(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 inputVector = context.ReadValue<Vector2>();
            ApplicationFacade.Instance.SendNotification(NotificationConst.ShopSwitch, inputVector);
        }
    }


    public void ShopPurchased(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ApplicationFacade.Instance.SendNotification(NotificationConst.ShopPurchaseConfirm);
        }
    }
    
}


public enum PutState
{
    Idle,
    InUi
}
