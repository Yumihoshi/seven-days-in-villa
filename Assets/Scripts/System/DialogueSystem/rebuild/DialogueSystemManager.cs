using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace DialogueSystem
{
    public class DialogueSystemManager : cjr.Single.SingleMon<DialogueSystemManager>
    {
        
        [SerializeField] TMPro.TextMeshProUGUI textMeshPro;
        [SerializeField] TMPro.TextMeshProUGUI speakerName;
        
        [ReadOnly] string OptionPrefabPath = "Prefabs/UI/Dialogue/ChosnOption";
        
        public Transform DialogueRoot;
        public Transform OptionRoot;
        [SerializeField] private float _wordInterval = 0.1f; // 每个字的间隔时间
        [SerializeField] private float _sentenceInterval = 1.0f; // 每句话的间隔时间
        
        [SerializeField] DialogueTree currentDialogueTree;
        
        WaitForSeconds _waitWordSecond     ;
        WaitForSeconds _waitSentenceSecond ;
        
        [SerializeField] DialogueNode nextNode;
        private int startIndex;

        public bool IsinOptions;
        
        public int CurrentOption = 0;

        public List<DialogueOptionNode> CurrentoptionNodes = new List<DialogueOptionNode>();

        [SerializeField] private string testFilePath;
        
        public bool optionConfirm;

        [SerializeField] private bool isDospeedUp;
        
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


        public void ReadyChosen()
        {
            ChosenColorClear();
            OptionRoot.GetChild(CurrentOption).GetChild(2).gameObject.SetActive(true);
        }

        public void ChosenColorClear()
        {
            int count=OptionRoot.childCount;
            for (int i = 0; i < count; i++)
            {
                var child = OptionRoot.GetChild(i);
                child.GetChild(2).gameObject.SetActive(false);
            }
        }
        
        
        protected override void Awake()
        {
            base.Awake();
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
            DialogueRoot.gameObject.SetActive(true);
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
            var textMeshPros = dialogueObject.GetComponentsInChildren<TextMeshProUGUI>();
            textMeshPros[0].text = optionNode.OptionText;
            textMeshPros[1].text = optionNode.id;
        }

        public void ClearOptionContent()
        {
            int count = OptionRoot.childCount;
            for (int i = 0; i < count; i++)
            {
                Destroy(OptionRoot.GetChild(i).gameObject);
            }
        }
        
        IEnumerator ShowDialogueNode(DialogueNode singleNode)
        {
            CurrentOption = 0;
            nextNode = null;
            speakerName.text = singleNode.SpeakerName;

            isDospeedUp = false;
            if (speakerName.text == "null")
            {
                nextNode = null;
                _waitSentenceSecond = new WaitForSeconds(0);
                speakerName.text = "";
                yield break;
            }
            //todo
            //ui的一些处理
            textMeshPro.text = String.Empty;

            
            

            if (singleNode.Content != "null")
            {
                int nums=singleNode.Content.Length/2==0?1:singleNode.Content.Length/2;
                foreach (var word in singleNode.Content)
                {
                    textMeshPro.text += word;
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
            
            textMeshPro.text=singleNode.Content;
            if(singleNode.nextNode!="null")
                nextNode=currentDialogueTree.nodes[int.Parse(singleNode.nextNode)-startIndex];
            else
            {
                nextNode=null;
            }
            yield return null;


           
            
            if(singleNode.nodeType==NodeType.option)
            {
                textMeshPro.text = String.Empty;
                IsinOptions = true;
                CurrentoptionNodes.Clear();
                //todo
                //处理选项
                CurrentoptionNodes =new List<DialogueOptionNode>(singleNode.OptionNodes);
                foreach (var optionNode in singleNode.OptionNodes)
                {
                    SetOptionContent(optionNode,ResoureManager.
                        LoadGameobject(OptionPrefabPath,OptionRoot));
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
                
               
                if(currentNode==null)
                   break;
                yield return _waitSentenceSecond;

            }
            //todo
            //ui的一些处理
            yield return _waitSentenceSecond;
            DialogueRoot.gameObject.SetActive(false);
            ApplicationFacade.Instance.SendNotification(NotificationConst.End_Dialogue);
        }
        
        
        [Button("Test Start Dialogue")]
        public void TestStartDialogue()
        { 
            Debug.LogWarning(ResoureManager.LoadGameobject(OptionPrefabPath));
            StartDialogue(currentDialogueTree);
        }
        
    }
    
    
}