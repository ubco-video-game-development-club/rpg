using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Animator2D : MonoBehaviour
{
    public Rigidbody2D Rigidbody2D { get; private set; }

    private bool isLocked;
    private Animation2D prev;
    private UnityAction triggerListener;

    private Coroutine framesTimer;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void AwaitTrigger(UnityAction listener)
    {
        triggerListener = listener;
    }

    public void Play(Animation2D animation, bool looping, bool reset = false)
    {
        if (isLocked && looping) return;
        if (prev == animation && looping) return;

        if (!looping)
        {
            StartCoroutine(LockAnimation(animation, reset));
        }
        else
        {
            prev = animation;
        }

        PlayFrames(animation, looping);
    }

    private void PlayFrames(Animation2D animation, bool looping)
    {
        if (framesTimer != null) StopCoroutine(framesTimer);
        framesTimer = StartCoroutine(PlayFramesTimer(animation, looping));
    }

    private IEnumerator PlayFramesTimer(Animation2D animation, bool looping)
    {
        if (animation.FrameRate == 0 || animation.Frames.Length == 0)
        {
            Debug.LogError("ERROR: Animation either has no frames or 0 frame rate!");
            yield break;
        }

        WaitForSeconds frameDelay = new WaitForSeconds(1f / animation.FrameRate);
        while (true)
        {
            for (int i = 0; i < animation.Frames.Length; i++)
            {
                spriteRenderer.sprite = animation.Frames[i];
                yield return frameDelay;
            }
            if (!looping) break;
        }
    }

    private IEnumerator LockAnimation(Animation2D animation, bool reset)
    {
        isLocked = true;
        yield return new WaitForSeconds(animation.Duration);
        if (reset && prev != null) PlayFrames(prev, true);
        isLocked = false;
    }
}
