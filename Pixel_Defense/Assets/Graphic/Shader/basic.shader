Shader "Custom/Sprite" {
    Properties{
        _MainTex("Sprite Texture", 2D) = "white" {}
    }

        SubShader{
            Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }
            LOD 100
           Blend One OneMinusSrcAlpha
            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata_t {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;

                v2f vert(appdata_t v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target {
                    fixed4 col = tex2D(_MainTex, i.uv);
                // One Minus Alpha 블렌딩 모드 적용
                col.rgb = col.a;
                //col.rgb *= col.a;
                return col;
            }
            ENDCG
        }
    }
}
