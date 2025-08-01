using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueSystemManager : cjr.Single.SingleMon<DialogueSystemManager>
    {
        
        public TMPro.TextMeshProUGUI textMeshPro;
        [SerializeField] float _wordInterval = 0.1f; // 每个字的间隔时间
        [SerializeField] float _sentenceInterval = 1.0f; // 每句话的间隔时间
        
        WaitForSeconds _waitWordSecond     ;
        WaitForSeconds _waitSentenceSecond ;
        
        [SerializeField] DialogueNode nextNode;


        public bool IsinOptions;
        
        int CurrentOption = -1;



        public void DoChosen(int optionIndex)
        {
            if (IsinOptions)
            {
                // 处理选项选择逻辑
                CurrentOption = optionIndex;
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
            if (dialogueTree == null || dialogueTree.DialogueNodes.Count == 0)
            {
                Debug.LogError("Dialogue tree is empty or null.");
                return;
            }

            nextNode = null;
            // Start the dialogue with the first node
            CoroutineFactory.Instance.RunCoroutine(ShowDialogueNodes(dialogueTree.DialogueNodes[0]));
        }

        //todo
        /// <summary>
        /// 快进对话节点
        /// </summary>
        /// <returns></returns>
        public bool InterruptDialogueNode()
        {
            return false;
        }

        //todo
        bool checkOptionEnd(DialogueNode currentNode)
        {
            if(CurrentOption==-1)
                return false;
            if(CurrentOption>= currentNode.OptionNodes.Count)
            {
                // 选项索引无效
                Debug.LogError("Invalid option index selected: " + CurrentOption);
                return false;
            }
            nextNode = currentNode.OptionNodes[CurrentOption];
            return nextNode!= null;
        }
        
        
        IEnumerator ShowDialogueNode(DialogueNode singleNode)
        {

            nextNode = null;
            //todo
            //ui的一些处理
            if(singleNode.nodeType==NodeType.option)
            {
                IsinOptions = true;
                //todo
                //处理选项
                
                while (true)
                {
                    
                    if(checkOptionEnd(singleNode))
                        break;
                    yield return null;
                }

                IsinOptions = false;
                yield break;
            }
            textMeshPro.text = String.Empty;

            foreach (var word in singleNode.Content)
            {
                textMeshPro.text += word;
                if(InterruptDialogueNode())
                    break;
                yield return _waitWordSecond;
                
            }
            
            textMeshPro.text=singleNode.Content;
            yield return null;
        }
        
        
        IEnumerator ShowDialogueNodes(DialogueNode startNode)
        {
            
            DialogueNode currentNode = startNode;
            while (currentNode != null)
            {
               //todo
               //每句话停顿也是不一定相同的
                yield return null;
                yield return ShowDialogueNode(currentNode);
                yield return _waitSentenceSecond;
                currentNode = nextNode;

            }
            
            //todo
            //ui的一些处理
            yield return null;
        }
        
    }
}