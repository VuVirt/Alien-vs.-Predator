Shader "Hidden/EMVisionShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Coeff("_Coeff", Float) = 0.5
		_Low("_Low", Color) = (0.01, 0.65, 0, 1)
		_High("_High", Color) = (0.25, 0.5, 0.25, 1)
	}
	SubShader
	{
		//Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		//Pass
		//{
		//	Blend SrcAlpha OneMinusSrcAlpha, One One
		//	SetTexture[_MainTex]{ combine texture }
		//}

		Pass
		{
			//Blend SrcAlpha OneMinusSrcAlpha, One One
			//SetTexture[_MainTex]{ combine texture }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 normal : TEXCOORD1;
				float3 viewDir : TEXCOORD2;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.viewDir = WorldSpaceViewDir(v.vertex);
				o.normal = v.normal;
				return o;
			}

			sampler2D _MainTex;
			float4 _MainTex_TexelSize;
			float _Coeff;
			fixed4 _Low;
			fixed4 _High;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 colOrg = tex2D(_MainTex, i.uv);

				// filter background
				if (colOrg.r < 0.02)
					return colOrg;


				half d = min(1, max(0, dot(i.viewDir, i.normal)));
				fixed4 col = d * colOrg;

				fixed2 off = fixed2(0.05f, 0.05f); 
				col *= _Coeff; 
				float c2 = _Coeff * 0.5f;
				col += tex2D(_MainTex, i.uv + (off.x / _MainTex_TexelSize.z)) * c2;
				col += tex2D(_MainTex, i.uv - (off.y / _MainTex_TexelSize.w)) * c2;

				float lum = min(3.0f, (col.r + col.g + col.b) / 3.0f);

				float ix = (lum < 0.5f) ? 0.0 : 1.0;

				float xSpace = _MainTex_TexelSize.z * 0.5f;
				float ySpace = _MainTex_TexelSize.w * 0.5f;
				if (frac(i.uv.x * xSpace) <= 0.5f && frac(i.uv.y * ySpace) <= 0.5f)
					return lerp(_High, _Low, (lum - ix * 0.5f) / 0.5); //even line 
				else
					return lerp(_Low, _High, (lum - ix * 0.5f) / 0.5); //even line 
			}

			ENDCG
		}
	}
}
