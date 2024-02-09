using Cysharp.Threading.Tasks;
using Script.Creature.Character;
using Script.Creature.FSM;
using UnityEngine;

namespace Script.Character.Human
{
    
    public partial class MyPlayer : BaseHuman
    {
        private BaseState _stateRun;
        private BaseState _stateJump;
        private BaseState _stateAttack;
        private float _dirX;

        private bool _isDownJumping;
      
        private void Start()
        {
            Init();
         
        }


        protected override void Init()
        {
            base.Init();
            BindGetComponent();
            _stateRun = new CharacterStateRun(this);
            _stateJump = new CharacterStateJump(this);
            _stateAttack = new CharacterStateAttack(this);
            
        }

        private void BindGetComponent()
        {
            Anim = GetComponentInChildren<Animator>();
            Rigid = GetComponentInChildren<Rigidbody2D>();
            Col = GetComponentInChildren<Collider2D>();
        }

        // [SerializeField] private ItemType itemType;
        // [SerializeField] private int ItemId;
        // [SerializeField] private int ListId;

        private void FixedUpdate()
        {
            // if (Input.GetKeyDown(KeyCode.Alpha1))
            // {
            //     // Exp += 10;
            //
            //     var itemInfo = new ItemInfo(ItemId, itemType, "hair");
            //
            //     SetSpriteItem(itemInfo);
            // }

            Move();
            Jump();
            UpdateState();
        }
    }

    partial class MyPlayer
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

}