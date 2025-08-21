using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SwitchRoom : InteractableItem
{
    [SerializeField] private PolygonCollider2D NextConfiner;
    [SerializeField] Transform NextPoisition;
    [SerializeField] private float duration = 0.2f;
    [SerializeField] private Collider2D Collider2D;
    private WaitForSeconds _waitForSeconds;
    protected override void Awake()
    {
        base.Awake();
        Collider2D = GetComponent<Collider2D>();
        Collider2D.isTrigger = true;
        _waitForSeconds = new WaitForSeconds(duration);
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        UiGameobject.Instance.SetInteractableInfo("press E to enter Other",1.5f);
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
    }

    public override void Interact()
    {
        base.Interact();
        UiGameobject.Instance.SetInteractbleInfoClose();
        StartCoroutine(interactRoutine());
    }
    
    IEnumerator interactRoutine()
    {
        PlayerUiInput.Instance.playerInput.SwitchCurrentActionMap("Menu");
        cjr.Scence.SceneManager.Instance.FainOut(1, duration);
        yield return  _waitForSeconds;
        PlayerAction.Instance.transform.position = NextPoisition.position;
        Debug.LogWarning(VcmManager.Instance);
        VcmManager.Instance.SwitchConfiner2D(NextConfiner);
        yield return _waitForSeconds;
        yield return _waitForSeconds;
        cjr.Scence.SceneManager.Instance.FainOut(0, duration).OnComplete(() =>
        {
            PlayerUiInput.Instance.playerInput.SwitchCurrentActionMap("GamePlay");
        });
    }
}
