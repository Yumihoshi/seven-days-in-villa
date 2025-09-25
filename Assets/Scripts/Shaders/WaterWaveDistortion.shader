Shader "Custom/WaterWaveDistortion2D"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _WaveStrength ("Wave Strength", Range(0,0.1)) = 0.04
        _WaveFrequency ("Wave Frequency", Range(1, 20)) = 8
        _WaveSpeed ("Wave Speed", Range(0, 10)) = 3
        _ColorDistortStrength ("Color Distort Strength", Range(0,0.5)) = 0.23
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _WaveStrength;
            float _WaveFrequency;
            float _WaveSpeed;
            float _ColorDistortStrength;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            float rand(float2 co)
            {
                return frac(sin(dot(co, float2(12.9898,78.233))) * 43758.5453);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float time = _Time.y * _WaveSpeed;

                // 基础波纹扰动
                float2 wave = float2(
                    sin(i.uv.y * _WaveFrequency + time),
                    cos(i.uv.x * _WaveFrequency + time)
                ) * _WaveStrength;

                // RGB通道分离采样，每个通道使用不同扰动
                float2 uvR = i.uv + wave + float2(rand(i.uv*13.1 + time)*_ColorDistortStrength, rand(i.uv*7.3 + time)*_ColorDistortStrength);
                float2 uvG = i.uv + wave + float2(rand(i.uv*23.5 + time)*_ColorDistortStrength, rand(i.uv*19.8 + time)*_ColorDistortStrength);
                float2 uvB = i.uv + wave + float2(rand(i.uv*2.1 + time)*_ColorDistortStrength, rand(i.uv*33.3 + time)*_ColorDistortStrength);

                float r = tex2D(_MainTex, uvR).r;
                float g = tex2D(_MainTex, uvG).g;
                float b = tex2D(_MainTex, uvB).b;

                float3 col = float3(r, g, b);

                // 可进一步施加全局噪声扰动
                float3 noise = (float3(rand(i.uv*41.6+time), rand(i.uv*57.7+time), rand(i.uv*89.3+time)) - 0.5) * 2.0 * _ColorDistortStrength * 0.5;
                col += noise;

                col = saturate(col);
                return fixed4(col, 1.0);
            }
            ENDCG
        }
    }
    FallBack "Sprites/Default"
}