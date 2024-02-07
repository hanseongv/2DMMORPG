using System.Threading;
using Cysharp.Threading.Tasks;
using Script.Character.FSM;
using UnityEngine;

namespace Script.Character.Enemy.Monster
{
    public class SlimeMonster : BaseEnemy
    {
        [SerializeField] private float _knockbackPower = 4;

        private BaseState _stateHit;

        protected override void Init()
        {
            base.Init();
            _curMonsterState = MonsterState.Idle;
            Speed = 1.0f;
            _stateHit = new StateHit(this);
            // _stateRun = new StateRun(this);
            // _stateJump = new StateJump(this);
            // _stateAttack = new StateAttack(this);
        }

        [SerializeField] private GameObject obj;

        protected override void Update()
        {
            base.Update();
            // if (Input.GetKeyDown(KeyCode.Alpha1))
            // {
            //     Hit(obj.transform.position, 1);
            // }
        }

        private CancellationTokenSource _asyncHitToken;

        protected override void Hit(Vector2 attackerPos, int damage)
        {
            if (isHit)
                return;
            isHit = true;
            base.Hit(attackerPos, damage);

            _asyncHitToken?.Cancel();
            _asyncHitToken = new CancellationTokenSource();
            var dirX = transform.position.x < attackerPos.x ? 1 : -1;

            TurnCreature(dirX);
            Rigid.velocity = new Vector2(-dirX * _knockbackPower, Rigid.velocity.y);

            _curMonsterState = MonsterState.Aggressive;
            StateMachine.ChangeState(_stateHit);
            isHit = false;

            AsyncHit(0.5f, _asyncHitToken.Token).Forget();
        }
    }
}