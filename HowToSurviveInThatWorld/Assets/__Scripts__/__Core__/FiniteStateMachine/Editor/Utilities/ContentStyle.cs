using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ContentStyle
{
    public static Color DarkGray { get; private set; }
    public static Color LightGray { get; private set; }
    public static Color Focused { get; private set; }
    public static Color ZebraDark { get; private set; }
    public static Color ZebraLight { get; private set; }
    public static RectOffset Padding { get; private set; }
    public static RectOffset LeftPadding { get; private set; }
    public static RectOffset Margin { get; private set; }
    public static GUIStyle BoldCentered { get; private set; } 
    public static GUIStyle StateListStyle { get; private set; } 
    public static GUIStyle WithPadding { get; private set; }
    public static GUIStyle WithPaddingAndMargins { get; private set; }

    private static bool _initialised;

    [InitializeOnLoadMethod]
    public static void Initialize()
    {
        if (_initialised) return;

        _initialised = true;
        
        DarkGray = EditorGUIUtility.isProSkin ? new Color(0.283f, 0.283f, 0.283f) : new Color(0.7f, 0.7f, 0.7f);
        LightGray = EditorGUIUtility.isProSkin ? new Color(0.33f, 0.33f, 0.33f) : new Color(0.8f, 0.8f, 0.8f);
        ZebraDark = new Color(0.4f, 0.4f, 0.4f, 0.1f);
        ZebraLight = new Color(0.8f, 0.8f, 0.8f, 0.1f);
        Focused = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        Padding = new RectOffset(5, 5, 5, 5);
        LeftPadding = new RectOffset(10, 0, 0, 0);
        Margin = new RectOffset(8, 8, 8, 8);
        WithPadding = new GUIStyle { padding = Padding };
        WithPaddingAndMargins = new GUIStyle { padding = Padding, margin = Margin };

        //Prepare a modification of the GUIStyleState to feed into the GUIStyle, for the text colour
        GUIStyleState guiStyleStateNormal = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).label.normal;
        //bright text for Professional skin, dark text for Personal skin
        guiStyleStateNormal.textColor = EditorGUIUtility.isProSkin ? new Color(.85f, .85f, .85f) : new Color(0.337f, 0.337f, 0.337f);

        BoldCentered = new GUIStyle { fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter };
        StateListStyle = new GUIStyle
        {
            alignment = TextAnchor.MiddleLeft,
            padding = LeftPadding,
            fontStyle = FontStyle.Bold,
            fontSize = 12,
            margin = Margin,
            normal = guiStyleStateNormal,
        };
    }
}
