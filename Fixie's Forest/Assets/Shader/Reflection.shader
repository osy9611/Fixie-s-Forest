Shader "Custom/Reflection" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Normal("Normal",2D)="bump"{}
		_Height("Height",Range(-1,1)) = 0
		_DistortionAmount("DistortionAmount",Range(-1,1)) = 0.1
		_WaterSpeedX("WaterSpeedX",Range(-5,5)) = 1
		_WaterSpeedY("WaterSpeedY",Range(-5,5)) = 1
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf NoLighting

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 2.0

		sampler2D _MainTex;
		sampler2D _Normal;
		struct Input {
			float2 uv_MainTex;
			fixed2 uv_Normal;
			fixed4 screenPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		fixed _Height; //이미지 높이값
		fixed _DistortionAmount; //왜곡 수치

		fixed _WaterSpeedX;	//물이 흐르는 양X
		fixed _WaterSpeedY;	//물이 흐르는 양Y

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutput o) {

			fixed3 Normal = (UnpackNormal(tex2D(_Normal, fixed2(IN.uv_Normal.x+_Time.x*_WaterSpeedX, IN.uv_Normal.y+ _Time.x*_WaterSpeedY)))*2+1.25)*_DistortionAmount;
			fixed3 screenUV = IN.screenPos.xyz / IN.screenPos.w;
			fixed4 c = tex2D(_MainTex, fixed2(screenUV.x+Normal.x, 1 - screenUV.y + _Height+Normal.x+Normal.y)) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}

		fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed attem)
		{
			fixed4 c;
			c.rgb = s.Albedo;
			c.a = s.Alpha;
			return c;
		}
		ENDCG
	}
	FallBack ""
}
