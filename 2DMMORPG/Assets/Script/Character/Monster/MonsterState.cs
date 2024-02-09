using Script.Character;
using Script.Creature.FSM;
using UnityEngine;

namespace Script.Creature.Character
{
    public class MonsterStateIdle : BaseState
    {
        public MonsterStateIdle(BaseCharacter character) : base(character)
        {
        }

        public override void OnStateEnter()
        {
            Character.Anim.Play("Idle");
            Character.Anim.SetBool("isIdle", true);
        }

        public override void OnStateUpdate()
        {
        }

        public override void OnStateExit()
        {
            Character.Anim.SetBool("isIdle", false);
        }
    }

    public class MonsterStateAttack : BaseState
    {
        public MonsterStateAttack(BaseCharacter character) : base(character)
        {
        }

        public override void OnStateEnter()
        {
        }

        public override void OnStateUpdate()
        {
        }

        public override void OnStateExit()
        {
        }
    }

    public class MonsterStateHit : BaseState
    {
        public MonsterStateHit(BaseCharacter character) : base(character)
        {
        }

        public override void OnStateEnter()
        {
            Character.Anim.Play("Hit");
            // Character.Anim.SetBool(IsIdle, true);
        }

        public override void OnStateUpdate()
        {
        }

        public override void OnStateExit()
        {
            // Character.Anim.SetBool(IsIdle, false);
        }
    }
}