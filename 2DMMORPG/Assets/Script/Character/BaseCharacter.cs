using System;
using Assets.Script.Character;
using Script.Creature.Character;
using Script.Creature.FSM;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Script.Character
{
    partial class BaseCharacter
    {
        #region Field

        private int _level;
        private int _exp;
        private int _health;
        protected float Speed = 5.0f;
        protected const float JumpForce = 16.0f;
        protected bool IsHit;

        #endregion

        #region Property

        protected int Exp
        {
            get => _exp;
            set
            {
                _exp = value;
                CheckLevelUp();
            }
        }

        protected int Health
        {
            get => _health;
            set => _health = Math.Max(0, value);
        }

        protected Rigidbody2D Rigid;
        protected Collider2D Col;
        protected internal Animator Anim;
        protected StateMachine StateMachine;
        protected BaseState StateIdle;
        protected readonly LayerMask GroundLayerMask = 1 << 6;

        #endregion
    }

    public partial class BaseCharacter : MonoBehaviour
    {
        protected virtual void Init()
        {
           
        }

        protected virtual void Update()
        {
            StateMachine?.UpdateState();
        }

        private void CheckLevelUp()
        {
            // targetExp = 목표 경험치를 넣어주면 됨.
            var targetExp = 5;

            if (Exp < targetExp) return;
            Exp -= targetExp;
            _level += 1;
        }


        protected virtual void Hit(Vector2 attackerPos, int damage)
        {
        }

        protected void TurnCreature(float dir)
        {
            transform.localRotation = Quaternion.Euler(0.0f, (0 < dir ? 180.0f : 0.0f), 0.0f);
        }

        protected void EnableCollider(bool enable)
        {
            Col.isTrigger = enable;
        }

        protected virtual bool IsGrounded()
        {
            var bounds = Col.bounds;
            var distance = 0.1f;
            return Physics2D.BoxCast(transform.position, bounds.size, 0, Vector2.down, distance,
                GroundLayerMask);
        }
    }
}