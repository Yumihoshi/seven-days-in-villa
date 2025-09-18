using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopShopPanel : PopUiBasePanel
{

    
    
    public override void BeforeShowPopPanel()
    {
        base.BeforeShowPopPanel();
        // Debug.LogWarning("用你心智的清明，来换取肉体的存续");
        UiGameobject.Instance.SetPopHintPopPanel("用你心智的清明，来换取肉体的存续",0.4f);
    }
}
