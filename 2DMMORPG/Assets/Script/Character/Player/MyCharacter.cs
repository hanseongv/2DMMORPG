using System.Collections.Generic;
using Assets.Script;
using Assets.Script.Character;
using Cysharp.Threading.Tasks;
using Script.Character.FSM;
using UnityEngine;

namespace Script.Character.Player
{
    public partial class MyCharacter : BaseCharacter
    {
        [SerializeField] protected SpriteList SList;

        private BaseState _stateRun;
        private BaseState _stateJump;
        private BaseState _stateAttack;
        private float _dirX;
        private bool _isDownJumping;

        //0
        private string hairName = "10_Hair";

        //1
        private string _faceHairName = "6_FaceHair";

        //2
        private string _clothBodyName = "ClothBody";
        private string _lArmName = "21_LCArm";
        private string _rArmName = "-19_RCArm";

        //3
        private string _rClothName = "_9R_Cloth";
        private string _lClothName = "_4L_Cloth";

        //4
        private string _helmetName = "11_Helmet1";

        //5
        private string _bodyArmorName = "BodyArmor";
        private string _lShoulderName = "25_L_Shoulder";
        private string _rShoulderName = "-15_R_Shoulder";

        //6
        private string _rWeaponName = "R_Weapon";
        private string _rShieldName = "R_Shield";

        //7
        private string _lWeaponName = "L_Weapon";
        private string _lShieldName = "L_Shield";

        //8     
        private string BackName = "Back";

        //
        private const string BodyName = "Body";
        private const string LeftName = "Left";
        private const string RightName = "Right";
        private const string shieldName = "Shield";

        private void Start()
        {
            Init();
            SList = new SpriteList(this);
            SList.Init();
        }


        protected override void Init()
        {
            base.Init();
            BindGetComponent();
            _stateRun = new StateRun(this);
            _stateJump = new StateJump(this);
            _stateAttack = new StateAttack(this);
        }

        private void BindGetComponent()
        {
            Anim = GetComponentInChildren<Animator>();
            Rigid = GetComponentInChildren<Rigidbody2D>();
            Col = GetComponentInChildren<Collider2D>();
        }

        [SerializeField] private ItemType itemType;
        [SerializeField] private int ItemId;
        [SerializeField] private int ListId;

        private void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                // Exp += 10;

                var itemInfo = new ItemInfo(ItemId, itemType, "hair");

                SetSpriteItem(itemInfo);
            }

            Move();
            Jump();
            UpdateState();
        }
    }

    partial class MyCharacter
    {
        private void Move()
        {
            _dirX = Input.GetAxis("Horizontal");
            Rigid.velocity = new Vector2(_dirX * Speed, Rigid.velocity.y);

            if (_dirX == 0) return;

            TurnCreature(_dirX);
        }

        private void UpdateState()
        {
            if (IsGrounded() == false) StateMachine.ChangeState(_stateJump);
            else if (_dirX == 0.0f) StateMachine.ChangeState(StateIdle);
            else StateMachine.ChangeState(_stateRun);
        }

        private void Jump()
        {
            if (0.1f < Rigid.velocity.y)
                return;

            if (Input.GetKey(KeyCode.DownArrow) && IsGrounded())
            {
                if (Input.GetKeyDown(KeyCode.LeftAlt) && _isDownJumping is false)
                {
                    AsyncDisablePlatformCollider().Forget();
                }
            }
            else if (Input.GetKey(KeyCode.LeftAlt) && IsGrounded())
            {
                Rigid.velocity = new Vector2(Rigid.velocity.x, JumpForce);
            }
        }


        private async UniTaskVoid AsyncDisablePlatformCollider()
        {
            _isDownJumping = true;

            var targetPosY = transform.position.y - 1.645f; // 1.645f = tilemap 높이 

            EnableCollider(true);
            while (targetPosY < transform.position.y)
            {
                await UniTask.Yield(PlayerLoopTiming.Update);
            }

            EnableCollider(false);
            _isDownJumping = false;
        }
    }

    partial class MyCharacter
    {
        public void SetSpriteItem(ItemInfo itemInfo)
        {
            var itemListId = itemInfo.ItemType.GetHashCode();
            if (SList._spriteList.Count < itemListId || itemListId < 0)
            {
                print($"Null ItemList Id : {itemListId}");
                return;
            }

            var itemId = itemInfo.ItemId;

            if (0 <= itemId)
            {
                var itemPath = $"Character/Items/{itemListId}_{itemInfo.ItemType.ToString()}/";
                if (itemType is ItemType.WeaponsL or ItemType.WeaponsR)
                {
                    itemPath = $"Character/Items/6_Weapons/";
                }

                var textureChk = itemInfo.ItemType is ItemType.Cloth or ItemType.Pant or ItemType.Armor;

                Object[] tObj;
                if (textureChk)
                    tObj = Resources.LoadAll<Texture2D>(itemPath);
                else
                    tObj = Resources.LoadAll<Sprite>(itemPath);

                if (tObj.Length <= itemId)
                {
                    print($"Null Item Id : {itemId}");
                    return;
                }

                switch (itemListId)
                {
                    case 0:
                        // 헤어
                        SList.SetSprite(SList._hairList, hairName, tObj[itemId]);
                        SList.SetSprite(SList._hairList, _helmetName);
                        break;

                    case 1:
                        //수염
                        SList.SetSprite(SList._hairList, _faceHairName, tObj[itemId]);
                        break;

                    case 2:
                        // 옷
                        SList.SetSprite(SList._clothList, _clothBodyName);
                        SList.SetSprite(SList._clothList, _lArmName);
                        SList.SetSprite(SList._clothList, _rArmName);

                        var tSpriteCloth =
                            Resources.LoadAll<Sprite>(itemPath + tObj[itemId].name);

                        foreach (var v in tSpriteCloth)
                        {
                            var targetName = v.name switch
                            {
                                LeftName => _lArmName,
                                RightName => _rArmName,
                                _ => _clothBodyName
                            };
                            SList.SetSprite(SList._clothList, targetName, v);
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
                                LeftName => _lClothName,
                                _ => _rClothName,
                            };
                            SList.SetSprite(SList._pantList, targetName, v);
                        }

                        break;

                    case 4:
                        //헬멧
                        SList.SetSprite(SList._hairList, _helmetName, tObj[itemId]);
                        SList.SetSprite(SList._hairList, hairName);
                        break;

                    case 5:
                        // 갑옷
                        SList.SetSprite(SList._armorList, _bodyArmorName);
                        SList.SetSprite(SList._armorList, _lShoulderName);
                        SList.SetSprite(SList._armorList, _rShoulderName);

                        var tSpriteArmor =
                            Resources.LoadAll<Sprite>(itemPath + tObj[itemId].name);


                        foreach (var v in tSpriteArmor)
                        {
                            var targetName = v.name switch
                            {
                                LeftName => _lShoulderName,
                                RightName => _rShoulderName,
                                _ => _bodyArmorName
                            };
                            SList.SetSprite(SList._armorList, targetName, v);
                        }

                        break;

                    case 6:
                        //오른손 무기
                        print(tObj[itemId]);
                        var equipShieldR = tObj[itemId].name.Contains(shieldName);
                        var equipNameR = equipShieldR ? _rShieldName : _rWeaponName;
                        var nullNameR = equipShieldR ? _rWeaponName : _rShieldName;
                        SList.SetSprite(SList._weaponList, equipNameR, tObj[itemId]);
                        SList.SetSprite(SList._weaponList, nullNameR);
                        break;

                    case 7:
                        //왼손 무기
                        var equipShieldL = tObj[itemId].name.Contains(shieldName);
                        var equipNameL = equipShieldL ? _lShieldName : _lWeaponName;
                        var nullNameL = equipShieldL? _lWeaponName : _lShieldName;
                        SList.SetSprite(SList._weaponList, equipNameL, tObj[itemId]);
                        SList.SetSprite(SList._weaponList, nullNameL);
                        break;

                    case 8:
                        //뒤 아이템
                        SList.SetSprite(SList._backList, BackName, tObj[itemId]);
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
                        SList.SetSprite(SList._hairList, hairName);
                        break;

                    case 1:
                        //수염
                        SList.SetSprite(SList._hairList, _faceHairName);
                        break;

                    case 2:
                        // 옷
                        SList.Clear(SList._clothList);
                        break;

                    case 3:
                        //바지
                        SList.Clear(SList._pantList);

                        break;

                    case 4:
                        //헬멧
                        SList.SetSprite(SList._hairList, _helmetName);
                        break;

                    case 5:
                        // 갑옷
                        SList.Clear(SList._armorList);
                        break;

                    case 6:
                        //오른손 무기
                        SList.SetSprite(SList._weaponList, _rShieldName);
                        SList.SetSprite(SList._weaponList, _rWeaponName);
                        break;

                    case 7:
                        //왼손 무기
                        SList.SetSprite(SList._weaponList, _lShieldName);
                        SList.SetSprite(SList._weaponList, _lWeaponName);
                        break;

                    case 8:
                        //뒤 아이템
                        SList.Clear(SList._backList);
                        break;
                }
            }
        }
    }
}