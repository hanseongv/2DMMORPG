using UnityEngine;

namespace Script.Character.FSM
{
    public class StateIdle : BaseState
    {

        public StateIdle(BaseCreature character) : base(character)
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

    public class StateRun : BaseState
    {
        private static readonly int IsRun = Animator.StringToHash("isRun");

        public StateRun(BaseCreature character) : base(character)
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
        public StateJump(BaseCreature character) : base(character)
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
        public StateAttack(BaseCreature character) : base(character)
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
    public class StateHit : BaseState
    {

        public StateHit(BaseCreature character) : base(character)
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