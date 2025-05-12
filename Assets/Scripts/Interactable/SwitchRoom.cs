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

    protected override void Awake()
    {
        base.Awake();
        Collider2D = GetComponent<Collider2D>();
        Collider2D.isTrigger = true;
    }

    public override void Interact()
    {
        base.Interact();
        StartCoroutine(interactRoutine());
    }

    IEnumerator interactRoutine()
    {
        cjr.Scence.SceneManager.Instance.FainOut(1, duration);
        yield return new WaitForSeconds(duration);
        PlayerAction.Instance.transform.position = NextPoisition.position;
        VcmManager.Instance.SwitchConfiner2D(NextConfiner);
        cjr.Scence.SceneManager.Instance.FainOut(0, duration);
    }
}
