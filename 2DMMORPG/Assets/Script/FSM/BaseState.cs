using Script.Character;

namespace Script.Creature.FSM
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