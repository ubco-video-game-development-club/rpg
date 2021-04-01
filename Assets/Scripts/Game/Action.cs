using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Action", menuName = "Action", order = 60)]
public class Action : ScriptableObject
{
    [SerializeField] private Effect[] effects;

    [SerializeField] private RuntimeAnimatorController animationController;
    public RuntimeAnimatorController AnimationController { get { return animationController; } }

    [SerializeField] private float cooldown = 1f;
    public float Cooldown { get { return cooldown; } }

    private bool enabled = true;
    public bool Enabled
    {
        get { return enabled; }
        set { enabled = value; }
    }

    public void Invoke(ActionData data)
    {
        Debug.Log("Action invoked");
        foreach (Effect effect in effects)
        {
            effect.Invoke(data);
        }
    }
}
