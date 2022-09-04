using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Menu : MonoBehaviour
{
    [SerializeField] private float showTime = 0.5f;
    [SerializeField] private float hideTime = 0.1f;

    private Coroutine transitionFade;
    private CanvasGroup canvasGroup;

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetVisible(bool visible)
    {
        SetVisible(visible, visible ? showTime : hideTime);
    }

    public void SetVisible(bool visible, float fadeTime)
    {
        canvasGroup.blocksRaycasts = visible;
        canvasGroup.interactable = visible;

        if (transitionFade != null) StopCoroutine(transitionFade);
        transitionFade = StartCoroutine(TransitionFade(visible, fadeTime));
    }

    private IEnumerator TransitionFade(bool visible, float fadeTime)
    {
        float fadeTarget = visible ? 1f : 0f;
        float fadeTimer = 0f;

        while (fadeTimer <= fadeTime)
        {
            canvasGroup.alpha = Mathf.Lerp(1f - fadeTarget, fadeTarget, fadeTimer / fadeTime);
            fadeTimer += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = fadeTarget;
    }
}
