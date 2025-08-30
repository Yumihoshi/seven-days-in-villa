using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToolDialogueSkin : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI Content;
    [SerializeField] private float interval = 0.1f;
    public void SetSentence(string sentence)
    {
        ShowToolDialogue();
        CoroutineFactory.Instance.RunCoroutine(TypingWord(sentence));
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
    }
    
    public void ShowToolDialogue()
    { 
        gameObject.SetActive(true);
    }
    
    
    
}
