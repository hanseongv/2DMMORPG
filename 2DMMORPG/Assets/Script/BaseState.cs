using Assets.Script.Character;

namespace Assets.Script
{
    public abstract class BaseState
    {
        protected BaseState(BaseCharacter character)
        {
            Character = character;
        }

        internal readonly BaseCharacter Character;

        public abstract void OnStateEnter();
        public abstract void OnStateUpdate();
        public abstract void OnStateExit();
    }
}