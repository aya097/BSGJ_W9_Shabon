Shader "Custom/ClapWaveShader"
{
    Properties
    {
        _Color("Color", color) = (1, 1, 1, 0)
        _MainTex("Base (RGB)", 2D) = "white" {}
        _MinDist("Min Distance", Range(0.1, 50)) = 10
        _MaxDist("Max Distance", Range(0.1, 50)) = 25
        _TessFactor("Tessellation", Range(1, 50)) = 10 //分割レベル
        _Displacement("Displacement", Range(0, 10.0)) = 0.3 //変位
        _Center("Center", Vector) = (0, 0, 0, 0)
        _Radius("Radius", Range(0, 1)) = 1.0 //中心からの半径
        _Width("Width", Range(0, 0.5)) = 1.0 //波の幅
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma hull hull
            #pragma domain domain

            #include "Tessellation.cginc"
            #include "UnityCG.cginc"

            #define INPUT_PATCH_SIZE 3
            #define OUTPUT_PATCH_SIZE 3

            float _TessFactor;
            float _Displacement;
            float _MinDist;
            float _MaxDist;
            sampler2D _MainTex;
            fixed4 _Color;
            float2 _Center;
            float _Radius;
            float _Width;
            


            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct HsInput
            {
                float4 position : POS;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct HsControlPointOutput
            {
                float3 position : POS;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct HsConstantOutput
            {
                float tessFactor[3] : SV_TessFactor;
                float insideTessFactor : SV_InsideTessFactor;
            };

            struct DsOutput
            {
                float4 position : SV_POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };


            HsInput vert (appdata v)
            {
                HsInput o;
                o.position = float4(v.vertex.xyz, 1.0);
                o.normal = v.normal;
                o.uv = v.uv;
                return o;
            }

            [domain("tri")]
            [partitioning("integer")]
            [outputtopology("triangle_cw")]
            [patchconstantfunc("hullConst")]
            [outputcontrolpoints(OUTPUT_PATCH_SIZE)]
            HsControlPointOutput hull(InputPatch<HsInput, INPUT_PATCH_SIZE> i, uint id : SV_OutputControlPointID)
            {
                HsControlPointOutput o = (HsControlPointOutput)0;
                o.position = i[id].position.xyz;
                o.normal = i[id].normal;
                o.uv = i[id].uv;
                return o;
            }

            HsConstantOutput hullConst(InputPatch<HsInput, INPUT_PATCH_SIZE> i)
            {
                HsConstantOutput o = (HsConstantOutput)0;

                float4 p0 = i[0].position;
                float4 p1 = i[1].position;
                float4 p2 = i[2].position;
                //頂点からカメラまでの距離を計算しテッセレーション係数を距離に応じて計算しなおす　LOD的な？
                float4 tessFactor = UnityDistanceBasedTess(p0, p1, p2, _MinDist, _MaxDist, _TessFactor);

                o.tessFactor[0] = tessFactor.x;
                o.tessFactor[1] = tessFactor.y;
                o.tessFactor[2] = tessFactor.z;
                o.insideTessFactor = tessFactor.w;

                return o;
            }

            [domain("tri")] //分割に利用する形状を指定　"tri" "quad" "isoline"から選択
            DsOutput domain(
                HsConstantOutput hsConst,
                const OutputPatch<HsControlPointOutput, INPUT_PATCH_SIZE> i,
                float3 bary : SV_DomainLocation)
            {
                DsOutput o = (DsOutput)0;

                //新しく出力する各頂点の座標を計算
                float3 f3Position =
                    bary.x * i[0].position +
                    bary.y * i[1].position +
                    bary.z * i[2].position;

                //新しく出力する各頂点の法線を計算
                float3 f3Normal = normalize(
                    bary.x * i[0].normal +
                    bary.y * i[1].normal +
                    bary.z * i[2].normal);

                //新しく出力する各頂点のUV座標を計算
                o.uv =
                    bary.x * i[0].uv +
                    bary.y * i[1].uv +
                    bary.z * i[2].uv;

                
                // f3Position.xyz += f3Normal 
                float2 cord = o.uv - _Center.xy;
                float dist = length(cord);
                float radius = _Radius - _Width;;
                dist = smoothstep(radius, radius + _Width * 0.5, dist) * (1 - smoothstep(radius + _Width * 0.5, radius + _Width, dist));
                f3Position.xyz += f3Normal * _Displacement * dist;


                o.position = UnityObjectToClipPos(float4(f3Position.xyz, 1.0));
                o.normal = f3Normal;

                return o;
            }

            //フラグメントシェーダー
            fixed4 frag(DsOutput i) : SV_Target
            {
                return tex2D(_MainTex, i.uv) * _Color;
            }
            ENDCG
        }
    }
}
