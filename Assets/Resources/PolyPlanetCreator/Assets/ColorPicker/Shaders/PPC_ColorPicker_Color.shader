Shader "_TS/Color Picker/Color"
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
			};
	
			struct v2f
			{
			    float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
			};
			
			v2f vert(appdata v)
			{
			    v2f o;

			    o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color * _Color;

			    return o;
			}
			
			half4 frag(v2f i) : COLOR
			{
			    return i.color;
			}
			ENDCG
	    }
    } 
}