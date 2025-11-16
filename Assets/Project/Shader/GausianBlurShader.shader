Shader "Custom/GausianBlurShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TexelSize ("TexelSize", Vector) = (0,0,0,0)
        _BlurRadius ("Blur Radius", Range(0.5, 10)) = 3
        _Horizontal ("Horizontal Pass", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _TexelSize;
            float _BlurRadius;
            float _Horizontal;

            fixed4 frag(v2f_img i) : SV_Target
            {
                float2 uv = i.uv;

                fixed4 col = fixed4(0,0,0,0);
                float total = 0;

                int kernelRadius = 4;
                float sigma = _BlurRadius;
                float twoSigmaSq = 2.0 * sigma * sigma;

                for (int x = -kernelRadius; x <= kernelRadius; x++)
                {
                    float weight = exp(- (x * x) / twoSigmaSq);
                    float2 offset = _TexelSize.xy * x * (_Horizontal > 0.5 ? 1 : 0) 
                                                + _TexelSize.xy * x * (_Horizontal > 0.5 ? 0 : 1);

                    col += tex2D(_MainTex, uv + offset) * weight;
                    total += weight;
                }
                return col / total;
            }
            ENDCG
        }
    }
}
