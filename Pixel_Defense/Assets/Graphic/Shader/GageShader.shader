Shader "Custom/GageShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _XVal("XVal", Range(0, 1)) = 1
        _StartU("_StartU", Range(0, 1)) = 0
        _EndU("_EndU", Range(0, 1)) = 1
    }
        SubShader
        {
            Tags { "Queue" = "Transparent" }
     
            Pass
            {
           


                Blend SrcAlpha OneMinusSrcAlpha // use alpha blending
                CGPROGRAM


                #include "UnityCG.cginc"
                #pragma vertex vert
                #pragma fragment frag
           

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;

                float _XVal;
                float _StartU;
                float _EndU;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                   
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 c= tex2D(_MainTex, i.uv);

                    float x = lerp(_StartU, _EndU, _XVal);

                    if (i.uv.x < x) {
                        return c;
                    }

                    return fixed4(0, 0, 0, 0);
                }
                ENDCG
            }
        }
}
