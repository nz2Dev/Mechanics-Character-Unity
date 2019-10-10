Shader "Projectors/MyProjector"
{
    Properties
    {
        _Color ("Tint", Color) = (1, 1, 1, 1)
        _ShadowTex ("Texture", 2D) = "white" {}
        _Attenuation ("Attenuation", Int) = 1
        _TestValue ("TestValue", Int) = 1
    }
    SubShader
    {
        Tags {"Queue"="Transparent"}
        
        Pass
        {
            ZWrite Off
			ColorMask RGB
			Blend One One
			Offset -1, -1
        
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 uvProj : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            float4 _Color;
            sampler2D _ShadowTex;
            float4x4 unity_Projector;
            half _Attenuation;
            half _TestValue;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uvProj = mul(unity_Projector, v.vertex);
                o.normal = mul(unity_Projector, v.normal);
                return o;
            }

            fixed4 frag (v2f input) : SV_Target
            {
                float4 uv = input.uvProj;
                
                // divide by distance from projector, so that it would scale each 1 unit size to fill entire frustume
                // by doing so, it will make vertex position in projector's view coordinate (uvProj)
                // to be able to lookup in texture from in [0, 1] range where the value near 0 is left-down most position
                // and value near 1 is top-right most position.
                uv.xy = uv.xy / uv.w; 
            
                fixed4 texColor = tex2D (_ShadowTex, uv);
                fixed4 outColor;
                outColor = _Color * texColor.a; 
//                outColor = _Color;

//                float3 n = input.normal.xyz * 0.5 + 0.5;//normalize(input.normal.xyz);
//                outColor = float4(input.normal.x, input.normal.y, input.normal.z, 1);
//                outColor = float4(n.x, n.y, n.z, 1);

                if (input.normal.z >= 0) {
                    outColor = float4(0, 0, 0, 0);
                }
                
//                if (uv.x < 0 || uv.x > _Max || uv.y < 0 || uv.y > _Max) {
//                    outColor = float4(0, 0, 0, 0);
//                }
                
                return outColor;
                
                // Attenuation
//				float depth = input.uvProj.z; // [-1 (near), 1 (far)]
//				return outColor * clamp(1.0 - abs(depth) + _Attenuation, 0.0, 1.0);
            }
            ENDCG
        }
    }
}
