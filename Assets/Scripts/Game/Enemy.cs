using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class Enemy : Actor
    {
        [SerializeField] private Action[] actions;

        protected override void Awake()
        {
            base.Awake();

            // Initialize actions
            actionData = new ActionData(LayerMask.GetMask("Player"));
            for (int i = 0; i < actions.Length; i++)
            {
                actions[i] = actions[i].GetInstance();
                actions[i].Enabled = true;
            }
        }

        protected void Start()
        {
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
                AnimateAction(action);
            }
        }

        private IEnumerator ActionCooldown(Action action)
        {
            action.Enabled = false;
            yield return new WaitForSeconds(action.Cooldown);
            action.Enabled = true;
        }
    }
}
