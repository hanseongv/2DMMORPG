using System;
using Assets.Script.Character;
using Script.Creature.Character;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Script.Character.Human
{
    public class BaseHuman : BaseCharacter
    {
        #region SpriteName

        private const string HairName = "10_Hair";
        private const string FaceHairName = "6_FaceHair";
        private const string ClothBodyName = "ClothBody";
        private const string LArmName = "21_LCArm";
        private const string RArmName = "-19_RCArm";
        private const string RClothName = "_9R_Cloth";
        private const string LClothName = "_4L_Cloth";
        private const string HelmetName = "11_Helmet1";
        private const string BodyArmorName = "BodyArmor";
        private const string LShoulderName = "25_L_Shoulder";
        private const string RShoulderName = "-15_R_Shoulder";
        private const string RWeaponName = "R_Weapon";
        private const string RShieldName = "R_Shield";
        private const string LWeaponName = "L_Weapon";
        private const string LShieldName = "L_Shield";
        private const string BackName = "Back";
        private const string BodyName = "Body";
        private const string LeftName = "Left";
        private const string RightName = "Right";
        private const string ShieldName = "Shield";

        #endregion

        private CharacterSprite _spriteList;

        protected override void Init()
        {
            base.Init();
            StateIdle = new CharacterStateIdle(this);
            StateMachine = new StateMachine(StateIdle);
            _spriteList = new CharacterSprite(this);
            _spriteList.Init();
        }

        public void SetSpriteItem(ItemInfo itemInfo)
        {
            var itemListId = itemInfo.ItemType.GetHashCode();
            if (_spriteList._spriteList.Count < itemListId || itemListId < 0)
            {
                print($"Null ItemList Id : {itemListId}");
                return;
            }

            var itemId = itemInfo.ItemId;

            if (0 <= itemId)
            {
                var itemPath = $"Character/Items/{itemListId}_{itemInfo.ItemType.ToString()}/";
                if (itemInfo.ItemType is ItemType.WeaponsL or ItemType.WeaponsR)
                {
                    itemPath = $"Character/Items/6_Weapons/";
                }

                // var textureChk = itemInfo.ItemType is ItemType.Cloth or ItemType.Pant or ItemType.Armor;

                var tObj = GameManager.ResoureceM.LoadAllResources<Object>(itemPath);

                // if (textureChk)
                //     tObj = Resources.LoadAll<Object>(itemPath);
                // else
                //     tObj = Resources.LoadAll<Sprite>(itemPath);

                if (tObj.Length <= itemId)
                {
                    print($"Null Item Id : {itemId}");
                    return;
                }

                switch (itemListId)
                {
                    case 0:
                        // 헤어
                        _spriteList.SetSprite(_spriteList._hairList, HairName, tObj[itemId]);
                        _spriteList.SetSprite(_spriteList._hairList, HelmetName);
                        break;

                    case 1:
                        //수염
                        _spriteList.SetSprite(_spriteList._hairList, FaceHairName, tObj[itemId]);
                        break;

                    case 2:
                        // 옷
                        _spriteList.SetSprite(_spriteList._clothList, ClothBodyName);
                        _spriteList.SetSprite(_spriteList._clothList, LArmName);
                        _spriteList.SetSprite(_spriteList._clothList, RArmName);

                        var tSpriteCloth =
                            Resources.LoadAll<Sprite>(itemPath + tObj[itemId].name);

                        foreach (var v in tSpriteCloth)
                        {
                            var targetName = v.name switch
                            {
                                LeftName => LArmName,
                                RightName => RArmName,
                                _ => ClothBodyName
                            };
                            _spriteList.SetSprite(_spriteList._clothList, targetName, v);
                        }

                        break;

                    case 3:
                        //바지
                        var tSpritePant =
                            Resources.LoadAll<Sprite>(itemPath + tObj[itemId].name);
                        foreach (var v in tSpritePant)
                        {
                            var targetName = v.name switch
                            {
                                LeftName => LClothName,
                                _ => RClothName,
                            };
                            _spriteList.SetSprite(_spriteList._pantList, targetName, v);
                        }

                        break;

                    case 4:
                        //헬멧
                        _spriteList.SetSprite(_spriteList._hairList, HelmetName, tObj[itemId]);
                        _spriteList.SetSprite(_spriteList._hairList, HairName);
                        break;

                    case 5:
                        // 갑옷
                        _spriteList.SetSprite(_spriteList._armorList, BodyArmorName);
                        _spriteList.SetSprite(_spriteList._armorList, LShoulderName);
                        _spriteList.SetSprite(_spriteList._armorList, RShoulderName);

                        var tSpriteArmor =
                            Resources.LoadAll<Sprite>(itemPath + tObj[itemId].name);


                        foreach (var v in tSpriteArmor)
                        {
                            var targetName = v.name switch
                            {
                                LeftName => LShoulderName,
                                RightName => RShoulderName,
                                _ => BodyArmorName
                            };
                            _spriteList.SetSprite(_spriteList._armorList, targetName, v);
                        }

                        break;

                    case 6:
                        //오른손 무기
                        print(tObj[itemId]);
                        var equipShieldR = tObj[itemId].name.Contains(ShieldName);
                        var equipNameR = equipShieldR ? RShieldName : RWeaponName;
                        var nullNameR = equipShieldR ? RWeaponName : RShieldName;
                        _spriteList.SetSprite(_spriteList._weaponList, equipNameR, tObj[itemId]);
                        _spriteList.SetSprite(_spriteList._weaponList, nullNameR);
                        break;

                    case 7:
                        //왼손 무기
                        var equipShieldL = tObj[itemId].name.Contains(ShieldName);
                        var equipNameL = equipShieldL ? LShieldName : LWeaponName;
                        var nullNameL = equipShieldL ? LWeaponName : LShieldName;
                        _spriteList.SetSprite(_spriteList._weaponList, equipNameL, tObj[itemId]);
                        _spriteList.SetSprite(_spriteList._weaponList, nullNameL);
                        break;

                    case 8:
                        //뒤 아이템
                        _spriteList.SetSprite(_spriteList._backList, BackName, tObj[itemId]);
                        break;
                }
            }
            else
            {
                // 없을때 초기화 구문
                switch (itemListId)
                {
                    case 0:
                        // 헤어
                        _spriteList.SetSprite(_spriteList._hairList, HairName);
                        break;

                    case 1:
                        //수염
                        _spriteList.SetSprite(_spriteList._hairList, FaceHairName);
                        break;

                    case 2:
                        // 옷
                        _spriteList.Clear(_spriteList._clothList);
                        break;

                    case 3:
                        //바지
                        _spriteList.Clear(_spriteList._pantList);

                        break;

                    case 4:
                        //헬멧
                        _spriteList.SetSprite(_spriteList._hairList, HelmetName);
                        break;

                    case 5:
                        // 갑옷
                        _spriteList.Clear(_spriteList._armorList);
                        break;

                    case 6:
                        //오른손 무기
                        _spriteList.SetSprite(_spriteList._weaponList, RShieldName);
                        _spriteList.SetSprite(_spriteList._weaponList, RWeaponName);
                        break;

                    case 7:
                        //왼손 무기
                        _spriteList.SetSprite(_spriteList._weaponList, LShieldName);
                        _spriteList.SetSprite(_spriteList._weaponList, LWeaponName);
                        break;

                    case 8:
                        //뒤 아이템
                        _spriteList.Clear(_spriteList._backList);
                        break;
                }
            }
        }

      
    }
}