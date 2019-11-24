using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PPC_PlanetWRimShaderGUI : PPC_PlanetShaderGUI
{
    protected override void DoRimArea(MaterialEditor _editor, MaterialProperty[] _properties)
    {
        base.DoRimArea(_editor, _properties);
        ShowShaderProperty(_editor, _properties, "Outer Color", "_OuterRimColor", "");
        ShowShaderProperty(_editor, _properties, "Outer Radius", "_OuterRimRadius", "");
        ShowShaderProperty(_editor, _properties, "Outer Offset", "_OuterRimOffset", "");
        ShowShaderProperty(_editor, _properties, "Outer Opacity", "_OuterRimOpacity", "");
        ShowShaderProperty(_editor, _properties, "Object Scale", "_ObjectScale", "Used in orthographic mode. Set this to the same value as your GameObject scale.");
    }
}
