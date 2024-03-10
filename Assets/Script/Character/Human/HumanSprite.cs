using System.Collections.Generic;
using UnityEngine;

namespace Script.Character
{
    public enum ItemType
    {
        Hair,
        FaceHair,
        Cloth,
        Pant,
        Helmet,
        Armor,
        WeaponsR,
        WeaponsL,
        Back,
    }

    public class ItemInfo
    {
        public ItemInfo(int itemId, ItemType itemType, string name)
        {
            ItemId = itemId;
            Name = name;
            ItemType = itemType;
        }

        public int ItemId;
        public ItemType ItemType;
        public string Name;
    }

    public class CharacterSprite
    {
        private BaseCharacter _baseCharacter;

        public CharacterSprite(BaseCharacter baseCharacter)
        {
            _baseCharacter = baseCharacter;
        }

        public List<List<SpriteRenderer>> _spriteList = new List<List<SpriteRenderer>>();
        public List<SpriteRenderer> _eyeList = new List<SpriteRenderer>();
        public List<SpriteRenderer> _bodyList = new List<SpriteRenderer>();
        public List<SpriteRenderer> _hairList = new List<SpriteRenderer>();
        public List<SpriteRenderer> _clothList = new List<SpriteRenderer>();
        public List<SpriteRenderer> _armorList = new List<SpriteRenderer>();
        public List<SpriteRenderer> _pantList = new List<SpriteRenderer>();
        public List<SpriteRenderer> _weaponList = new List<SpriteRenderer>();
        public List<SpriteRenderer> _backList = new List<SpriteRenderer>();


        public void Init()
        {
            var transform = _baseCharacter.transform;
            var sr = new List<SpriteRenderer>();
            FindAllSpriteRenderersInChildren(transform, sr);


            foreach (var v in sr)
            {
                switch (v.name)
                {
                    case "6_R_Eye":
                    case "6_L_Eye":
                        _eyeList.Add(v);
                        break;
                    case "Body":
                    case "5_Head":
                    case "20_L_Arm":
                    case "20_R_Arm":
                    case "_10R_Foot":
                    case "_5L_Foot":
                        _bodyList.Add(v);
                        break;
                    case "6_FaceHair":
                    case "10_Hair":
                    case "11_Helmet1":
                    case "12_Helmet2":
                        _hairList.Add(v);
                        break;
                    case "ClothBody":
                    case "21_LCArm":
                    case "-19_RCArm":
                        _clothList.Add(v);
                        break;
                    case "BodyArmor":
                    case "25_L_Shoulder":
                    case "-15_R_Shoulder":
                        _armorList.Add(v);
                        break;
                    case "_9R_Cloth":
                    case "_4L_Cloth":
                        _pantList.Add(v);
                        break;
                    case "L_Weapon":
                    case "L_Shield":
                    case "R_Weapon":
                    case "R_Shield":
                        _weaponList.Add(v);
                        break;
                    case "Back":
                        _backList.Add(v);
                        break;
                }
            }

            _spriteList.Add(_eyeList);
            _spriteList.Add(_bodyList);
            _spriteList.Add(_hairList);
            _spriteList.Add(_clothList);
            _spriteList.Add(_armorList);
            _spriteList.Add(_pantList);
            _spriteList.Add(_weaponList);
            _spriteList.Add(_backList);
        }

        public void SetSprite(ICollection<SpriteRenderer> spriteList, string spriteName, Object texture = null)
        {
            var t = texture == null ? null : texture as Sprite;
            foreach (var v in spriteList)
            {
                if (v.name == spriteName)
                    v.sprite = t;
            }
        }

        private void FindAllSpriteRenderersInChildren(Transform parent, ICollection<SpriteRenderer> spriteList)
        {
            // 현재 레벨에서 SpriteRenderer 컴포넌트가 있는지 확인하고, 있다면 리스트에 추가
            var spriteRenderer = parent.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteList.Add(spriteRenderer);
            }

            // 현재 레벨의 모든 자식을 반복하며 재귀적으로 탐색
            foreach (Transform child in parent)
            {
                FindAllSpriteRenderersInChildren(child, spriteList);
            }
        }

        public void Clear()
        {
            foreach (var s in _spriteList)
            {
                if (s == _eyeList || s == _bodyList)
                    continue;

                foreach (var v in s)
                {
                    if (v != null) v.sprite = null;
                }
            }
        }

        public void Clear(List<SpriteRenderer> spriteList)
        {
            spriteList.ForEach(x => x.sprite = null);
        }
    }
}