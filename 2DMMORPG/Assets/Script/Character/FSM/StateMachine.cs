using Script.Character.FSM;

namespace Assets.Script.Character
{
    public class StateMachine
    {
        private BaseState _currentState;

        public StateMachine(BaseState state)
        {
            _currentState = state;
            ChangeState(_currentState);
        }

        public void ChangeState(BaseState state)
        {
            if (_currentState == state) return;

            _currentState?.OnStateExit();

            _currentState = state;
            _currentState.OnStateEnter();
        }

        public void UpdateState()
        {
            if (_currentState is null) return;

            _currentState.OnStateUpdate();
        }
    }
}