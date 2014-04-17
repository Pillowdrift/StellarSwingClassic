Shader "Custom/InsideOut"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color("Color", color) = (1.0, 1.0, 1.0, 1.0)
	}
	
	SubShader
	{
		Pass
		{
			Cull Front
			Lighting Off
			ZWrite On
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			struct a2v
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float3 tangent : TANGENT;
				float2 texcoord : TEXCOORD0;
			}; 
			
			struct v2f
			{
				float4 pos : POSITION;
				float4 col : COLOR;
				float2 uv : TEXCOORD0;
			};
			
			float4 _Color;
			float4 _MainTex_ST;
			sampler2D _MainTex;
			
			v2f vert (a2v v)
			{
				v2f o;
				float4 pos = mul( UNITY_MATRIX_MV, v.vertex); 
				float3 normal = mul( (float3x3)UNITY_MATRIX_IT_MV, v.normal);
				o.pos = mul(UNITY_MATRIX_P, pos);
				o.col = _Color;
				o.uv  = TRANSFORM_TEX(v.texcoord, _MainTex); 
				
				return o;
			}
			
			float4 frag (v2f IN) : COLOR
			{
				return tex2D(_MainTex, IN.uv) * IN.col;
			}
			ENDCG
		}
	}
	
	Fallback "Diffuse"
}
