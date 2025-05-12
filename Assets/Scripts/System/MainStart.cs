using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainStart : MonoBehaviour
{
    [SerializeField] GameData_So gameData_SO;

    [SerializeField] private CanvasGroup Mask;
    public void OpenNewGame()
    {
        gameData_SO.CurrentSaveSlotName = gameData_SO.SaveSlotNames[0];
        StartCoroutine(ChangeScene());
    }


    IEnumerator ChangeScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
        operation.allowSceneActivation = false; // �Ƚ�ֹ�Զ�����
        Mask.alpha = 0;
        Mask.DOFade(1, 0.1f);
        // �ȴ����ص� 90%
        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        // ��ʱ�����ѵ� 90%���������
        operation.allowSceneActivation = true;

        // �ȴ�������ȫ����
        while (!operation.isDone)
        {
            yield return null;
        }

        Mask.alpha = 0;
        Debug.Log("����������ϣ�");
    }
}
