

Shader ".ShaderExample/Tangents" {
	SubShader {
	    Pass {
	        CGPROGRAM
	        #pragma vertex vert
	        #pragma fragment frag

	        // vertex input: position, tangent
	        struct appdata {
	            float4 vertex : POSITION;
	            float4 tangent : TANGENT;
	        };

	        struct v2f {
	            float4 pos : SV_POSITION;
	            fixed4 color : COLOR;
	        };
	        
	        v2f vert (appdata IN) {
	            v2f OUT;
	            OUT.pos = mul( UNITY_MATRIX_MVP, IN.vertex );
	            OUT.color = IN.tangent * 0.5 + 0.5;
	            return OUT;
	        }
	        
	        fixed4 frag (v2f IN) : SV_Target { 
	        	return IN.color; 
	        }
	        ENDCG
	    }
	}
}
