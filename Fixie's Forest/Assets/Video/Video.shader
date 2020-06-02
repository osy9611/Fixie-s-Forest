Shader "Custom/Video"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
	   Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
	  
	    CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf NoLighting


        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 2.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

		fixed4 _Color;

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex)* _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }

		fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed attem)
		{
			
			return float4(1, 1, 1, 1);
		}
        ENDCG
    }
    FallBack ""
}
