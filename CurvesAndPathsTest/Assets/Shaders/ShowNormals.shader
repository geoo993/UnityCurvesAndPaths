Shader ".ShaderExample/ShowNormals" {

	Properties {
	    _MainTex ("Base (RGB)", 2D) = "white" {}
	    _DepthNormal ("Depth and Normals", 2D) = "white" {}
	}

	// Shader code pasted into all further CGPROGRAM blocks
	CGINCLUDE
	#include "UnityCG.cginc"
	struct v2f {
	    float4 pos : POSITION;
	    float2 uv : TEXCOORD0;
	};

	uniform sampler2D _MainTex;
	uniform sampler2D _DepthNormal;


	v2f vert( appdata_img v ) {
	    v2f o;
	    o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

	    float2 uv = v.texcoord.xy;
	    o.uv = uv;

	    return o;
	}

	half4 frag(v2f i) : COLOR {
	    float depth;
	    float3 norm;
	    int outlineDepth = 0 ;
	    DecodeDepthNormal(tex2D(_DepthNormal, i.uv.xy), depth, norm);

	    if (outlineDepth > 0)
	        return half4(norm.x, norm.y, norm.z, 0);
	    else
	        return half4(tex2D(_MainTex, i.uv).rgb,1);
	}
	ENDCG

	SubShader {
	    Pass {
	        ZTest Always
	        Cull Off
	        ZWrite Off
	        Fog { Mode off }

	        CGPROGRAM
	        #pragma vertex vert
	        #pragma fragment frag
	        ENDCG
	    }
	}
	FallBack off
}