using UnityEngine;

namespace Assets.Script.Character
{
    // public enum CharacterStateType
    // {
    //     Idle,
    //     Run,
    //     Jump,
    //     Attack,
    // }

    public abstract class BaseCharacter : MonoBehaviour
    {
        // protected CharacterStateType CurrentCharacterStateType;
        protected internal Animator Anim;
        protected StateMachine _stateMachine;
        protected BaseState _stateIdle;

        protected virtual void Init()
        {
            _stateIdle = new StateIdle(this);
            _stateMachine = new StateMachine(_stateIdle);
            // CurrentCharacterStateType = CharacterStateType.Idle;
        }

        protected virtual void Update()
        {
            _stateMachine?.UpdateState();
        }
    }
}