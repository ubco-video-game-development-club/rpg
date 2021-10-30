using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RPG.Animation;

namespace RPG
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator2D))]
    public class Actor : Entity
    {
        ///<summary>
        /// Value beyond which a direction is considered to be left, right,
        /// down, or up for the purposes of character animation orientation.
        ///</summary>
        private const float DIRECTION_LOOK_THRESHOLD = 0.45f;

        [SerializeField] private int initialMaxHealth;
        [SerializeField] private AnimationSet8D idleAnimations;
        [SerializeField] private AnimationSet8D moveAnimations;

        public int MaxHealth
        {
            get => GetProperty<int>(PropertyName.MaxHealth);
            private set => SetProperty<int>(PropertyName.MaxHealth, value);
        }

        public int Health
        {
            get => GetProperty<int>(PropertyName.Health);
            private set => SetProperty<int>(PropertyName.Health, value);
        }

        private UnityEvent<int> onDamageTaken = new UnityEvent<int>();
        public UnityEvent<int> OnDamageTaken { get => onDamageTaken; }

        private UnityEvent onDeath = new UnityEvent();
        public UnityEvent OnDeath { get => onDeath; }

        protected ActionData actionData;
        protected Vector2Int facingDirection;
        protected new Rigidbody2D rigidbody2D;
        protected Animator2D animator2D;

        protected virtual void Awake()
        {
            // Initialize components
            rigidbody2D = GetComponent<Rigidbody2D>();
            animator2D = GetComponent<Animator2D>();

            // Setup events
            AddPropertyChangedListener<int>(PropertyName.MaxHealth, (maxHealth) => Health = maxHealth);

            // Initialize properties
            MaxHealth = initialMaxHealth;
            facingDirection = Vector2Int.right;
        }

        protected virtual void Update()
        {
            // Animate movement
            if (GetMoveDirection() != Vector2Int.zero)
            {
                AnimateMove();
            }
            else
            {
                AnimateIdle();
            }
        }

        public virtual void TakeDamage(int damage)
        {
            Health = Mathf.Max(0, Health - damage);
            onDamageTaken.Invoke(damage);
            if (Health <= 0) Die();
        }

        protected virtual void Die()
        {
            onDeath.Invoke();
            Destroy(gameObject);
        }

        protected virtual void AnimateIdle()
        {
            animator2D.PlayAnimation(idleAnimations.Get(facingDirection), true);
        }

        protected virtual void AnimateMove()
        {
            facingDirection = GetMoveDirection();
            animator2D.PlayAnimation(moveAnimations.Get(facingDirection), true);
        }

        protected virtual void AnimateAction(Action action)
        {
            facingDirection = GetActionDirection();
            animator2D.PlayAnimation(action.Animation.Get(facingDirection), false);
        }

        protected Vector2Int GetMoveDirection()
        {
            int dirX = MathUtils.Sign(rigidbody2D.velocity.x);
            int dirY = MathUtils.Sign(rigidbody2D.velocity.y);
            return new Vector2Int(dirX, dirY);
        }

        protected Vector2Int GetActionDirection()
        {
            Vector2 mouseDiff = actionData.target - (Vector2)transform.position;
            float angle = Vector2.SignedAngle(Vector2.right, mouseDiff) * Mathf.Deg2Rad;
            float x = Mathf.Cos(angle);
            float y = Mathf.Sin(angle);
            int dirX = x > DIRECTION_LOOK_THRESHOLD ? 1 : x < -DIRECTION_LOOK_THRESHOLD ? -1 : 0;
            int dirY = y > DIRECTION_LOOK_THRESHOLD ? 1 : y < -DIRECTION_LOOK_THRESHOLD ? -1 : 0;
            return new Vector2Int(dirX, dirY);
        }
    }
}
