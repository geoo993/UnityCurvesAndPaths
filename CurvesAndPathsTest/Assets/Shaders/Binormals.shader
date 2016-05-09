Shader ".ShaderExample/Binormals"
{
SubShader {
    Pass {
        Fog { Mode Off }
        CGPROGRAM

        #pragma vertex vert
        #pragma fragment frag

        // vertex input: position, normal, tangent
        struct appdata {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
            float4 tangent : TANGENT;
        };

        struct v2f {
            float4 pos : SV_POSITION;
            float4 color : COLOR;
        };
        
        v2f vert (appdata IN) {
            v2f OUT;
            OUT.pos = mul( UNITY_MATRIX_MVP, IN.vertex );
            // calculate binormal
            float3 binormal = cross( IN.normal, IN.tangent.xyz ) * IN.tangent.w;
            OUT.color.xyz = binormal * 0.5 + 0.5;
            OUT.color.w = 1.0;
            return OUT;
        }
        
        fixed4 frag (v2f IN) : COLOR0 
        { 
        	return IN.color; 
        }
        ENDCG
    }
}
}