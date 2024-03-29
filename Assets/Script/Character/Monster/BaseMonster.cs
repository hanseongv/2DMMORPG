using System;
using System.Threading;
using Cysharp.Threading.Tasks;
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

    public class BaseMonster : BaseCharacter
    {
        internal MonsterState CurMonsterState;

        private void Start()
        {
            Init();
            TempAutoStateChange();
        }

        protected override void Init()
        {
            base.Init();
            BindGetComponent();
            CurMonsterState = MonsterState.Idle;
        }

        private void BindGetComponent()
        {
            Anim = GetComponentInChildren<Animator>();
            Rigid = GetComponentInChildren<Rigidbody2D>();
            col = GetComponentInChildren<Collider2D>();
        }

        protected override void Hit(Vector2 attackerPos, int damage)
        {
            base.Hit(attackerPos, damage);
            _tempAutoStateChangeToken?.Cancel();
        }


        protected async UniTaskVoid AsyncHit(float time, CancellationToken cancellationToken = default)
        {
            await UniTask.Delay((int)(time * 1000), cancellationToken: cancellationToken);
            StateMachine.ChangeState(StateIdle);
            CurMonsterState = MonsterState.Idle;
            // TempAutoStateChange();
            // isHit = false;
        }

        private CancellationTokenSource _tempAutoStateChangeToken;

        private void TempAutoStateChange()
        {
            if (CurMonsterState is not (MonsterState.Idle or MonsterState.Patrolling))
                return;
            _tempAutoStateChangeToken = new CancellationTokenSource();
            CurMonsterState = (MonsterState)Random.Range(0, 2);
            CurMonsterState = MonsterState.Patrolling;
            var time = Random.Range(2.0f, 10.0f);
            var dirX = Random.Range(0, 2) == 0 ? -1 : 1;
            TurnCreature(dirX);

            switch (CurMonsterState)
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
                    await UniTask.Yield(PlayerLoopTiming.Update);
                    if (Rigid.velocity.y is >= 0.1f or >= 0.1f)
                        continue;
                    Rigid.velocity = new Vector2(dirX * Speed, Rigid.velocity.y);

                    if (IsGrounded()) break;
                }
            }

            TempAutoStateChange();
        }

        private async UniTaskVoid AsyncIdle(float time, CancellationToken cancellationToken = default)
        {
            await UniTask.Delay((int)(time * 1000), cancellationToken: cancellationToken);
            TempAutoStateChange();
        }

        protected override bool IsGrounded()
        {
            var bounds = col.bounds;
            var distance = bounds.size.y / 2.0f + 0.1f;

            return Physics2D.Raycast(bounds.center, Vector2.down, distance,
                GroundLayerMask);
        }
    }
}