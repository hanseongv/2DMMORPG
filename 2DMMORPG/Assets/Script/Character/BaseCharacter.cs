using Assets.Script;
using Assets.Script.Character;
using UnityEngine;

namespace Script.Character
{
    partial class BaseCharacter
    {
        protected float speed = 5.0f;
       protected const float JumpForce = 16.0f;
       protected Rigidbody2D _rigid;
       protected Collider2D _collider;
        protected internal Animator Anim;
        protected StateMachine _stateMachine;
        protected BaseState _stateIdle;
    }

    public abstract partial class BaseCharacter : MonoBehaviour
    {
        protected virtual void Init()
        {
            _stateIdle = new StateIdle(this);
            _stateMachine = new StateMachine(_stateIdle);
        }

        protected virtual void Update()
        {
            _stateMachine?.UpdateState();
        }
    }
}