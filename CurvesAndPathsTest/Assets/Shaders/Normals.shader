Shader ".ShaderExample/Normals" {
	SubShader {
	    Pass {
	        CGPROGRAM
	        #pragma vertex vert
	        #pragma fragment frag

	        // vertex input: position, normal
	        struct appdata {
	            float4 vertex : POSITION;
	            float3 normal : NORMAL;
	        };

	        struct v2f {
	            float4 pos : SV_POSITION;
	            fixed4 color : COLOR;
	        };
	        
	        v2f vert (appdata IN) {
	            v2f OUT;
	            OUT.pos = mul( UNITY_MATRIX_MVP, IN.vertex );
	            OUT.color.xyz = IN.normal * 0.5 + 0.5;
	            OUT.color.w = 1.0;
	            return OUT;
	        }
	        
	        fixed4 frag (v2f IN) : SV_Target 
	        { 
	       		return IN.color; 
	        }
	        ENDCG
	    }
	}
}