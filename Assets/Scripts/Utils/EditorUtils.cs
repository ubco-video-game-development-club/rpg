using UnityEngine;
using UnityEditor;

public static class EditorUtils
{
    public static string TrimStringToFit(string text, float width, GUIStyle style)
    {
        string s = text;
        Vector2 size = style.CalcSize(new GUIContent(s));
        int count = 0;
        while (size.x > width)
        {
            s = $"{text.Substring(0, text.Length - ++count).Trim()}...";
            size = style.CalcSize(new GUIContent(s));
        }

        return s;
    }

    public static void DrawBox(Rect rect, Color color)
    {
        // Apply global color changes
        Color tempColor = GUI.backgroundColor;
        GUI.backgroundColor = color;

        // Overwrite the default box skin to allow recoloring boxes (default is black, can't be tinted)
        GUIStyle style = new GUIStyle(GUI.skin.box);
        style.normal.background = Texture2D.whiteTexture;

        // Create the box
        GUI.Box(rect, "", style);

        // Revert global color changes
        GUI.backgroundColor = tempColor;
    }

    public static void DrawBorderBox(Rect rect, Color color, int borderWidth, Color borderColor)
    {
        // Draw border box
        DrawBox(rect, borderColor);

        // Draw inner box
        int w = borderWidth;
        Rect innerRect = new Rect(rect.x + w, rect.y + w, rect.width - w * 2, rect.height - w * 2);
        DrawBox(innerRect, color);
    }
}