using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticAnimator2D : Animator2D
{
    [SerializeField] private Animation2D animation2D;

    private void Start()
    {
        Play(animation2D, true);
    }
}
