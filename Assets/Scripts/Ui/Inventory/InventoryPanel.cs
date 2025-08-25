using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : GameObjectSkin
{
    [SerializeField] private Transform PagesHolder;
    [SerializeField] private Transform ButtonsHolder;

    private void Start()
    {
        PagesHolder = GameSkins[0].transform;
        ButtonsHolder = GameSkins[1].transform;
        for (int i = 0; i < ButtonsHolder.childCount; i++)
        {
            Button button = ButtonsHolder.GetChild(i).GetComponent<Button>();
            button.onClick.RemoveAllListeners(); // ·ÀÖ¹ÖØ¸´×¢²á
            int index = i;
            button.onClick.AddListener(() => SwitchContentPage(index));
        }
        SwitchContentPage(0);
    }


    public void CleanShowPages()
    {
        for (int i = 0; i < PagesHolder.childCount; i++)
        {
            PagesHolder.GetChild(i).gameObject.SetActive(false);
        }
    }
    
    public void SwitchContentPage(int index)
    {
        if(index>4)
            return;
        CleanShowPages();
        PagesHolder.GetChild(index).gameObject.SetActive(true);
            
    }
}
