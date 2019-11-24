Shader "_TS/Color Picker/Color Hue"
{
	Properties
	{
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		[Toggle]_Vertical("Vertical", Float) = 1
	}
	
	SubShader
	{
	    Pass
		{	
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#pragma shader_feature _VERTICAL_OFF _VERTICAL_ON
			
			#include "UnityCG.cginc"		
			
			struct appdata
			{
			    float4 vertex : POSITION;
			    fixed2 uv : TEXCOORD0;
			};
	
			struct v2f
			{
			    float4 vertex : SV_POSITION;
			    fixed uv : TEXCOORD0;
			};
			
			v2f vert(appdata v)
			{
			    v2f o;

			    o.vertex = UnityObjectToClipPos(v.vertex);
#ifdef _VERTICAL_ON
				o.uv = v.uv.y;
#else
			    o.uv = v.uv.x;
#endif

			    return o;
			}
			
			half4 frag(v2f i) : COLOR
			{
				return lerp(
					lerp(
						fixed4(1,0,0,1),
						lerp(
							fixed4(1,1,0,1),
							lerp(
								fixed4(0,1,0,1),
								lerp(
									fixed4(0,1,1,1),
									lerp(
										fixed4(0,0,1,1),
										fixed4(1,0,1,1),
										saturate(i.uv * 6 - 4)
									), 
									saturate(i.uv * 6 - 3)
								), 
								saturate(i.uv * 6 - 2)
							), 
							saturate(i.uv * 6 - 1)
						),
						saturate(i.uv * 6)
					), 
					fixed4(1,0,0,1), 
					saturate(i.uv * 6 - 5));
			}
			ENDCG
	    }
	}
}

