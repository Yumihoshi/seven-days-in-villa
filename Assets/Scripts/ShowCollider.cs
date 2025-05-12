using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ShowCollider : MonoBehaviour
{
   [SerializeField] TilemapRenderer tilemapRenderer;

   public void ShoworHideCollider()
   {
      tilemapRenderer.enabled=!tilemapRenderer.enabled;
   }
}
