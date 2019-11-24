Shader "_TS/PPC/Planet"
{
	Properties
    {
		// [Header(Settings)]
		[KeywordEnum(Perspective, Orthographic)] _Camera("Camera", Float) = 0.0
		[KeywordEnum(Unity, Central, Custom)] _Lighting("Lighting", Float) = 0.0
		_LightDirection("Light Direction", Vector) = (-1,-1,0,1)
        [Toggle]_PolyLiquid("Poly Liquid", Float) = 0

        // [Header(Main)]
        [KeywordEnum(Average, Min, Max)]_TerrainColoring("Terrain Coloring", Float) = 0
        _DarkSide("Dark Side", Range(0.0, 1.0)) = 0.666

        // [Header(Liquid)]
        _LiquidColor("Color", Color) = (1,1,1,1)
        _LiquidHeight("Height", Range(0.99, 1.51)) = 0.99
		_SpecularColor("Specular Color", Color) = (1,1,1,1)
		_SpecularHighlight("Specular Highlight", Range(1.0, 6.0)) = 3.0

        // [Header(Core)]
        _CoreColor("Core Color", Color) = (1,0.8,0.4,1)
        
		// [Header(Rim)]
		_RimColor("Color", Color) = (0.6,0.85,1.0,1.0)
		_RimPower("Power", Range(1.0, 4.0)) = 3.0
		_RimOpacity("Opacity", Range(0.0, 1.0)) = 0.5
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Geometry"
			"Queue" = "Geometry"
        }

        Pass // terrain
        {
			Tags { "LightMode" = "ForwardBase" }

            CGPROGRAM
            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag
            
			#pragma shader_feature _CAMERA_ORTHOGRAPHIC _CAMERA_PERSPECTIVE
			#pragma shader_feature _LIGHTING_UNITY _LIGHTING_CENTRAL _LIGHTING_CUSTOM
			#pragma shader_feature _POLYLIQUID_OFF _POLYLIQUID_ON
			#pragma multi_compile _TERRAINCOLORING_MIN _TERRAINCOLORING_MAX _TERRAINCOLORING_AVERAGE
			#pragma multi_compile_fwdadd_fullshadows

            #include "PPC_PassTerrain.cginc"

            ENDCG
        }

        Pass // fix vertex displacement shadows
        {
            Tags { "LightMode"="ShadowCaster" }
 
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster

            #include "PPC_PassShadow.cginc"
            ENDCG
        }
    }
	Fallback "VertexLit"
    CustomEditor "PPC_PlanetShaderGUI"
}
