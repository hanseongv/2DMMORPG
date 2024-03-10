using Script.Creature.FSM;
using UnityEngine;

namespace Script.Character.Human
{
    public class CharacterStateIdle : BaseState
    {
        public CharacterStateIdle(BaseCharacter character) : base(character)
        {
        }

        public override void OnStateEnter()
        {
            // Character.Anim.Play("Idle");
            Character.Anim.SetTrigger("doIdle");
            // Play();
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

    public class CharacterStateRun : BaseState
    {
        private static readonly int IsRun = Animator.StringToHash("isRun");

        public CharacterStateRun(BaseCharacter character) : base(character)
        {
        }

        public override void OnStateEnter()
        {
            // Character.Anim.Play("Run");
            Character.Anim.SetTrigger("doRun");

            Character.Anim.SetBool(IsRun, true);
        }

        public override void OnStateUpdate()
        {
        }

        public override void OnStateExit()
        {
            Character.Anim.SetBool(IsRun, false);
        }
    }

    public class CharacterStateJump : BaseState
    {
        public CharacterStateJump(BaseCharacter character) : base(character)
        {
        }

        public override void OnStateEnter()
        {
            // Character.Anim.Play("Jump");
            Character.Anim.SetTrigger("doJump");
        }

        public override void OnStateUpdate()
        {
        }

        public override void OnStateExit()
        {
        }
    }

    public class CharacterStateAttack : BaseState
    {
        public CharacterStateAttack(BaseCharacter character) : base(character)
        {
        }

        public override void OnStateEnter()
        {
            Character.isAttack = true;
            Character.isCanAttack = false;

            // Character.Anim.Play("Attack");
            Character.Anim.SetTrigger("doAttack");
        }

        public override void OnStateUpdate()
        {
        }

        public override void OnStateExit()
        {
        }
    }

    public class CharacterStateHit : BaseState
    {
        public CharacterStateHit(BaseCharacter character) : base(character)
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