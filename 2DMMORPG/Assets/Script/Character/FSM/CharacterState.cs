using Script.Character;
using UnityEngine;

namespace Assets.Script.Character
{
    public class StateIdle : BaseState
    {
        private static readonly int IsIdle = Animator.StringToHash("isIdle");

        public StateIdle(BaseCharacter character) : base(character)
        {
        }

        public override void OnStateEnter()
        {

            Character.Anim.Play("Idle");
            Character.Anim.SetBool(IsIdle, true);
        }

        public override void OnStateUpdate()
        {
        }

        public override void OnStateExit()
        {
            Character.Anim.SetBool(IsIdle, false);
        }
    }

    public class StateRun : BaseState
    {
        private static readonly int IsRun = Animator.StringToHash("isRun");

        public StateRun(BaseCharacter character) : base(character)
        {
        }

        public override void OnStateEnter()
        {
            Character.Anim.Play("Run");
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

    public class StateJump : BaseState
    {
        public StateJump(BaseCharacter character) : base(character)
        {
        }

        public override void OnStateEnter()
        {

            Character.Anim.Play("Jump");
        }

        public override void OnStateUpdate()
        {
        }

        public override void OnStateExit()
        {
        }
    }

    public class StateAttack : BaseState
    {
        public StateAttack(BaseCharacter character) : base(character)
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
}