Shader "Custom/hit"
{
    Properties
    {
        _Color("Tint", Color) = (1,1,1,1)
        _FlashAmount("Flash Amount", Range(0, 1)) = 1.0
    }

        SubShader
    {
        Tags
        {
            "Queue" = "Overlay"
            "RenderType" = "Overlay"
        }
        LOD 100

        CGPROGRAM
        #pragma surface surf Lambert

        struct Input
        {
            float3 viewDir;
        };

        fixed4 _Color;
        float _FlashAmount;

        void surf(Input IN, inout SurfaceOutput o)
        {
            // 몬스터가 타격받았을 때 흰색으로 변하도록 처리
            o.Albedo = lerp(o.Albedo, float3(1, 1, 1), _FlashAmount);
            o.Alpha = 1.0; // Alpha는 항상 1로 설정
            o.Emission = _Color.rgb * _FlashAmount;
        }
        ENDCG
    }

        Fallback "Diffuse"
}
