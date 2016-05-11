Shader ".ShaderExample/Cutout Bumped Specular" {

	Properties {
	    _Color ("Main Color", Color) = (1,1,1,1)
	    _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 0)
	    _Shininess ("Shininess", Range (0.01, 1)) = 0.078125
	    _MainTex ("Base (RGB) TransGloss (A)", 2D) = "white" {}
	    _BumpMap ("Normalmap", 2D) = "bump" {}
	    _SpecMap ("Specmap", 2D) = "white" {}
	    _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
	}
	 
	SubShader {
	    Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
	    LOD 400
	   
	CGPROGRAM
	// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it does not contain a surface program or both vertex and fragment programs.
	#pragma exclude_renderers gles
	#pragma surface surf BlinnPhong2 alphatest:_Cutoff
	#pragma exclude_renderers flash
	 
	sampler2D _MainTex;
	sampler2D _BumpMap;
	sampler2D _SpecMap;
	fixed4 _Color;
	half _Shininess;
	 
	struct SurfaceOutput2 {
	    fixed3 Albedo;
	    fixed3 Normal;
	    fixed3 Emission;
	    half Specular;
	    half3 GlossColor;
	    fixed Gloss;
	    fixed Alpha;
	};
	 
	 
	// NOTE: some intricacy in shader compiler on some GLES2.0 platforms (iOS) needs 'viewDir'  'h'
	// to be mediump instead of lowp, otherwise specular highlight becomes too bright.
	inline fixed4 LightingBlinnPhong2 (SurfaceOutput2 s, fixed3 lightDir, half3 viewDir, fixed atten)
	{
	    half3 h = normalize (lightDir + viewDir);
	   
	    fixed diff = max (0, dot (s.Normal, lightDir));
	   
	    float nh = max (0, dot (s.Normal, h));
	    float spec = pow (nh, s.Specular*256.0) * s.GlossColor;
	   
	    fixed4 c;
	    c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * s.Gloss * spec) * (atten * 2);
	    c.a = s.Alpha + _LightColor0.a * _SpecColor.a * spec * atten;
	    return c;
	}
	 
	inline fixed4 LightingBlinnPhong2_PrePass (SurfaceOutput2 s, half4 light)
	{
	    fixed spec = light.a * s.Gloss;
	    fixed4 c;
	    c.rgb = (s.Albedo * light.rgb + light.rgb * s.GlossColor * _SpecColor.rgb * spec);
	    c.a = s.Alpha + spec * _SpecColor.a;
	    return c;
	}
	 
	 
	struct Input {
	    float2 uv_MainTex;
	    float2 uv_BumpMap;
	};
	 
	void surf (Input IN, inout SurfaceOutput2 o) {
	    fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
	    fixed4 spec = tex2D(_SpecMap, IN.uv_MainTex);
	    o.Albedo = tex.rgb * _Color.rgb;
	    o.Gloss = tex.a;
	    o.GlossColor = spec.rgb;
	    o.Alpha = tex.a * _Color.a;
	    o.Specular = _Shininess;
	    o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
	}
	ENDCG
	}
 
	FallBack "Transparent/Cutout/VertexLit"
}
 
 
 