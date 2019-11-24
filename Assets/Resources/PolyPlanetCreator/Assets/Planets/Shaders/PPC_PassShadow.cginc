#include "UnityCG.cginc"

half _LiquidHeight;

struct v2f
{
	V2F_SHADOW_CASTER;
};

v2f vert(appdata_base v)
{
	v2f o;
	
	v.vertex = float4(normalize(v.vertex.xyz) * max(length(v.vertex.xyz), _LiquidHeight * ceil(saturate(_LiquidHeight - 0.99))), 1);
	TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)

	return o;
}

float4 frag(v2f i) : SV_Target
{
	SHADOW_CASTER_FRAGMENT(i)
}