using UnityEngine;

namespace Script.Character.Human
{
    public class AnimationEventPlayer : MonoBehaviour
    {
        private BaseCharacter _bCharacter;

        internal void Init()
        {
            _bCharacter = GetComponentInParent<BaseCharacter>();
        }

        public void OnAttackHit()
        {
            
            var bTransform = _bCharacter.transform;
            RaycastHit2D hit = Physics2D.BoxCast(bTransform.position+(bTransform.right*-1.0f*_bCharacter.attackRange),
                 new Vector2(1f, 1f), 0f,
                Vector2.zero, 0, _bCharacter.MonsterLayerMask);

            if (hit)
                print(hit.collider.name);
        }

        public void AttackEnd()
        {
            _bCharacter.isAttack = false;

            if (_bCharacter.attackCoolTime == 0)
            {
                _bCharacter.isCanAttack = true;
            }
            else
            {
                _bCharacter.AsyncAttackCoolTime().Forget();
            }
        }
    }
}