Shader "_TS/TSA/Skybox/3D 6 Sided" 
{
    Properties 
    {
        _Tint ("Tint Color", Color) = (.5, .5, .5, .5)
        [Gamma] _Exposure ("Exposure", Range(0, 8)) = 1.0
        [NoScaleOffset] _MainTex("Main Texture (HDR)", 2D) = "grey" {}
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Background"
            "RenderType"="Background"
            "PreviewType"="Skybox"
        }
        ZWrite Off

        CGINCLUDE
        #include "UnityCG.cginc"

        sampler2D _MainTex;
        half4 _MainTex_HDR;

        half4 _Tint;
        half _Exposure;

        struct appdata_t
        {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        struct v2f
        {
            float4 vertex : SV_POSITION;
            float2 uv : TEXCOORD0;
            UNITY_VERTEX_OUTPUT_STEREO
        };

        v2f vert (appdata_t v)
        {
            v2f o;

            UNITY_SETUP_INSTANCE_ID(v);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
            o.vertex = UnityObjectToClipPos(mul(unity_WorldToObject, _WorldSpaceCameraPos) + v.vertex);
            o.uv = v.uv;

            return o;
        }

        half4 skybox_frag (v2f i, sampler2D _tex, half4 _texDecode)
        {
            half3 c = DecodeHDR(tex2D(_tex, i.uv), _texDecode);
            c = c * _Tint.rgb * unity_ColorSpaceDouble.rgb;
            c *= _Exposure;
            return half4(c, 1);
        }
        ENDCG

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            half4 frag (v2f i) : SV_Target { return skybox_frag(i, _MainTex, _MainTex_HDR); }
            
            ENDCG
        }
    }
}
