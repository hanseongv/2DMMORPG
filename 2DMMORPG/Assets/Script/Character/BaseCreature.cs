using System;
using Assets.Script.Character;
using Script.Character.FSM;
using UnityEngine;

namespace Script.Character
{
    partial class BaseCreature
    {
        #region Field

        private int _level;
        private int _exp;
        private int _health;
        protected float Speed = 5.0f;
        protected const float JumpForce = 16.0f;
        protected bool isHit;

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
        private readonly LayerMask _groundLayerMask = 1 << 6;

        #endregion
    }

    public partial class BaseCreature : MonoBehaviour
    {
        protected virtual void Init()
        {
            StateIdle = new StateIdle(this);
            StateMachine = new StateMachine(StateIdle);
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

        protected bool IsGrounded()
        {
            var bounds = Col.bounds;
            var distance = bounds.size.y / 2.0f + 0.1f;
            return Physics2D.Raycast(bounds.center, Vector2.down, distance,
                _groundLayerMask);
        }
    }
}