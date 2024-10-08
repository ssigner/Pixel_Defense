Shader "Custom/DiamondPattern" {
    Properties{
        _MainTex("Main Texture", 2D) = "white" {}
        _Progress("Progress", Range(0, 1)) = 0.5
        _DiamondPixelSize("Diamond Pixel Size", Range(1, 100)) = 10
    }

        SubShader{
            Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}

            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata_t {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                    float4 color : COLOR;
                };

                struct v2f {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                    float4 color : COLOR;
                };

                uniform float _Progress;
                uniform float _DiamondPixelSize;
                sampler2D _MainTex;

                v2f vert(appdata_t v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    o.color = v.color;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target {
                    float2 fragCoord = i.uv * _DiamondPixelSize;
                    float xFraction = frac(fragCoord.x);
                    float yFraction = frac(fragCoord.y);
                    float xDistance = abs(xFraction - 0.5);
                    float yDistance = abs(yFraction - 0.5);
                    if (xDistance + yDistance + i.uv.x + i.uv.y > _Progress * 4.0) {
                        discard;
                    }
                    fixed4 texColor = tex2D(_MainTex, i.uv);
                    return texColor * i.color;
                }
                ENDCG
            }
        }
}
