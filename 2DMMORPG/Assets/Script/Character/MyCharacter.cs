using System.Collections;
using Assets.Script;
using Assets.Script.Character;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Script.Character
{
    public partial class MyCharacter : BaseCharacter
    {
        private BaseState _stateRun;
        private BaseState _stateJump;
        private BaseState _stateAttack;
        private float _dirX;

        private void Start()
        {
            Init();
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
            _rigid = GetComponentInChildren<Rigidbody2D>();
            _collider = GetComponentInChildren<Collider2D>();
        }


        private void FixedUpdate()
        {
            Move();
            Jump();
            UpdateState();
        }

        private void Move()
        {
            _dirX = Input.GetAxis("Horizontal");
            _rigid.velocity = new Vector2(_dirX * speed, _rigid.velocity.y);

            if (_dirX == 0) return;

            transform.localRotation = Quaternion.Euler(0.0f, (0 < _dirX ? 180.0f : 0.0f), 0.0f);
        }

        private void UpdateState()
        {
            if (IsGrounded() == false) _stateMachine.ChangeState(_stateJump);
            else if (_dirX == 0.0f) _stateMachine.ChangeState(_stateIdle);
            else _stateMachine.ChangeState(_stateRun);
        }

        private void Jump()
        {
            if (0.1f < _rigid.velocity.y)
                return;

            if (Input.GetKey(KeyCode.DownArrow) && IsGrounded())
            {
                if (Input.GetKeyDown(KeyCode.LeftAlt) && _isDownJumping is false)
                {
                    DisablePlatformCollider().Forget();
                }
            }
            else if (Input.GetKey(KeyCode.LeftAlt) && IsGrounded())
            {
                _rigid.velocity = new Vector2(_rigid.velocity.x, JumpForce);
            }
        }

        private bool _isDownJumping;

        private async UniTaskVoid DisablePlatformCollider()
        {
            _isDownJumping = true;
            var targetPosY = transform.position.y - 1.645f;

            EnableCollider(true);
            while (targetPosY < transform.position.y)
            {
                await UniTask.Yield(PlayerLoopTiming.Update); 
            }

            EnableCollider(false);
            _isDownJumping = false;
        }
      
        private void EnableCollider(bool enable)
        {
            _collider.isTrigger = enable;
        }
    }

    partial class MyCharacter
    {
        [SerializeField] private float CastDistance = 0.45f;

        private readonly Vector2 _groundCheckBoxSize = new Vector2(0.56f, 0.03f);

        // private static readonly int GroundLayer = LayerMask.NameToLayer("Ground");
        [SerializeField] private LayerMask _groundLayerMask = 1 << 6;

        private bool IsGrounded()
        {
            return Physics2D.BoxCast(_collider.bounds.center, _groundCheckBoxSize, 0f, Vector2.down,
                CastDistance,
                _groundLayerMask);
        }

        private RaycastHit2D HitGround()
        {
            return Physics2D.BoxCast(_collider.bounds.center, _groundCheckBoxSize, 0f, Vector2.down,
                CastDistance,
                _groundLayerMask);
        }

        // private void OnDrawGizmos()
        // {
        //     Vector2 boxDirection = _collider.bounds.center;
        //     Gizmos.color = Color.black;
        //     var finalPosition = boxDirection + Vector2.down * CastDistance;
        //     Gizmos.DrawWireCube(finalPosition, _groundCheckBoxSize);
        // }
    }
}