using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopShopPanel : PopUiBasePanel
{

    public const string IsFirst = "FirstShopAppearFirst";

    [SerializeField] private bool opened;


    [SerializeField] private string SpritePath = "SvnResource/Art/Tools/商店道具/";
    
    
    [SerializeField] string path="Assets/Resources/SvnResource/文案/道具商品相关表格/商店存货单.xlsx";
    [SerializeField] List<BaseMeta> metas = new List<BaseMeta>();


    [SerializeField] float fadeTime = 0.4f;
    [SerializeField] private CanvasGroup confirmDecrations;

    [SerializeField] private TextMeshProUGUI IconName;
    [SerializeField] private TextMeshProUGUI Description;
    [SerializeField] private TextMeshProUGUI Price;
    [SerializeField] private TextMeshProUGUI Mymoney;
    
    
    [SerializeField] private Sprite[] oriClass;
    [SerializeField] private Sprite[] SwithchClass;

    [SerializeField] private List<Image> ChosenBar;


    [SerializeField] private int CurrentChosen;
    private Vector3 pos;

    [SerializeField] private Image ItemIcon;



    public void Refresh(int nowSan)
    {
        
    }
    
    public override void BeforeShowPopPanel()
    {
        pos = ItemIcon.transform.position;
        SpritePath = "SvnResource/Art/Tools/商店道具/";
        base.BeforeShowPopPanel();
        // Debug.LogWarning("用你心智的清明，来换取肉体的存续");
        if (!SaveSystemManager.Instance.IsContainKey(IsFirst))
        {
            
            UiGameobject.Instance.SetPopHintPopPanel("用你心智的清明，来换取肉体的存续",0.4f,true);
            
            SaveSystemManager.Instance.SaveObject(IsFirst,opened);
        }
        else
        {
            Debug.LogWarning("i am saved opened ");
        }

        Mymoney.text = "现有理智: " + PlayerHpSystem.Instance.GetMySan();
        
        CurrentChosen = 0;
        PlayerAction.Instance.playerInput.SwitchCurrentActionMap("ShopInput");
        ApplicationFacade.Instance.SendNotification(NotificationConst.ShopPanelCreate,this);
    }

    List<ShopGoodForSaveItem> shopGoodItems = new List<ShopGoodForSaveItem>();

    [SerializeField] private List<ShopGoodForSaveItem> RecoveryItems = new List<ShopGoodForSaveItem>();
    [SerializeField] private List<ShopGoodForSaveItem> AimingItems = new List<ShopGoodForSaveItem>();
    [SerializeField] private List<ShopGoodForSaveItem> EfficiencyItems = new List<ShopGoodForSaveItem>();
    
    [SerializeField] int itemChosen = 0;
    
    
    public override void AfterShowPopPanel()
    {
        pos = ItemIcon.transform.position;
        InitGoods();
        InitClassOfItems();
        
        itemChosen = 0;
        CurrentChosen = 0;
        SwitchGoods(Vector2.zero);
    }

    List<ShopGoodForSaveItem> GetClassOfItems()
    {
        switch (CurrentChosen)
        {
            case 0:
                return RecoveryItems;
            case 1:
                return AimingItems;
            case 2:
                return EfficiencyItems;
            default:
                return null;
        }
    }


    public void InitClassOfItems()
    {
        RecoveryItems=new List<ShopGoodForSaveItem>();
        AimingItems=new List<ShopGoodForSaveItem>();
        EfficiencyItems=new List<ShopGoodForSaveItem>();
        foreach (var item in shopGoodItems)
        {
            switch (item.itemType)
            {
                case ItemType.Recovery:
                    RecoveryItems.Add(item);
                    break;
                case ItemType.Aiming:
                    AimingItems.Add(item);
                    break;
                case ItemType.Efficiency:
                    EfficiencyItems.Add(item);
                    break;
            }
        }
    }

    void InitGoods()
    {
        metas=ExcelParser.ParseExcel(path);
        shopGoodItems=new List<ShopGoodForSaveItem>();
        foreach (var varMeta in metas)
        {
            ShopGoodForSaveItem item=new ShopGoodForSaveItem();
            item.ItemID = int.Parse((string)varMeta.Get("ID"));
            item.GoodDescription = (string)varMeta.Get(3);
         
            item.GoodPrice = ((int)varMeta.Get("GoodPrice"));
            
            BaseMeta tmp= MetaManager.Instance.GetToolMeta(item.ItemID);
            item.GoodName=(string) tmp.Get("GoodName");
            
            item.itemType=EnumParser.ParseItemType((string)MetaManager.Instance.GetToolMeta(item.ItemID).Get("GoodType"));
            
            
            
            
            item.Icon=ResourceLoader.Instance.LoadSprite(SpritePath+item.GoodName);
            shopGoodItems.Add(item);
        }
       
    }

    [SerializeField] private float downPoi = 10f;
    [SerializeField] private float duration = .5f;
    
    void AppearIcon()
    {
      
       
        ItemIcon.transform.localScale = Vector3.zero;
        Vector3 downpos = pos;
        downpos.y -= downPoi;
        ItemIcon.color = new Color(1f, 1f, 1f, 0);
        // ItemIcon.transform.position = downpos;

        ItemIcon.transform.position = downpos;
        
        ItemIcon.transform.DOMoveY(pos.y, duration);
        ItemIcon.DOFade(1, duration);
        ItemIcon.transform.DOScale(Vector3.one, duration);
        
    }
    
    public void ClearSp()
    {
        for (int i = 0; i < ChosenBar.Count; i++)
        {
            ChosenBar[i].sprite = oriClass[i];
            ChosenBar[i].SetNativeSize();
        }
    }
    
    public void SetChosen(int chosen,List<ShopGoodForSaveItem> chosenItems)
    {


        if (chosenItems.Count == 0)
        {
            ClearSp();
            ItemIcon.sprite=null;
            return;
        }
        
        if (itemChosen < 0 || itemChosen >= chosenItems.Count)
            return;
        
        ClearSp();
        ChosenBar[CurrentChosen].sprite = SwithchClass[CurrentChosen];
        ChosenBar[CurrentChosen].SetNativeSize();
        //todo
        
        IconName.SetText(chosenItems[chosen].GoodName);
        
        Description.SetText(chosenItems[chosen].GoodDescription);
        Price.SetText("花费: "+chosenItems[chosen].GoodPrice);
        ItemIcon.sprite = chosenItems[chosen].Icon;
        
        AppearIcon();
    }
    
    public void SwitchGoods(Vector2 v)
    {

        if (v.x != 0)
        {
            itemChosen = 0;
            CurrentChosen+=(int) (v.x / Mathf.Abs(v.x));
            if (CurrentChosen >=3)
            {
                CurrentChosen = 0;
            }

            if (CurrentChosen < 0)
            {
                CurrentChosen = 2;
            }
        }

        var lists = GetClassOfItems();

        if (v.y != 0)
        {
            itemChosen += (int)(v.y / Mathf.Abs(v.y));
            if (itemChosen >= lists.Count)
            {
                itemChosen = 0;
            }

            if (itemChosen < 0)
            {
                itemChosen = lists.Count - 1;
            }
        }
        
        SetChosen(itemChosen,lists);
    }


    ShopGoodForSaveItem GetNowChosenItem()
    {

        List<ShopGoodForSaveItem> now = GetClassOfItems();
        
       

        if (itemChosen < now.Count && itemChosen >= 0)
        {
            return now[itemChosen];
        }

        ShopGoodForSaveItem a = new ShopGoodForSaveItem();
        return a;
    }

    void RemoveItem(ShopGoodForSaveItem item)
    {
        var now = GetClassOfItems();
        for (int i = 0; i < now.Count; i++)
        {
            if (now[i].ItemID == item.ItemID)
            {
                now.RemoveAt(i);
                break;
            }
        }
        
        SwitchGoods(Vector2.down);
    }
    
    
    
    
    public void ConfirmPurchase()
    {
        
      
        if(GetClassOfItems().Count==0)
            return;
        
        
        var good = GetNowChosenItem();
        if(!PlayerHpSystem.Instance.subSan(good.GoodPrice))
            return;
      
        PlayerInventory.Instance.AddItem(good);



        int nowSan = PlayerHpSystem.Instance.GetMySan();
        
        
        
        
        
        PlayerAction.Instance.SetPlayerInputNull();
        confirmDecrations.DOFade(1f,fadeTime).OnComplete(() =>
        {
            Mymoney.text = $"现有理智: {nowSan}";
            confirmDecrations.DOFade(0f,fadeTime).OnComplete(() =>
            {
                RemoveItem(good);
                PlayerAction.Instance.playerInput.SwitchCurrentActionMap("ShopInput");
                SwitchGoods(Vector2.down);
            });
            
        });


       

    }
    
    public override void BeforeHidePopPanel()
    {
        base.BeforeHidePopPanel();
        ApplicationFacade.Instance.SendNotification(NotificationConst.ShopPanelHide);
    }
}
