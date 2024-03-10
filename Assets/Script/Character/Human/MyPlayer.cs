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

        private AnimationEventPlayer _animationEventPlayer;

        private void Start()
        {
            Init();
        }
 private void OnDrawGizmos()
    {
        // Vector2 characterPosition = transform.position;
        // Vector2 forwardDirection = transform.right;
        // Vector2 boxCenter = characterPosition + (forwardDirection * attackRange);
        //
        // Gizmos.color = boxColor;
        // // Gizmos를 사용하여 시각적으로 박스를 그리기 (에디터에서만 보임)
        // Gizmos.DrawWireCube(boxCenter, boxSize);
        
         Vector2 characterPosition = transform.position; // 캐릭터의 현재 위치
        Vector2 forwardDirection = transform.right*-1.0f; // 캐릭터가 바라보는 방향 (2D에서는 보통 right 사용)

        // 박스의 중심 위치 계산: 캐릭터 위치 + (바라보는 방향 * 공격 범위)
        Vector2 boxCenter = characterPosition + (forwardDirection * attackRange);

        // Gizmos 설정
        Gizmos.color = Color.black;

        // 회전을 고려하지 않는 박스 그리기 (2D 게임에서 캐릭터가 주로 수평으로 움직이는 경우)
        // 필요한 경우, 캐릭터의 회전을 반영하여 박스를 그릴 수 있습니다.
        Gizmos.DrawWireCube(boxCenter, new Vector2(1,1));
    }

        protected override void Init()
        {
            base.Init();
            BindGetComponent();
            _stateRun = new CharacterStateRun(this);
            _stateJump = new CharacterStateJump(this);
            _stateAttack = new CharacterStateAttack(this);
            _animationEventPlayer = GetComponentInChildren<AnimationEventPlayer>();
            _animationEventPlayer.Init();
            attackCoolTime = 0.0f;
        }

        private void BindGetComponent()
        {
            Anim = GetComponentInChildren<Animator>();
            Rigid = GetComponentInChildren<Rigidbody2D>();
            col = GetComponentInChildren<Collider2D>();
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

            Attack();
            if (isAttack)
                return;
            Move();
            Jump();
            UpdateState();
        }
    }

    partial class MyPlayer
    {
        private void Attack()
        {
            if (!Input.GetKey(KeyCode.LeftControl) || !IsGrounded() || isAttack || !isCanAttack) return;

            StateMachine.ChangeState(_stateAttack);
        }

        private void Move()
        {
            _dirX = Input.GetAxis("Horizontal");
            if (Input.GetKey(KeyCode.LeftArrow))
                _dirX = -1.0f;
            if (Input.GetKey(KeyCode.RightArrow))
                _dirX = 1.0f;
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
                if (Input.GetKey(KeyCode.LeftAlt) && _isDownJumping is false)
                {
                                print("ㅊㅔ크2");

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