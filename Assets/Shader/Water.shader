Shader "CG/Water"
{
    Properties
    {
        _CubeMap("Reflection Cube Map", Cube) = "" {}
        _NoiseScale("Texture Scale", Range(1, 100)) = 10 
        _TimeScale("Time Scale", Range(0.1, 5)) = 3 
        _BumpScale("Bump Scale", Range(0, 0.5)) = 0.05
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM

                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"
                #include "CGUtils.cginc"
                #include "CGRandom.cginc"

                #define DELTA 0.01

                // Declare used properties
                uniform samplerCUBE _CubeMap;
                uniform float _NoiseScale;
                uniform float _TimeScale;
                uniform float _BumpScale;

                struct appdata
                { 
                    float4 vertex   : POSITION;
                    float3 normal   : NORMAL;
                    float4 tangent  : TANGENT;
                    float2 uv       : TEXCOORD0;
                };

                struct v2f
                {
                    float4 pos      : SV_POSITION;
                    //added 
                    float4 posWorld : TEXCOORD1;
                    float2 uv       : TEXCOORD0;
                    float3 normal   : NORMAL;
                    float4 tangent  : TANGENT;
                };

                // Returns the value of a noise function simulating water, at coordinates uv and time t
                float waterNoise(float2 uv, float t)
                {
                    // Your implementation 
                    return perlin3d(float3(0.5 * uv.x, 0.5 * uv.y, 0.5 * t)) + 0.5 * perlin3d(float3(uv.x, uv.y, t)) +
                        0.2 * perlin3d(float3(2.0 * uv.x, 2.0 * uv.y, 3.0 * t));
                    
                   
                }

                // Returns the world-space bump-mapped normal for the given bumpMapData and time t
                float3 getWaterBumpMappedNormal(bumpMapData i, float t)
                {
                    // Your implementation 
                    float f_tag_v = (waterNoise(_NoiseScale * float2(i.uv.x, i.uv.y + i.dv), t) - waterNoise(_NoiseScale * i.uv, t)) / i.dv;
                    float f_tag_u = (waterNoise(_NoiseScale * float2(i.uv.x + i.du, i.uv.y), t) - waterNoise(_NoiseScale * i.uv, t)) / i.du;
                    float3 normal_h = normalize(float3(-f_tag_u * i.bumpScale, -f_tag_v * i.bumpScale, 1));
                    // Converting to world space 
                    float3 b = i.tangent * i.normal;
                    float3 n_world = normal_h.x * i.tangent + normal_h.z * i.normal + normal_h.y * b;
                    return n_world;
                    
                    
                }


                v2f vert (appdata input)
                {
                    v2f output;
                    output.pos = UnityObjectToClipPos(input.vertex);
                    output.uv = input.uv;
                    output.normal = mul(unity_ObjectToWorld, input.normal);
                    output.tangent = mul(unity_ObjectToWorld, input.tangent);
                    float t = _Time.y * _TimeScale;
                    float water_noise = waterNoise(_NoiseScale * input.uv, t) * _BumpScale;
                    float3 updated_vertex = (input.vertex + (water_noise * output.normal));
                    output.pos = UnityObjectToClipPos(updated_vertex);
                    output.posWorld = mul(unity_ObjectToWorld, updated_vertex);

                    return output;
                }

                fixed4 frag(v2f input) : SV_Target
                {
                   
                    bumpMapData bumpMap;
                    bumpMap.normal = normalize(input.normal);
                    bumpMap.tangent = normalize(input.tangent.xyz);
                    bumpMap.uv = input.uv;
                    bumpMap.du = DELTA;
                    bumpMap.dv = DELTA;
                    bumpMap.bumpScale = _BumpScale;

                    float t = _Time.y * _TimeScale;;
                    float water_noise = waterNoise(_NoiseScale * input.uv, t);
                    water_noise = water_noise * 0.5 + 0.5;
                    float3 n = getWaterBumpMappedNormal(bumpMap, t);
                    float4 v = normalize(float4(_WorldSpaceCameraPos - input.posWorld.xyz, 0));
                    float3 r = (2 * dot(v.xyz, n) * n) - v.xyz;
                    float4 reflected_color = texCUBE(_CubeMap, r);
                    float4 color = (1 - max(0, dot(n, v.xyz)) + 0.2) * reflected_color;


                    return color;
                }

            ENDCG
        }
    }
}
