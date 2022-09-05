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

        [SerializeField] private string displayName = "Actor";
        [SerializeField] private int initialMaxHealth;
        [SerializeField] private bool facingRight = true;
        [SerializeField] private Animation2D idleAnimation;
        [SerializeField] private Animation2D moveAnimation;
        [SerializeField] private Color flashColor = Color.red;
        [SerializeField] private float flashDuration = 0.1f;

        public string DisplayName { get => displayName; }

        public int MaxHealth
        {
            get => GetProperty<int>(PropertyName.MaxHealth);
            protected set => SetProperty<int>(PropertyName.MaxHealth, value);
        }

        public int Health
        {
            get => GetProperty<int>(PropertyName.Health);
            protected set => SetProperty<int>(PropertyName.Health, value);
        }

        public bool IsAlive { get; protected set; }

        private UnityEvent<int> onDamageTaken = new UnityEvent<int>();
        public UnityEvent<int> OnDamageTaken { get => onDamageTaken; }

        private UnityEvent onDeath = new UnityEvent();
        public UnityEvent OnDeath { get => onDeath; }

        private UnityEvent<Vector2> onPositionChanged = new UnityEvent<Vector2>();
        public UnityEvent<Vector2> OnPositionChanged { get => onPositionChanged; }

        protected ActionData actionData;
        protected Vector2Int facingDirection;
        protected new Rigidbody2D rigidbody2D;
        protected Animator2D animator2D;
        protected SpriteRenderer spriteRenderer;
        private Vector3 prevFramePosition;

        private Coroutine flashCoroutine;

        protected virtual void Awake()
        {
            // Initialize components
            rigidbody2D = GetComponent<Rigidbody2D>();
            animator2D = GetComponent<Animator2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            // Setup events
            AddPropertyChangedListener<int>(PropertyName.MaxHealth, (maxHealth) => Health = maxHealth);

            // Initialize properties
            MaxHealth = initialMaxHealth;
            facingDirection = Vector2Int.right;
            IsAlive = true;
        }

        protected virtual void Update()
        {
            // Update position info
            if (transform.position != prevFramePosition) onPositionChanged.Invoke(transform.position);

            // Animate movement
            if (GetMoveDirection() != Vector2Int.zero)
            {
                AnimateMove();
            }
            else
            {
                AnimateIdle();
            }

            // Update sprite flip direction
            spriteRenderer.flipX = facingRight ? facingDirection.x < 0 : facingDirection.x >= 0;

            prevFramePosition = transform.position;
        }

        public virtual void TakeDamage(int damage, Actor source)
        {
            if (!IsAlive) return;

            Health = Mathf.Max(0, Health - damage);
            onDamageTaken.Invoke(damage);
            if (Health <= 0) Die(source);
            else AnimateHurt();
        }

        protected virtual void Die(Actor source)
        {
            onDeath.Invoke();
            IsAlive = false;
        }

        protected virtual void AnimateIdle()
        {
            animator2D.Play(idleAnimation, true);
        }

        protected virtual void AnimateMove()
        {
            facingDirection = GetMoveDirection();
            animator2D.Play(moveAnimation, true);
        }

        protected virtual void AnimateHurt()
        {
            if (flashCoroutine != null) StopCoroutine(flashCoroutine);
            flashCoroutine = StartCoroutine(FlashHurt());
        }

        private IEnumerator FlashHurt()
        {
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = Color.white;
        }

        protected virtual void AnimateAction(Action action)
        {
            facingDirection = GetActionDirection();
            animator2D.Play(action.Animation, false, true);
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
            return new Vector2Int(MathUtils.Sign(mouseDiff.x), MathUtils.Sign(mouseDiff.y));
            // float angle = Vector2.SignedAngle(Vector2.right, mouseDiff) * Mathf.Deg2Rad;
            // float x = Mathf.Cos(angle);
            // float y = Mathf.Sin(angle);
            // int dirX = x > DIRECTION_LOOK_THRESHOLD ? 1 : x < -DIRECTION_LOOK_THRESHOLD ? -1 : 0;
            // int dirY = y > DIRECTION_LOOK_THRESHOLD ? 1 : y < -DIRECTION_LOOK_THRESHOLD ? -1 : 0;
            // return new Vector2Int(dirX, dirY);
        }
    }
}
