using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EditorNode
{
    public Vector2 position;
    protected Rect displayRect;
    protected bool isSelected = false;

    public EditorNode(Vector2 position)
    {
        this.position = position;
    }

    public abstract void Draw(Vector2 offset = default(Vector2));

    public bool ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0 && displayRect.Contains(e.mousePosition))
                {
                    isSelected = true;
                    GUI.changed = true;
                }
                break;
            case EventType.MouseUp:
                if (e.button == 0)
                {
                    isSelected = false;
                    GUI.changed = true;
                }
                break;
            case EventType.MouseDrag:
                if (isSelected)
                {
                    position += e.delta;
                    GUI.changed = true;
                }
                break;
            default:
                break;
        }

        return isSelected;
    }

    public bool Contains(Vector2 position) => displayRect.Contains(position);
}
