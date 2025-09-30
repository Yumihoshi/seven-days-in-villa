Shader "Custom/AutoRotatingDistort2D"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SwingAngle ("Swing Angle (deg)", Float) = 15
        _SwingSpeed ("Swing Speed", Float) = 2.0
        _DistortStrength ("Distort Strength", Range(0,0.2)) = 0.08
        _BreakStrength ("Break Strength", Range(0,0.3)) = 0.13
        _ColorDistortStrength ("Color Distort Strength", Range(0,1)) = 0.5
        _ColorDistortChance ("Block Distort Chance", Range(0,1)) = 0.2
        _BlockSize ("Block Size", Range(0.01,0.2)) = 0.08
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _SwingAngle;
            float _SwingSpeed;
            float _DistortStrength;
            float _BreakStrength;
            float _ColorDistortStrength;
            float _ColorDistortChance;
            float _BlockSize;

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

            float rand(float2 co)
            {
                return frac(sin(dot(co, float2(12.9898, 78.233))) * 43758.5453123);
            }

            // 颜色扰动（块状变色）
            fixed4 distortColor(fixed4 col, float noise, float strength)
            {
                // 块状偏色、明暗扰动、RGB打乱
                float r = col.r + (noise - 0.5) * strength;
                float g = col.g + (frac(noise * 7.8) - 0.5) * strength;
                float b = col.b + (frac(noise * 2.1) - 0.5) * strength;
                if (noise > 0.7)
                {
                    float t = r; r = g; g = b; b = t;
                }
                if (noise < 0.1)
                {
                    r *= 0.5; g *= 0.5; b *= 0.5;
                }
                if (noise > 0.9)
                {
                    r = saturate(r + 0.4 * strength);
                    g = saturate(g + 0.3 * strength);
                    b = saturate(b + 0.2 * strength);
                }
                return fixed4(saturate(r), saturate(g), saturate(b), col.a);
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 pivot = float2(0.5, 0.0);
                float angle = radians(_SwingAngle) * sin(_Time.y * _SwingSpeed);
                float2 rel = i.uv - pivot;
                float ca = cos(angle);
                float sa = sin(angle);
                float2 rotated = float2(
                    ca * rel.x - sa * rel.y,
                    sa * rel.x + ca * rel.y
                ) + pivot;

                float2 noiseUV = rotated * 10.0 + _Time.y * 2.0;
                float distortX = (rand(noiseUV) - 0.5) * _DistortStrength;
                float distortY = (rand(noiseUV.yx) - 0.5) * _DistortStrength;
                float2 distorted = rotated + float2(distortX, distortY);

                float breakVal = rand(noiseUV * 3.3 + _Time.y * 1.7);
                if (breakVal > 1.0 - _BreakStrength)
                {
                    distorted += float2(
                        (rand(noiseUV * 2.0) - 0.5) * 0.2,
                        (rand(noiseUV.yx * 2.0) - 0.5) * 0.2
                    );
                }

                fixed4 col = tex2D(_MainTex, distorted);

                // 块状变色：用低频噪声
                float2 blockUV = floor(i.uv / _BlockSize) * _BlockSize;
                float blockNoise = rand(blockUV * 13.5 + _Time.y * 0.7);

                if (blockNoise < _ColorDistortChance)
                {
                    col = distortColor(col, blockNoise, _ColorDistortStrength);
                }

                return col;
            }
            ENDCG
        }
    }
    FallBack "Sprites/Default"
}