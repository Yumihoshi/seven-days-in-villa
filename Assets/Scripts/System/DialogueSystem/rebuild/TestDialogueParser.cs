using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DialogueSystem
{
    public class TestDialogueParser
    {
        public static DialogueTree Parse(string FilePath)
        {
            DialogueTree JsonText = JsonUtility.FromJson<DialogueTree>(File.ReadAllText(FilePath));
            return JsonText;
        }
        

    }




}