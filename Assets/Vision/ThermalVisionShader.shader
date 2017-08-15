Shader "Hidden/ThermalVisionShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Coeff("_Coeff", Float) = 0.5
		_Low("_Low", Color) = (0, 0, 0.05, 1)
		_Medium("_Medium", Color) = (1, 0, 0, 1)
		_High("_High", Color) = (1, 1, 0, 1)
	}
	SubShader
	{
		Pass
		{
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

			v2f vert (appdata v)
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
			fixed4 _Medium;
			fixed4 _High;

			fixed4 frag (v2f i) : SV_Target
			{
				float3 viewDir = normalize(i.viewDir);
				float3 normal = normalize(i.normal);

				half d = min(1, max(0, dot(viewDir, normal)));
				fixed4 col = d * tex2D(_MainTex, i.uv);
				
				fixed2 off = fixed2(0.5f, 0.5f);
				col *= _Coeff; 
				float c2 = _Coeff * 0.5f;
				col += tex2D(_MainTex, i.uv + (off.x / _MainTex_TexelSize.z)) * c2;
				col += tex2D(_MainTex, i.uv - (off.y / _MainTex_TexelSize.w)) * c2;

				fixed4 pixcol = col; 
				fixed4 colors[3];
				colors[0] = _Low;
				colors[1] = _Medium;
				colors[2] = _High;

				float f = 3.0f * (_Coeff > 1.0f ? _Coeff : 1.0f);

				float lum = min(1.0f, (pixcol.r + pixcol.g + pixcol.b) / f);
				float ix = (lum < 0.5f) ? 0.0 : 1.0;
				fixed4 thermal = lerp(colors[ix], colors[ix + 1], (lum - ix * 0.5f) / 0.5);
				return thermal; 
			}
			ENDCG
		}
	}
}
