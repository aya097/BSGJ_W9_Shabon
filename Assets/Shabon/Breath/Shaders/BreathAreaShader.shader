Shader "Custom/BreathAreaShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainColor("Color" , Color) = (1,1,1,1)
        _WaveNum("WaveNum", Float) = 1
        _WaveSpeed("WaveSpeed", Float) = 1
        _WaveIntensity("WaveIntensity", Float) = 1
        _Edge("Edge", Float) = 0.1
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off	
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainColor;
            float _WaveNum;
            float _WaveSpeed;
            float _WaveIntensity;
            float _Edge;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 col =_MainColor;

                // 両サイドは濃く
                col += pow( abs(i.uv.x - 0.5), 2) * 3 * _MainColor;

                // 波打
                float y = frac(i.uv.y * _WaveNum + sin(_Time.y + i.uv.x * 10) * 0.1);
                float t = frac(_Time.x * _WaveSpeed);
                
                // yとtの差が小さいところだけ発行
                // col += max(step(abs(t - y) , 0.1) , step(1 - abs(t - y), 0.1));
                col += min(1 - (abs(t - y) -0.1) , 1 - (1 - abs(t - y) - 0.1)) * _WaveIntensity;

                
                // 奥に行くほど薄くする
                col.a *= pow(1 - (i.uv.y), 3);

                // 両サイドをぼかす
                col.a *= smoothstep(0, _Edge, 0.5 - abs(i.uv.x - 0.5));

                return col;
            }
            ENDCG
        }
    }
}
