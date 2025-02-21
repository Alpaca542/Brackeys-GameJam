Shader "Universal Render Pipeline/BlackWhiteWithExclusion"
{
    Properties
    {
        _ExclusionRadius ("Exclusion Radius", Float) = 2.0
        _EdgeSmoothness ("Edge Smoothness", Float) = 0.5
        _MainTex ("Texture", 2D) = "white" {}
        _Surface("__surface", Float) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha // Enables transparency
            ZWrite Off  // Prevents depth issues with transparency
            Cull Off    // Render both sides (optional)

            CGPROGRAM
            #pragma shader_feature_local_fragment _SURFACE_TYPE_TRANSPARENT
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
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _PlayerPosition;
            float _ExclusionRadius;
            float _EdgeSmoothness;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample texture safely
                fixed4 col = tex2D(_MainTex, i.uv);

                // Get distance from player
                float distanceFromPlayer = distance(i.worldPos, _PlayerPosition.xyz);

                // Detect pixels outside valid tile areas (fixes diagonal glitch)
                if (i.uv.x < 0 || i.uv.x > 1 || i.uv.y < 0 || i.uv.y > 1)
                {
                    clip(-1); // Discards invalid pixels (prevents weird completion effect)
                }

                // Smooth transition using `smoothstep`
                float effectFactor = smoothstep(_ExclusionRadius, _ExclusionRadius + _EdgeSmoothness, distanceFromPlayer);

                // Convert to grayscale
                float gray = dot(col.rgb, float3(0.3, 0.59, 0.11));
                col.rgb = lerp(col.rgb, gray.xxx, effectFactor);

                // Ensure transparency by modifying alpha
                //col.a *= effectFactor; // Adjust alpha based on effect

                return col;
            }
            ENDCG
        }

        Pass
        {
            Name "XRMotionVectors"
            Tags { "LightMode" = "XRMotionVectors" }
            ColorMask RGBA

            // Stencil write for obj motion pixels
            Stencil
            {
                WriteMask 1
                Ref 1
                Comp Always
                Pass Replace
            }

            HLSLPROGRAM
            #pragma shader_feature_local _ALPHATEST_ON
            #pragma multi_compile _ LOD_FADE_CROSSFADE
            #pragma shader_feature_local_vertex _ADD_PRECOMPUTED_VELOCITY
            #define APLICATION_SPACE_WARP_MOTION 1

            #include "Packages/com.unity.render-pipelines.universal/Shaders/BakedLitInput.hlsl"
            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ObjectMotionVectors.hlsl"
            ENDHLSL
        }
    }
	fallback "Sprites/Default"
}
