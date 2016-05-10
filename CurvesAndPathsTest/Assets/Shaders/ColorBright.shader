Shader ".ShaderExample/ColorBright"
{
    Properties {
        _MainTex ("Greyscale (R) Alpha (A)", 2D) = "white" {}
        _ColorRamp ("Colour Palette", 2D) = "gray" {}
    }
 
    SubShader {


        Pass {
            
           
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma fragmentoption ARB_precision_hint_fastest
                #include "UnityCG.cginc"
               
                struct v2f
                {
                    float4  pos : SV_POSITION;
                    float2  uv : TEXCOORD0;
                };
 
                v2f vert (appdata_tan IN)
                {
                    v2f OUT;
                    OUT.pos = mul(UNITY_MATRIX_MVP, IN.vertex);
                    OUT.uv = IN.texcoord.xy;
                    return OUT;
                }
               
                sampler2D _MainTex;
                sampler2D _ColorRamp;
 
                float4 frag(v2f IN) : COLOR
                {
                // SURFACE COLOUR
                    float greyscale = tex2D(_MainTex, IN.uv).rg;
               
                // RESULT
                    float4 result;
//
//                    result.rgb = tex2D(_ColorRamp, float2(greyscale, 0.5)).rgb;
//                    result.a = tex2D(_MainTex, IN.uv).a;
//                    return result;

                    float4 result;
                    result.a = tex2D(_MainTex, IN.uv).a;
                    if (result.a < 0.5)
                         result.rgb = tex2D(_MainTex, IN.uv).rgb;
                   else
                         result.rgb = tex2D(_ColorRamp, float2(greyscale, 0.5)).rgb;
                    return result;

                }

            
            ENDCG

        	}

    	}


     Fallback "Diffuse"
}  