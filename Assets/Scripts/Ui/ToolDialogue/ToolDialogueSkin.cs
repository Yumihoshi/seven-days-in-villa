using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToolDialogueSkin : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI Content;
    [SerializeField] private float interval = 0.1f;

    [SerializeField] private string Coroitinue;
    
    [SerializeField] Transform OptionHolder;
    
    
    public void SetSentence(string sentence)
    {
        ShowToolDialogue();
        if (Coroitinue != null)
            CoroutineFactory.Instance.HaltCoroutine(Coroitinue);
        Coroitinue = CoroutineFactory.Instance.RunCoroutine(TypingWord(sentence));
    }

    IEnumerator TypingWord(string sentence)
    {
        
        Content.text = string.Empty;
        foreach (var word in sentence)
        {
            this.Content.text += word;
            yield return new WaitForSeconds(interval);
        }

        Content.text = sentence;
        PlayerAction.Instance.playerInput.SwitchCurrentActionMap("GamePlay");
    }


    public void CleanOptions()
    {
        for (int i = 0; i < OptionHolder.childCount; i++)
        {
            Transform child = OptionHolder.GetChild(i);
            child.gameObject.SetActive(false);
        }
    }
    
    public void ShowToolDialogue()
    { 
        gameObject.SetActive(true);
    }

    private void OnDisable()
    {

        if (Coroitinue != null)
        {
            CoroutineFactory.Instance.HaltCoroutine(Coroitinue);
        }
        
    }
}
