Shader "Custom/Outline"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color("Color", color) = (1.0, 1.0, 1.0, 1.0)
		_Outline ("Outline", Range(0, 1.0)) = 0.08
		_OutlineColor ("Color", color) = (1.0, 1.0, 1.0, 1.0)
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
			}; 
			
			struct v2f
			{
				float4 pos : POSITION;
				float4 col : COLOR;
			};
			
			float _Outline;
			float4 _OutlineColor;
			
			v2f vert (a2v v)
			{
				v2f o;
				float4 pos = mul( UNITY_MATRIX_MV, v.vertex); 
				float3 normal = mul( (float3x3)UNITY_MATRIX_IT_MV, v.normal);  
				//normal.z = -0.4;
				pos = pos + float4(normalize(normal),0) * _Outline;
				o.pos = mul(UNITY_MATRIX_P, pos);
				o.col = _OutlineColor;
				
				return o;
			}
			
			float4 frag (v2f IN) : COLOR
			{
				return IN.col;
			}
			ENDCG
		}
		
		Pass
		{
			Cull Back 
			Lighting On
			Material
			{
				Diffuse [_Color]
			}
			
			SetTexture [_MainTex] {
				constantColor [_Color]
				Combine texture * primary DOUBLE, texture * constant
			}
		}
	}
	
	Fallback "Diffuse"
}
