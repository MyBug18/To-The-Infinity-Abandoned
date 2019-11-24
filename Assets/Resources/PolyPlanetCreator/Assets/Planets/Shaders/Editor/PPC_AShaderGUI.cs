using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public abstract class PPC_AShaderGUI : ShaderGUI
{
    protected void SetKeyword(Material _targetMat, string _keyword, bool _state)
    {
        if (_state)
            _targetMat.EnableKeyword(_keyword);
        else
            _targetMat.DisableKeyword(_keyword);
    }

    protected void ShowToggle(Material _targetMat, MaterialEditor _editor, out bool _toggle, string _inspectorName, string _keywordOnName, string _tooltip)
    {
        _toggle = Array.IndexOf(_targetMat.shaderKeywords, _keywordOnName) != -1;
        EditorGUI.BeginChangeCheck();
        _toggle = EditorGUILayout.Toggle(new GUIContent(_inspectorName, _keywordOnName + (_tooltip == "" ? "" : " - " + _tooltip)), _toggle);
        if (EditorGUI.EndChangeCheck())
        {
            _editor.RegisterPropertyChangeUndo(_inspectorName);
            SetKeyword(_targetMat, _keywordOnName, _toggle);
        }
    }

    protected void ShowShaderProperty(MaterialEditor _editor, MaterialProperty[] _properties, string _inspectorName, string _propertyName, string _tooltip)
    {
        _editor.ShaderProperty(FindProperty(_propertyName, _properties), new GUIContent(_inspectorName, _propertyName + (_tooltip == "" ? "" : " - " + _tooltip)));
    }

    protected void ShowTextureSingleLine(MaterialEditor _editor, MaterialProperty[] _properties, string _inspectorName, string _propertyName, string _tooltip)
    {
        _editor.TexturePropertySingleLine(new GUIContent(_inspectorName, _propertyName + (_tooltip == "" ? "" : " - " + _tooltip)), 
            FindProperty(_propertyName, _properties));
        ShowTextureOffset(_editor, _properties, _propertyName);
    }

    protected void ShowTextureSingleLine(MaterialEditor _editor, MaterialProperty[] _properties, string _inspectorName, string _propertyName, string _propertyName1, string _tooltip)
    {
        _editor.TexturePropertySingleLine(new GUIContent(_inspectorName, _propertyName + " & " + _propertyName1 + (_tooltip == "" ? "" : " - " + _tooltip)), 
            FindProperty(_propertyName, _properties), FindProperty(_propertyName1, _properties));
        ShowTextureOffset(_editor, _properties, _propertyName);
    }

    protected void ShowTextureSingleLine(MaterialEditor _editor, MaterialProperty[] _properties, string _inspectorName, string _propertyName, string _propertyName1, string _propertyName2, string _tooltip)
    {
        _editor.TexturePropertySingleLine(new GUIContent(_inspectorName, _propertyName + " & " + _propertyName1 + " & " + _propertyName2 + (_tooltip == "" ? "" : " - " + _tooltip)),
            FindProperty(_propertyName, _properties), FindProperty(_propertyName1, _properties), FindProperty(_propertyName2, _properties));
        ShowTextureOffset(_editor, _properties, _propertyName);
    }

    protected void ShowTextureOffset(MaterialEditor _editor, MaterialProperty[] _properties, string _propertyName)
    {
        MaterialProperty tex = FindProperty(_propertyName, _properties);
        if ((tex.flags & MaterialProperty.PropFlags.NoScaleOffset) == 0)
        {
            EditorGUI.indentLevel++;
            _editor.TextureScaleOffsetProperty(FindProperty(_propertyName, _properties));
            EditorGUI.indentLevel--;
        }
    }
}
