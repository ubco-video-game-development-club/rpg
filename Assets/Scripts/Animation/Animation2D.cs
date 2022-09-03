using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Animation2D", menuName = "Animation2D", order = 400)]
public class Animation2D : ScriptableObject
{
    [SerializeField] private int frameRate;
    public int FrameRate { get => frameRate; }

    [SerializeField] private int lockFrames;
    public float LockFrames { get => lockFrames; }

    [SerializeField] private Sprite[] frames;
    public Sprite[] Frames { get => frames; }

    public float Duration { get => (float)frames.Length / frameRate; }

    public float LockDuration { get => (float)lockFrames / frameRate; }
}
