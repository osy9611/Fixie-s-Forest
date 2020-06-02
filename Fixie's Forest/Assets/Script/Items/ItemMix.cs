using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    Normal,
    Clear
}

[System.Serializable]
public struct Data
{
    public string Name1;            //각각의 아이템 이름과 조합을 해야하는 갯수를 기록한다
    public int ItemAmount1;
    public string Name2;
    public int ItemAmount2;
    public string Name3;
    public int ItemAmount3;
    public string Name4;
    public int ItemAmount4;
    public ItemType type;
    public Sprite MixItemImage;
}

public class ItemMix : MonoBehaviour {

    [SerializeField]
    public List<Data> Data;

    /*인벤토리에 조건에 충족되는 아이템이 있으면 값을 받아온다*/
    public List<Image> ItemImage1;      
    public List<Image> ItemImage2;
    public List<Image> ItemImage3;
    public List<Image> ItemImage4;

    /*UI 관련함수*/
    public UIManager UI;

    //Player 관련 함수
    public Player player;
    /*아이템 조합이 완료됬는지 확인하는 함수*/
    public bool MixDone;

    public Sprite[] Elemental;

    /*GameManger 관련함수*/
    public GameManager Manager;
    
    public int CanMixCount;
    public int ClearItemCount;
    //아이템이 클리어 관련 아이템인지 아니면 일반 아이템인지 판별하는 함수
    public List<ItemType> ItemTypes;

    public Color On = new Color(1, 1, 1, 1);
    public Color Off = new Color(1, 1, 1, 0);

    public EndPoint endpoint;   //클리어 조건 아이템
    public void CheckMix()    //조합하는 함수
    {
        for (int i=0;i<Data.Count;i++)
        {
            if (Data[i].Name1 == UI.ItemImage[0].sprite.name&&Data[i].Name2== UI.ItemImage[1].sprite.name)
            {
                UI.MixComplete.sprite = Data[i].MixItemImage;
                UI.MixComplete.color = On;
               MixDone = true;
            }
            else if (Data[i].Name1 == UI.ItemImage[1].sprite.name && Data[i].Name2 == UI.ItemImage[0].sprite.name)
            {
                UI.MixComplete.sprite = Data[i].MixItemImage;
                UI.MixComplete.color = On;
                MixDone = true;
            }
        }
    }

    public void CheckItems()        //어떤게 조합가능한지를 채크하는 함수
    {
        for (int i = 0; i < UI.ItemImage.Count; i++)
        {
            if (UI.ItemImage[i] != null)
            {
                if (UI.ItemImage[i].sprite == Elemental[0])
                {
                    ItemImage1.Add(UI.ItemImage[i]);
                }
                if (UI.ItemImage[i].sprite == Elemental[1])
                {
                    ItemImage2.Add(UI.ItemImage[i]);
                }
                if (UI.ItemImage[i].sprite == Elemental[2])
                {
                    ItemImage3.Add(UI.ItemImage[i]);
                }
                if (UI.ItemImage[i].sprite == Elemental[3])
                {
                    ItemImage4.Add(UI.ItemImage[i]);
                }
            }
        }

        for (int i=0;i< Data.Count;i++)
        {
            if(Data[i].ItemAmount3==0&&Data[i].ItemAmount4==0)
            {
                if (Data[i].ItemAmount1 <= ItemImage1.Count && Data[i].ItemAmount2 <= ItemImage2.Count)
                {
                    if (CanMixCount != UI.CanMix.Count)
                    {
                        if (Data[i].MixItemImage != null)
                        {
                            if(Data[i].MixItemImage.name==endpoint.ClearItem)
                            {
                                UI.CanMix[0].sprite = Data[i].MixItemImage;
                                UI.CanMix[0].color = new Color(1, 1, 1, 1);
                            }
                            if (Data[i].MixItemImage.name == "SpeedUp")
                            {
                                UI.CanMix[1].sprite = UI.OnItem[0];
                                UI.CanMix[1].color = new Color(1, 1, 1, 1);
                            }
                            if (Data[i].MixItemImage.name == "Ghost")
                            {
                                UI.CanMix[2].sprite = UI.OnItem[1];
                                UI.CanMix[2].color = new Color(1, 1, 1, 1);
                            }
                            if (Data[i].MixItemImage.name == "SuperPower")
                            {
                                UI.CanMix[3].sprite = UI.OnItem[2];
                                UI.CanMix[3].color = new Color(1, 1, 1, 1);
                            }
                            CanMixCount++;
                        }
                    }
                    else
                    {
                        CanMixCount = 0;
                    }
                }
            }
            else
            {
                if (Data[i].ItemAmount1 <= ItemImage1.Count && Data[i].ItemAmount2 <= ItemImage2.Count && Data[i].ItemAmount3 <= ItemImage3.Count && Data[i].ItemAmount4 <= ItemImage4.Count)
                {
                    if (CanMixCount != UI.CanMix.Count)
                    {
                        if (Data[i].MixItemImage != null)
                        {
                            if (Data[i].MixItemImage.name == endpoint.ClearItem)
                            {
                                UI.CanMix[0].sprite = Data[i].MixItemImage;
                                UI.CanMix[0].color = new Color(1, 1, 1, 1);
                            }
                            if (Data[i].MixItemImage.name == "SpeedUp")
                            {
                                UI.CanMix[1].sprite = UI.OnItem[0];
                            }
                            if (Data[i].MixItemImage.name == "Ghost")
                            {
                                UI.CanMix[2].sprite = UI.OnItem[1];
                            }
                            if (Data[i].MixItemImage.name == "SuperPower")
                            {
                                UI.CanMix[3].sprite = UI.OnItem[2];
                            }
                            CanMixCount++;
                        }
                    }
                    else
                    {
                        CanMixCount = 0;
                    }
                }
            }
        }

    }
  
    public void DeleteItmes(string Name)
    {
        for (int i=0;i<Data.Count;i++)
        {
            if (Data[i].MixItemImage != null && Data[i].MixItemImage.name == Name)
            {
                if (Data[i].Name1 != "" && Data[i].Name1 == ItemImage1[0].sprite.name)
                {
                    for (int j = 0; j < Data[i].ItemAmount1; j++)
                    {
                        ItemImage1[j].sprite = null;
                    }
                }
                if (Data[i].Name2 != "" && Data[i].Name2 == ItemImage2[0].sprite.name)
                {
                    for (int j = 0; j < Data[i].ItemAmount2; j++)
                    {
                        ItemImage2[j].sprite = null;
                    }
                    
                }
                if (Data[i].Name3!=""&&Data[i].Name3 == ItemImage3[0].sprite.name)
                {
                    for (int j=0;j<Data[i].ItemAmount3;j++)
                    {
                        ItemImage3[j].sprite = null;
                    }
                    
                }
                if(Data[i].Name4 != "" && Data[i].Name4==ItemImage4[0].sprite.name)
                {
                    for (int j=0;j<Data[i].ItemAmount4;j++)
                    {
                        ItemImage4[j].sprite = null;
                    }
                  
                }
            }
        }

        ItemImage1.Clear();
        ItemImage2.Clear();
        ItemImage3.Clear();
        ItemImage4.Clear();
        ItemTypes.Clear();
        CanMixCount = 0;
        
        player.ItemSprite.Clear();

        for (int s = 0; s < player.ItemCount; s++)
        {
            if (UI.ItemImage[s].sprite != null)
            {
                player.ItemSprite.Add(UI.ItemImage[s].sprite);
            }
        }

        player.ItemCount = player.ItemSprite.Count;

        for (int i = 0; i < player.MaxItemCount; i++)
        {
            UI.ItemImage[i].sprite = null;
            UI.ItemImage[i].color = Off;
        }

        for (int z = 0; z < player.ItemCount; z++)
        {
            UI.ItemImage[z].sprite = player.ItemSprite[z];
            UI.ItemImage[z].color = On;
        }

        for (int j = 0; j < UI.CanMix.Count; j++)
        {
            if(j==0)
            {
                UI.CanMix[j].sprite = null;
                UI.CanMix[j].color = Off;
            }
            else
            {
                UI.CanMix[j].sprite = UI.OffItem[j - 1];
            }
          
        }
        //UI.MixWindow.SetActive(false);
        //UI.MixWaitTime = 0;

        CheckItems();
    }

    public void ItemReset()
    {
        ItemImage1.Clear();
        ItemImage2.Clear();
        ItemImage3.Clear();
        ItemImage4.Clear();
        ItemTypes.Clear();
        CanMixCount = 0;
    }

   
}
