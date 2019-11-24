Shader "_TS/Color Picker/Color With Edge"
{ 
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
    }
 
    SubShader
    {
	    Pass
		{	
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			fixed4 _Color;
			
			struct appdata
			{
			    float4 vertex : POSITION;
				fixed4 color : COLOR;
			    fixed2 uv : TEXCOORD0;
			};
	
			struct v2f
			{
			    float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				fixed2 uv : TEXCOORD0;
			};
			
			v2f vert(appdata v)
			{
			    v2f o;

			    o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv * 2.4 - 1.2;
				o.color = v.color * _Color;

			    return o;
			}
			
			half4 frag(v2f i) : COLOR
			{
			    return lerp(i.color, 1, floor(max(abs(i.uv.y), abs(i.uv.x))));
			}
			ENDCG
	    }
    } 
}