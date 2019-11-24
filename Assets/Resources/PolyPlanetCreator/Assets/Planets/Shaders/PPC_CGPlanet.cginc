///////////////
// LIGHTING
///////////////

float4 _LightDirection;
fixed _AmbientLight;

fixed4 GetLightDirection(half3 worldPos)
{
	fixed4 lightDirection;
	#ifdef _LIGHTING_CENTRAL
		lightDirection = fixed4(normalize(float3(normalize(-worldPos.xy), -0.8)), 1);
	#else 
		#ifdef _LIGHTING_CUSTOM
			_LightDirection.w = max(0, _LightDirection.w);
			lightDirection = fixed4(normalize(-_LightDirection.xyz), _LightDirection.w);
		#else
			half3 fragmentToLightSource = _WorldSpaceLightPos0.xyz - worldPos.xyz;
			lightDirection = fixed4(
				normalize(lerp(_WorldSpaceLightPos0.xyz, fragmentToLightSource, _WorldSpaceLightPos0.w)),
				lerp(1, 1/length(fragmentToLightSource), _WorldSpaceLightPos0.w) );
		#endif
	#endif

	return lightDirection;
}

fixed3 GetWorldViewDirection(float3 worldPos)
{
	#ifdef _CAMERA_ORTHOGRAPHIC
		return mul(UNITY_MATRIX_MV[2].xyz, unity_WorldToObject);
	#else
		return normalize(_WorldSpaceCameraPos - worldPos);
	#endif
}