using System.Threading;
using Script.Character.Enemy;
using Script.Creature.Character;
using Script.Creature.FSM;
using UnityEngine;

namespace Script.Creature.Monster
{
    public class SlimeMonster : BaseMonster
    {
        [SerializeField] private float knockBackPower = 4;

        private BaseState _stateHit;

        protected override void Init()
        {
            base.Init();
            CurMonsterState = MonsterState.Idle;
            Speed = 1.0f;
            _stateHit = new MonsterStateHit(this);
        }

        private CancellationTokenSource _asyncHitToken;

        protected override void Hit(Vector2 attackerPos, int damage)
        {
            if (IsHit)
                return;
            IsHit = true;
            base.Hit(attackerPos, damage);

            _asyncHitToken?.Cancel();
            _asyncHitToken = new CancellationTokenSource();
            var dirX = transform.position.x < attackerPos.x ? 1 : -1;

            TurnCreature(dirX);
            Rigid.velocity = new Vector2(-dirX * knockBackPower, Rigid.velocity.y);

            CurMonsterState = MonsterState.Aggressive;
            StateMachine.ChangeState(_stateHit);
            IsHit = false;

            AsyncHit(0.5f, _asyncHitToken.Token).Forget();
        }
    }
}