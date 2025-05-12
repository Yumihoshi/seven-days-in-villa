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
        operation.allowSceneActivation = false; // 先禁止自动激活
        Mask.alpha = 0;
        Mask.DOFade(1, 0.1f);
        // 等待加载到 90%
        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        // 此时加载已到 90%，允许激活场景
        operation.allowSceneActivation = true;

        // 等待场景完全激活
        while (!operation.isDone)
        {
            yield return null;
        }

        Mask.alpha = 0;
        Debug.Log("场景加载完毕！");
    }
}
