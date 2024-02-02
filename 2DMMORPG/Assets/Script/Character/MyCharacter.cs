using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Script.Character
{
    public partial class MyCharacter : BaseCharacter
    {
        private Rigidbody2D _rigid;
        private Collider2D _collider;

        private float speed = 5.0f;

        private float jumpForce = 16.0f;

        private float groundCheckRadius = 0.1f;

        public LayerMask groundLayer = 6;


        private BaseState _stateRun;
        private BaseState _stateJump;
        private BaseState _stateAttack;

        private void Start()
        {
            Init();
        }

        protected override void Update()
        {
            base.Update();
        }

        private void FixedUpdate()
        {
            Move();
            Jump();
        }

        protected override void Init()
        {
            base.Init();
            GetComponents();
            _stateRun = new StateRun(this);
            _stateJump = new StateJump(this);
            _stateAttack = new StateAttack(this);
        }

        private void GetComponents()
        {
            Anim = GetComponentInChildren<Animator>();
            _rigid = GetComponentInChildren<Rigidbody2D>();
            _collider = GetComponentInChildren<Collider2D>();
        }

        void Move()
        {
            // if (_rigid.velocity.y != 0 && IsGrounded())
            // {
            //     return;
            // }

            var dirX = Input.GetAxis("Horizontal");
            TurnTransform(dirX);

            _rigid.velocity = new Vector2(dirX * speed, _rigid.velocity.y);

            if (IsGrounded() == false)
                _stateMachine.ChangeState(_stateJump);
            else if (dirX != 0)
                _stateMachine.ChangeState(_stateRun);
            else
                _stateMachine.ChangeState(_stateIdle);
        }

        private void TurnTransform(float dirX)
        {
            if (dirX == 0)
                return;
            var y = 0.0f < dirX ? 180.0f : 0.0f;
            transform.localRotation = Quaternion.Euler(0.0f, y, 0.0f);
        }

        void Jump()
        {
            if (!Input.GetKey(KeyCode.LeftAlt) || !IsGrounded()) return;
            _rigid.velocity = new Vector2(_rigid.velocity.x, jumpForce);
        }


        private void EnableCollider(bool enable)
        {
            _collider.enabled = enable;
        }
    }

    partial class MyCharacter
    {
        private const float Radius = 0.3f;
        private const float CastDistance = 0.3f;

        private bool IsGrounded()
        {
            if (0.0f < _rigid.velocity.y)
                return false;
            
            return Physics2D.CircleCast(
                _collider.bounds.center, // 시작점
                Radius, // 반지름
                Vector2.down, // 방향
                CastDistance, // 거리
                groundLayer);
        }
    }
}