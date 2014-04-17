Shader "Custom/BumpSpecOutline" 
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BumpMap ("Normalmap", 2D) = "bump" {}
		_Shininess ("Shininess", Range (0.03, 1)) = 0.078125
		_Outline ("Outline Level", Range(0, 0.1)) = 0.01
		_OutlineColor ("Outline Color", color) = (1.0, 1.0, 1.0, 1.0)
	}
	
	SubShader {
		Tags { "RenderType" = "Opaque" }
		CGPROGRAM
		#pragma surface surf MobileBlinnPhong exclude_path:prepass nolightmap noforwardadd 

		inline fixed4 LightingMobileBlinnPhong (SurfaceOutput s, fixed3 lightDir, fixed3 halfDir, fixed atten)
		{
			fixed diff = max (0, dot (s.Normal, lightDir));
			fixed nh = max (0, dot (s.Normal, halfDir));
			fixed spec = pow (nh, s.Specular*128) * s.Gloss;
			
			fixed4 c;
			c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * spec) * (atten*2);
			c.a = 0.0;
			return c;
		}

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
		};
		sampler2D _MainTex;
		sampler2D _BumpMap;
		half _Shininess;
	
		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = tex.rgb;
			o.Gloss = tex.a;
			o.Alpha = tex.a;
			o.Specular = _Shininess;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
		}
	
		ENDCG
		
		CGINCLUDE
		#include "UnityCG.cginc"
	
		struct appdata {
			float4 vertex : POSITION;
			float3 normal : NORMAL;
		};

		struct v2f {
			float4 pos : POSITION;
			float4 color : COLOR;
		};
	
		uniform float _Outline;
		uniform float4 _OutlineColor;
	
		v2f vert(appdata v) {
			v2f o;
			o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

			float3 norm   = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);
			float2 offset = TransformViewToProjection(norm.xy);

			o.pos.xy += offset * o.pos.z * _Outline;
			o.color = _OutlineColor;
			return o;
		}
		ENDCG
		
		Pass {
			Name "OUTLINE"
			Tags { "LightMode" = "Always" }
			Cull Front
			ZWrite On
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			half4 frag(v2f i) :COLOR { return i.color; }
			ENDCG
		}
	
    }
	
	Fallback "Diffuse Bumped"
}
