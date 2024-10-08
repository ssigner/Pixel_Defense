Shader "Sprites/DefaultWhite"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)
    }
        SubShader
        {
            Tags
            {
                "Queue" = "Transparent"
                "RenderType" = "Transparent"
                "PreviewType" = "Plane"
                "CanUseSpriteAtlas" = "True"
            }
            LOD 100

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile _ PIXELSNAP_ON
                #include "UnityCG.cginc"

                struct appdata_t
                {
                    float4 vertex   : POSITION;
                    float4 color    : COLOR;
                    float2 texcoord : TEXCOORD0;
                };

                struct v2f
                {
                    float4 vertex   : SV_POSITION;
                    fixed4 color : COLOR;
                    half2 texcoord  : TEXCOORD0;
                };

                fixed4 _Color;

                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.texcoord = v.texcoord;
                    o.color = v.color * _Color;
                    #ifdef PIXELSNAP_ON
                    o.vertex = UnityPixelSnap(o.vertex);
                    #endif
                    return o;
                }

                sampler2D _MainTex;

                fixed4 frag(v2f i) : SV_Target
                {
                    // 이 부분에서 색상을 하얗게 만듭니다.
                    return fixed4(1, 1, 1, i.color.a);
                }
                ENDCG
            }
        }
}
