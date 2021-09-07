using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace BehaviourTree
{
    public class AttackNode : IBehaviourTreeNode
    {
        public void Init(Behaviour behaviour)
        {
            behaviour.SetProperty("attack-range", new VariableProperty(VariableProperty.Type.Number));
            behaviour.SetProperty("attack-damage", new VariableProperty(VariableProperty.Type.Number));
            behaviour.SetProperty("attack-cooldown", new VariableProperty(VariableProperty.Type.Number));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, Agent agent)
        {
            Debug.Log("AttackNode");

            if (!agent.HasProperty("target"))
            {
                return NodeStatus.Failure;
            }

            Actor target = (Actor)agent.GetProperty("target");
            Vector2 targetPos = target.transform.position;
            Vector2 selfPos = agent.transform.position;

            // Update attack cooldown
            float cooldownTime = 0;
            if (agent.HasProperty("attack-cooldown-time"))
            {
                cooldownTime = (float)agent.GetProperty("attack-cooldown-time");
                if (cooldownTime > 0)
                {
                    cooldownTime = Mathf.Max(cooldownTime - Time.deltaTime, 0);
                    agent.SetProperty("attack-cooldown-time", cooldownTime);
                }
            }

            // Validate attack
            float attackRange = (float)self.Element.GetProperty("attack-range").GetNumber();
            bool inRange = Vector2.SqrMagnitude(targetPos - selfPos) < attackRange * attackRange;
            if (inRange && cooldownTime <= 0)
            {
                // Reset cooldown
                float cooldown = (float)self.Element.GetProperty("attack-cooldown").GetNumber();
                agent.SetProperty("attack-cooldown-time", cooldown);

                // Apply attack
                int attackDamage = (int)self.Element.GetProperty("attack-damage").GetNumber();
                target.TakeDamage(attackDamage);

                return NodeStatus.Success;
            }

            return NodeStatus.Failure;
        }
    }
}
