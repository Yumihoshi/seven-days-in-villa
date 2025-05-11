using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventHandler
{
  public static event Action OnLoadScene;

  public static void LoadScene()
  {
      OnLoadScene?.Invoke();
  }
}
