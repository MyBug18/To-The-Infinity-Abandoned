#include "UnityCG.cginc"
#include "PPC_CGPlanet.cginc"
#include "AutoLight.cginc"

fixed4 _LiquidColor;
half _LiquidHeight;
fixed _DarkSide;
float _SpecularHighlight;
fixed4 _SpecularColor;

fixed4 _CoreColor;

fixed4 _RimColor;
float _RimPower;
fixed _RimOpacity;

struct appdata
{
	float4 vertex : POSITION;
	float3 normal : NORMAL;
	fixed3 color : COLOR;
};

struct v2g
{
	float4 pos : SV_POSITION;
	half4 worldNormal : NORMAL;
	fixed4 color : COLOR;
	fixed4 lightDir : TEXCOORD0;
	fixed4 worldNormalW : TEXCOORD1;
	fixed3 viewDir : TEXCOORD2;
	fixed3 vertex : TEXCOORD3;
#if defined (SHADOWS_SCREEN) || defined (SHADOWS_DEPTH) || defined (SHADOWS_CUBE) // if shadows
	SHADOW_COORDS(4)
#endif
};

struct g2f
{
	float4 pos : SV_POSITION;
	fixed4 color : COLOR;
	half2 custom : TEXCOORD0; // NdotL, local vertex distance
	fixed3 viewDir : TEXCOORD1;
#ifdef _POLYLIQUID_ON
	fixed3 worldNormalW : TEXCOORD2;
#else
	fixed4 worldNormalW : TEXCOORD2;
	fixed3 lightDir : TEXCOORD3;
#endif
#if defined (SHADOWS_SCREEN) || defined (SHADOWS_DEPTH) || defined (SHADOWS_CUBE) // if shadows
	SHADOW_COORDS(4)
#endif
};

v2g vert(appdata v)
{
	v2g o;

	fixed liquidHeight = _LiquidHeight * ceil(saturate(_LiquidHeight - 0.99));

	o.worldNormal.xyz = normalize(mul(unity_ObjectToWorld, v.normal));
	o.worldNormal.w = length(v.vertex.xyz);
	
	float3 worldPos = mul(unity_ObjectToWorld, v.vertex);
	o.lightDir = GetLightDirection(worldPos);
	o.viewDir = GetWorldViewDirection(worldPos);
	
	o.worldNormalW.xyz = normalize(mul(unity_ObjectToWorld, v.vertex.xyz));
	o.worldNormalW.w = saturate(dot(o.worldNormalW.xyz, o.lightDir.xyz));

	o.color = fixed4(v.color, liquidHeight - o.worldNormal.w);
	
	o.pos = UnityObjectToClipPos(float4(normalize(v.vertex.xyz) * max(o.worldNormal.w, liquidHeight), 1));
	o.worldNormal.w = max(o.worldNormal.w, liquidHeight);

	o.vertex = mul(unity_ObjectToWorld, normalize(fixed4(v.vertex.xyz, 0.666)));

#if defined (SHADOWS_SCREEN) || defined (SHADOWS_DEPTH) || defined (SHADOWS_CUBE) // if shadows
	TRANSFER_SHADOW(o);
#endif

	return o;
}

[maxvertexcount(3)]
void geom(triangle v2g i[3], inout TriangleStream<g2f> triStream)
{
	g2f o;

	fixed3 lightDir = (i[0].lightDir + i[1].lightDir + i[2].lightDir) * 0.333;
	fixed terrainMask = saturate(3 - sign(i[0].color.w) - sign(i[1].color.w) - sign(i[2].color.w));

	// terrain NdotL
	fixed3 crossNormal = normalize(cross(i[1].vertex - i[0].vertex, i[2].vertex - i[0].vertex));
	o.custom.x = saturate(dot(crossNormal, lightDir));
	
// terrain coloring type
#ifdef _TERRAINCOLORING_AVERAGE
	o.color = fixed4((i[0].color.xyz + i[1].color.xyz + i[2].color.xyz) * 0.333, 1);
#else
	#ifdef _TERRAINCOLORING_MIN
		o.color = fixed4(
			lerp(
				lerp(i[0].color.xyz, i[2].color.xyz, saturate(ceil(i[2].color.w - i[0].color.w))), 
				lerp(i[1].color.xyz, i[2].color.xyz, saturate(ceil(i[2].color.w - i[1].color.w))), 
				saturate(ceil(i[1].color.w - i[0].color.w))),
			1);
	#else
		o.color = fixed4(
			lerp(
				lerp(i[0].color.xyz, i[2].color.xyz, saturate(ceil(i[0].color.w - i[2].color.w))), 
				lerp(i[1].color.xyz, i[2].color.xyz, saturate(ceil(i[1].color.w - i[2].color.w))), 
				saturate(ceil(i[0].color.w - i[1].color.w))),
			1);
	#endif
#endif
	
// poly liquid
#ifdef _POLYLIQUID_ON
	fixed3 viewDir = normalize(i[0].viewDir + i[1].viewDir + i[2].viewDir);
	fixed3 worldNormalW = normalize(i[0].worldNormalW.xyz + i[1].worldNormalW.xyz + i[2].worldNormalW.xyz);

	fixed specular = saturate(dot(reflect(-normalize(lightDir * 2 + viewDir), worldNormalW), worldNormalW));
	specular *= specular;
	specular *= specular;
	specular = pow(specular, _SpecularHighlight);
	// merge specular with water
	o.color = lerp(lerp(_LiquidColor, _SpecularColor, specular), o.color, terrainMask);
	
	// merge terrain NdotL with water NdotL
	o.custom.x = lerp((i[0].worldNormalW.w + i[1].worldNormalW.w + i[2].worldNormalW.w) * 0.333, o.custom.x, terrainMask);
#endif

// outputs
	o.pos = i[0].pos;
	o.viewDir = i[0].viewDir;
	o.custom.y = i[0].worldNormal.w;
#ifdef _POLYLIQUID_ON
	o.worldNormalW = i[0].worldNormalW.xyz;
#else
	o.custom.x = lerp(i[0].worldNormalW.w, o.custom.x, terrainMask);
	o.worldNormalW = half4(i[0].worldNormalW.xyz, terrainMask);
	o.lightDir = i[0].lightDir;
#endif
#if defined (SHADOWS_SCREEN) || defined (SHADOWS_DEPTH) || defined (SHADOWS_CUBE) // if shadows
	o._ShadowCoord = i[0]._ShadowCoord;
#endif
	triStream.Append(o);

	o.pos = i[1].pos;
	o.viewDir = i[1].viewDir;
	o.custom.y = i[1].worldNormal.w;
#ifdef _POLYLIQUID_ON
	o.worldNormalW = i[1].worldNormalW.xyz;
#else
	o.custom.x = lerp(i[1].worldNormalW.w, o.custom.x, terrainMask);
	o.worldNormalW = half4(i[1].worldNormalW.xyz, terrainMask);
	o.lightDir = i[1].lightDir;
#endif
#if defined (SHADOWS_SCREEN) || defined (SHADOWS_DEPTH) || defined (SHADOWS_CUBE) // if shadows
	o._ShadowCoord = i[1]._ShadowCoord;
#endif
	triStream.Append(o);

	o.pos = i[2].pos;
	o.viewDir = i[2].viewDir;
	o.custom.y = i[2].worldNormal.w;
#ifdef _POLYLIQUID_ON
	o.worldNormalW = i[2].worldNormalW.xyz;
#else
	o.custom.x = lerp(i[2].worldNormalW.w, o.custom.x, terrainMask);
	o.worldNormalW = half4(i[2].worldNormalW.xyz, terrainMask);
	o.lightDir = i[2].lightDir;
#endif
#if defined (SHADOWS_SCREEN) || defined (SHADOWS_DEPTH) || defined (SHADOWS_CUBE) // if shadows
	o._ShadowCoord = i[2]._ShadowCoord;
#endif
	triStream.Append(o);
}

fixed4 frag(g2f i) : COLOR
{
	fixed4 color = i.color;
	// merge with water NdotL
	// if no shadows: SHADOW_ATTENUATION(i) = 1.0
	i.custom.x = min(i.custom.x, SHADOW_ATTENUATION(i)) * _DarkSide + 1 - _DarkSide;

	i.worldNormalW.xyz = normalize(i.worldNormalW.xyz);
	fixed innerRim = saturate(1 - dot(i.viewDir, i.worldNormalW.xyz));
	innerRim *= innerRim;
	innerRim = pow(innerRim, _RimPower);

#ifndef _POLYLIQUID_ON
	fixed specular = saturate(dot(reflect(-normalize(i.lightDir * 2 + i.viewDir), i.worldNormalW.xyz), i.worldNormalW.xyz));
	specular *= specular;
	specular *= specular;
	specular = pow(specular, _SpecularHighlight);
	// merge specular with water
	color = lerp(lerp(_LiquidColor, _SpecularColor, specular), color, i.worldNormalW.w);
#endif

	color = lerp(color * i.custom.x, _RimColor, innerRim * _RimOpacity);

	// core
	fixed dist = saturate(i.custom.y * 2 - 1);
	color = lerp(lerp(_CoreColor * 3, _CoreColor, dist), color, dist);

	return color;
}