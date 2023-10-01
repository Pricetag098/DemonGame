Shader "Hidden/Vista/Graph/LoadTexture"
{
	CGINCLUDE
	#pragma vertex vert
	#pragma fragment frag

	#include "UnityCG.cginc"
	#include "../Includes/Math.hlsl"

	struct appdata
	{
		float4 vertex: POSITION;
		float2 uv: TEXCOORD0;
	};

	struct v2f
	{
		float2 uv: TEXCOORD0;
		float4 vertex: SV_POSITION;
		float4 localPos: TEXCOORD1;
	};

	sampler2D _MainTex;

	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = v.uv;
		o.localPos = v.vertex;
		return o;
	}

	ENDCG

	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100
		Pass
		{
			Name "All Channel"
			CGPROGRAM

			int _ChannelIndex;
			
			float4 frag(v2f input): SV_Target
			{
				float4 color = tex2D(_MainTex, input.localPos);
				return color;
			}
			ENDCG
		}
		Pass
		{
			Name "Single Channel"
			CGPROGRAM

			int _ChannelIndex;
			
			float frag(v2f input): SV_Target
			{
				float4 color = tex2D(_MainTex, input.localPos);
				float value = color[_ChannelIndex];
				return value;
			}
			ENDCG
		}

	}
}
