using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Script.Character.FSM;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.Character.Enemy
{
    public enum MonsterState
    {
        Idle, // 몬스터가 아무 행동도 하지 않는 대기 상태
        Patrolling, // 몬스터가 정해진 경로나 지역을 순찰하는 상태
        Alert, // 몬스터가 플레이어의 존재를 인지하기 시작한 상태, 하지만 아직 전투 태세는 아님
        Aggressive, // 몬스터가 플레이어를 발견하고 전투 태세인 상태
        Attacking // 몬스터가 플레이어를 공격하는 상태
    }

    public class BaseEnemy : BaseCreature
    {
        internal MonsterState _curMonsterState;
        // private BaseState _stateRun;
        // private BaseState _stateJump;
        // private BaseState _stateAttack;

        private void Start()
        {
            Init();
            TempAutoStateChange();
        }

        protected override void Init()
        {
            base.Init();
            BindGetComponent();
            _curMonsterState = MonsterState.Idle;
        }

        private void BindGetComponent()
        {
            Anim = GetComponentInChildren<Animator>();
            Rigid = GetComponentInChildren<Rigidbody2D>();
            Col = GetComponentInChildren<Collider2D>();
        }

        protected override void Hit(Vector2 attackerPos,int damage)
        {
            base.Hit(attackerPos,damage);
            _tempAutoStateChangeToken.Cancel();
        }



        protected async UniTaskVoid AsyncHit(float time, CancellationToken cancellationToken = default)
        {
            await UniTask.Delay((int)(time * 1000), cancellationToken: cancellationToken);
            StateMachine.ChangeState(StateIdle);
            _curMonsterState = MonsterState.Idle;
            // TempAutoStateChange();
            // isHit = false;
        }

        private CancellationTokenSource _tempAutoStateChangeToken;

        private void TempAutoStateChange()
        {

            if (_curMonsterState is not MonsterState.Idle or MonsterState.Patrolling)
                return;

            _tempAutoStateChangeToken = new CancellationTokenSource();
            _curMonsterState = (MonsterState)Random.Range(0, 2);
            var time = Random.Range(2.0f, 10.0f);
            var dirX = Random.Range(0, 2) == 0 ? -1 : 1;
            TurnCreature(dirX);

            switch (_curMonsterState)
            {
                case MonsterState.Idle:
                    AsyncIdle(time, _tempAutoStateChangeToken.Token).Forget();
                    break;
                case MonsterState.Patrolling:
                    AsyncPatrolling(dirX, time, _tempAutoStateChangeToken.Token).Forget();
                    break;
                default:
                    break;
            }
        }

        private async UniTaskVoid AsyncPatrolling(int dirX, float time, CancellationToken cancellationToken = default)
        {
            var endTime = Time.time + time;
            while (Time.time < endTime)
            {
                cancellationToken.ThrowIfCancellationRequested();
                Rigid.velocity = new Vector2(dirX * Speed, Rigid.velocity.y);
                await UniTask.Yield(PlayerLoopTiming.Update);

                if (IsGrounded()) continue;

                dirX *= -1;
                TurnCreature(dirX);

                while (true)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    Rigid.velocity = new Vector2(dirX * Speed, Rigid.velocity.y);
                    await UniTask.Yield(PlayerLoopTiming.Update);

                    if (IsGrounded()) break;
                }
            }

            TempAutoStateChange();
        }

        private async UniTask AsyncIdle(float time, CancellationToken cancellationToken = default)
        {
            await UniTask.Delay((int)(time * 1000), cancellationToken: cancellationToken);
            TempAutoStateChange();
        }
    }
}