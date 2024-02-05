namespace Script.Character.FSM
{
    public abstract class BaseState
    {
        protected BaseState(BaseCreature character)
        {
            Character = character;
        }

        internal readonly BaseCreature Character;

        public abstract void OnStateEnter();
        public abstract void OnStateUpdate();
        public abstract void OnStateExit();
    }
}