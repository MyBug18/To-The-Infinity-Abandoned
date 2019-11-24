Shader "_TS/PPC/Planet Outer Rim"
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
		_OuterRimColor("Outer Color", Color) = (0.6,0.85,1.0,1.0)
		_OuterRimOffset("Outer Offset", Range(-0.5, 0.5)) = 0.0
		_OuterRimRadius("Outer Radius", Range(0, 2)) = 0.5
		_OuterRimOpacity("Outer Opacity", Range(0.0, 1.0)) = 1.0
		_ObjectScale("Object Scale", Float) = 1
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Geometry"
			"Queue" = "AlphaTest"
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

		Pass // outer rim
		{
			Blend SrcColor One
			Tags { "LightMode" = "ForwardBase" }
			Zwrite Off
			Cull Front

			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			
			#pragma shader_feature _CAMERA_ORTHOGRAPHIC _CAMERA_PERSPECTIVE

			#include "UnityCG.cginc"
			#include "PPC_CGPlanet.cginc"

			fixed4 _OuterRimColor;
			float _OuterRimRadius;
            fixed _OuterRimOpacity;
			fixed _OuterRimOffset;

			#ifdef _CAMERA_ORTHOGRAPHIC
				float _ObjectScale;
			#endif

			struct appdata
			{
				half4 vertex : POSITION;
			};

			struct v2f
			{
				half4 vertex : SV_POSITION;
				#ifdef _CAMERA_PERSPECTIVE
					float rimPos : TEXCOORD0;
				#else
					float3 worldPos : TEXCOORD0;
				#endif
			};

			v2f vert(appdata v)
			{
				v2f o;

                v.vertex.xyz = normalize(v.vertex.xyz);
				float3 worldPos;
				#ifdef _CAMERA_ORTHOGRAPHIC
					float3 objCenterViewPos = UnityObjectToViewPos(fixed4(0,0,0,1));
					worldPos = UnityObjectToViewPos(v.vertex) - objCenterViewPos;
				#endif
				
				float3 worldCenter = mul(unity_ObjectToWorld, fixed4(0,0,0,1));
				v.vertex.xyz *= 1 + _OuterRimRadius - _OuterRimOffset;
				o.vertex = UnityObjectToClipPos(v.vertex);

				worldPos = mul(unity_ObjectToWorld, v.vertex);
				fixed3 viewDir = GetWorldViewDirection(worldPos);
				
				#ifdef _CAMERA_PERSPECTIVE
					float3 rimPos = worldCenter - _WorldSpaceCameraPos;
					o.rimPos = dot(rimPos, viewDir);
					rimPos = viewDir * o.rimPos * sign(-o.rimPos) - rimPos;
					rimPos = mul(unity_WorldToObject, rimPos);
					o.rimPos = dot(rimPos, rimPos);
				#else
					o.worldPos = UnityObjectToViewPos(v.vertex) - objCenterViewPos; // local-ish Position
				#endif

				return o;
			}

			fixed4 frag(v2f i) : COLOR
			{
				#ifdef _CAMERA_PERSPECTIVE
					fixed rim = sqrt(i.rimPos) + _OuterRimOffset;
					rim = saturate(1 - abs(1-rim) / _OuterRimRadius); // i.lightVector
				#else
					float r = sqrt(i.worldPos.x * i.worldPos.x + i.worldPos.y * i.worldPos.y) + _OuterRimOffset * _ObjectScale;
					fixed rim = saturate(1 - abs(r - _ObjectScale) / _OuterRimRadius / _ObjectScale); // izbolsan od PPS
				#endif
				
				return rim * _OuterRimColor * _OuterRimOpacity;
			}

			ENDCG
		}
    }
	Fallback "VertexLit"
    CustomEditor "PPC_PlanetWRimShaderGUI"
}
