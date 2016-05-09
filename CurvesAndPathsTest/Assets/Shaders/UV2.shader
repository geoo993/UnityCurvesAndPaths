Shader ".ShaderExample/UV2" {

	SubShader {


	    Pass {
	        CGPROGRAM
	        #pragma vertex vert
	        #pragma fragment frag

	        // vertex input: position, second UV
	        struct appdata {
	            float4 vertex : POSITION;
	            float4 texcoord1 : TEXCOORD1;
	        };

	        struct v2f {
	            float4 pos : SV_POSITION;
	            float4 uv : TEXCOORD0;
	        };
	        
	        v2f vert (appdata IN) {
	            v2f OUT;
	            OUT.pos = mul( UNITY_MATRIX_MVP, IN.vertex );
	            OUT.uv = float4( IN.texcoord1.xy, 0, 0 );
	            return OUT;
	        }
	        
	        half4 frag( v2f IN ) : SV_Target {
	            half4 col = frac( IN.uv );
	            if (any(saturate(IN.uv) - IN.uv))
	                col.b = 0.5;
	            return col;
	        }
	        ENDCG
	    }


	}
}