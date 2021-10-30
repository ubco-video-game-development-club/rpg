using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class Enemy : Actor
    {
        [SerializeField] private Action[] actions;
        [SerializeField] private float flashDuration = 0.3f;

        private ActionData actionData;
        private YieldInstruction flashDurationInstruction;
        private SpriteRenderer spriteRenderer;

        protected override void Awake()
        {
            base.Awake();

            // Initialize actions
            actionData = new ActionData(LayerMask.GetMask("Player"));
            foreach (Action action in actions)
            {
                action.Enabled = true;
            }

            flashDurationInstruction = new WaitForSeconds(flashDuration);
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected void Start()
        {
            OnDamageTaken.AddListener(FlashRed);
            OnDamageTaken.AddListener((health) => GameManager.LevelingSystem.AddXP(20));
            OnDeath.AddListener(() => GameManager.LevelingSystem.AddXP(100));
        }

        public void SetActionTarget(Vector2 target)
        {
            actionData.target = target;
        }

        public void InvokeAction(int actionId)
        {
            Action action = actions[actionId];
            if (action.Enabled)
            {
                StartCoroutine(ActionCooldown(action));
                actionData.origin = transform.position;
                action.Invoke(actionData);
            }
        }

        private IEnumerator ActionCooldown(Action action)
        {
            action.Enabled = false;
            yield return new WaitForSeconds(action.Cooldown);
            action.Enabled = true;
        }

        private void FlashRed(int health)
        {
            StartCoroutine(FlashRedTimer());
        }

        private IEnumerator FlashRedTimer()
        {
            spriteRenderer.color = Color.red;
            yield return flashDurationInstruction;
            spriteRenderer.color = Color.white;
        }
    }
}
