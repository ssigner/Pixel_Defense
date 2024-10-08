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
            // ���Ͱ� Ÿ�ݹ޾��� �� ������� ���ϵ��� ó��
            o.Albedo = lerp(o.Albedo, float3(1, 1, 1), _FlashAmount);
            o.Alpha = 1.0; // Alpha�� �׻� 1�� ����
            o.Emission = _Color.rgb * _FlashAmount;
        }
        ENDCG
    }

        Fallback "Diffuse"
}
