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
}