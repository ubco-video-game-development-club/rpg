using UnityEngine;

public struct ActionData
{
    public ActionData(LayerMask targetMask)
    {
        this.targetMask = targetMask;
    }

    public LayerMask targetMask;
}
