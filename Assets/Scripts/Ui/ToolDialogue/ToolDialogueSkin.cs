using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using FlexFramework.Excel;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class ToolDialogueSkin : cjr.Single.SingleMon<ToolDialogueSkin>
{

    [SerializeField] TextMeshProUGUI Content;
    [SerializeField] private float interval = 0.1f;

    [SerializeField] private string Coroitinue;
    
    [SerializeField] Transform OptionHolder;


  
    private void OnEnable()
    {
        CleanOptions();
    }

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
            CoroutineFactory.Instance?.HaltCoroutine(Coroitinue);
        }
        
    }


    [SerializeField] private string filePath;


    [Button("测试解析读表")]
    public void pahre()
    {
        // var fileContent = System.IO.File.ReadAllLines(filePath,System.Text.Encoding.UTF8);
        // WorkBook book = new FlexFramework.Excel.WorkBook(filePath);
        // Debug.LogWarning(book);
        // var sheet = book[0];
        // for (int r = 0; r < sheet.Rows.Count; r++)
        // {
        //     var row = sheet.Rows[r];
        //     for (int c = 0; c < row.Cells.Count; c++)
        //     {
        //         Debug.Log(row.Cells[c].Value);
        //     }
        // }
        
        var tree=DialogueParser.ParseDialogueNodes(filePath,
            "食用油",true);
        foreach (var le in tree)
        {
            var tol = le as ToolDialogueNode;
            Debug.LogWarning(tol);
            tol?.Debug();
        }
    }
    
    
    
    
    



    [Button("Test Start Dialogue")]
    public void TestStartDialogue()
    { 
        StartDialogue(currentDialogueTree);
    }













    [SerializeField] private bool isDospeedUp;

    public bool IsinOptions;
    public int CurrentOption;

    [SerializeField] private List<GameObject> Options;




    [SerializeField] private DialogueNode nextNode;
    
    [SerializeField] float _wordInterval;
    [SerializeField] float _sentenceInterval;

    [SerializeField] private WaitForSeconds _waitWordSecond;
    [SerializeField] private WaitForSeconds _waitSentenceSecond;

    public List<DialogueOptionNode> CurrentoptionNodes;
    
     public void DoSpeedUp()
     {
         isDospeedUp = true;
     }
     public void DoChosen(int optionIndex)
     {
         if (IsinOptions)
         {
                // 处理选项选择逻辑
                CurrentOption = optionIndex;
         }
     }

     [SerializeField] private int startIndex;



     [SerializeField] public bool optionConfirm;
     
     [SerializeField] private DialogueTree currentDialogueTree;
     public void ReadyChosen()
     {
         //todo 
            ChosenColorClear();
            OptionHolder.GetChild(CurrentOption).GetChild(2).gameObject.SetActive(true);
     }

     public void ChosenColorClear()
     {
         
         for (int i = 0; i < 4; i++)
         {
                var child = OptionHolder.GetChild(i);
                child.GetChild(2).gameObject.SetActive(false);
         }
     }
        
        
        protected  override void Awake()
        {
            nextNode = null;
            IsinOptions = false;
            _waitWordSecond = new WaitForSeconds(_wordInterval);
            _waitSentenceSecond = new WaitForSeconds(_sentenceInterval);
        }
        
        
        
        public void StartDialogue(DialogueTree dialogueTree)
        {
            if (dialogueTree == null || dialogueTree.nodes.Count == 0)
            {
                Debug.LogError("Dialogue tree is empty or null.");
                return;
            }
            ApplicationFacade.Instance.SendNotification(NotificationConst.Start_Dialogue);
            gameObject.SetActive(true);
            startIndex =int.Parse(dialogueTree.nodes[0].id);
            nextNode = null;
            // Start the dialogue with the first node
            CoroutineFactory.Instance.RunCoroutine(ShowDialogueNodes(dialogueTree.nodes[0]));
        }

        //todo
        /// <summary>
        /// 快进对话节点
        /// </summary>
        /// <returns></returns>
        public bool InterruptDialogueNode()
        {
            if (isDospeedUp)
            {
                isDospeedUp = false;
                return true;
            }
            isDospeedUp = false;
            return false;
        }

        //todo
        bool checkOptionEnd(DialogueNode currentNode)
        {
            if(CurrentOption>= currentNode.OptionNodes.Count)
            {
                // 选项索引无效
                Debug.LogError("Invalid option index selected: " + CurrentOption);
                return false;
            }
            DialogueOptionNode currentDialogueOption = currentNode.OptionNodes[CurrentOption];
            nextNode = currentDialogueTree.nodes [int.Parse(currentDialogueOption.nextNodeId)-startIndex ];
            return nextNode!= null;
        }

        void SetOptionContent(DialogueOptionNode optionNode, GameObject dialogueObject)
        {
            dialogueObject.SetActive(true);
            var textMeshPros = dialogueObject.GetComponentsInChildren<TextMeshProUGUI>();
            textMeshPros[0].text = optionNode.OptionText;
            textMeshPros[1].text = optionNode.id;
        }

        public void ClearOptionContent()
        {
            int count = 4;
            for (int i = 0; i < count; i++)
            {
                OptionHolder.GetChild(i).gameObject.SetActive(false);
            }
        }
        
        IEnumerator ShowDialogueNode(DialogueNode singleNode)
        {
            CurrentOption = 0;
            nextNode = null;
          

            isDospeedUp = false;
            if (singleNode.SpeakerName == "null")
            {
                nextNode = null;
                _waitSentenceSecond = new WaitForSeconds(0);
                yield break;
            }
            //todo
            //ui的一些处理
            Content.text = String.Empty;

            
            

            if (singleNode.Content != "null")
            {
                int nums=singleNode.Content.Length/2==0?1:singleNode.Content.Length/2;
                foreach (var word in singleNode.Content)
                {
                    Content.text += word;
                    if(InterruptDialogueNode())
                        break;
                    yield return _waitWordSecond;
                    
                }
                _waitSentenceSecond = new WaitForSeconds(_wordInterval*nums);
            }
            else
            {
                _waitSentenceSecond= new WaitForSeconds(0);
            }
            
            Content.text=singleNode.Content;
            if(singleNode.nextNode!="null")
                nextNode=currentDialogueTree.nodes[int.Parse(singleNode.nextNode)-startIndex];
            else
            {
                nextNode=null;
            }
            yield return null;


           
            
            if(singleNode.nodeType==NodeType.option)
            {
                Content.text = String.Empty;
                IsinOptions = true;
                CurrentoptionNodes.Clear();
                //todo
                //处理选项
                CurrentoptionNodes =new List<DialogueOptionNode>(singleNode.OptionNodes);
                int ii = 0;
                foreach (var optionNode in singleNode.OptionNodes)
                {
                    SetOptionContent(optionNode, Options[ii]);
                    ii++;
                }
                while (singleNode.OptionNodes.Count>0&&true)
                {
                    ReadyChosen();   
                    checkOptionEnd(singleNode);
                    yield return null;
                    if(optionConfirm)
                        break;
                }

                optionConfirm = false;
                checkOptionEnd(singleNode);
                IsinOptions = false;
                ClearOptionContent();
            }
            
            
        }


        IEnumerator ShowDialogueNodes(DialogueNode startNode)
        {

            DialogueNode currentNode = startNode;
            while (currentNode != null)
            {
                //todo
                //每句话停顿也是不一定相同的
                yield return null;
                yield return (ShowDialogueNode(currentNode));



                currentNode = nextNode;


                if (currentNode == null)
                    break;
                yield return _waitSentenceSecond;

            }

            //todo
            //ui的一些处理
            yield return _waitSentenceSecond;
            gameObject.SetActive(false);
            ApplicationFacade.Instance.SendNotification(NotificationConst.End_Dialogue);
        }
}
