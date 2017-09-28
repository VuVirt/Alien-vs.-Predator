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
				half4 vertex : POSITION;
				half2 uv : TEXCOORD0;
				half4 color : COLOR;
				half3 normal : NORMAL; 
			};

			struct v2f
			{
				half2 uv : TEXCOORD0;
				half4 vertex : SV_POSITION;
				half3 normal : TEXCOORD1;
				half3 viewDir : TEXCOORD2;
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
			half4 _MainTex_TexelSize;
			half _Coeff;
			half4 _Low;
			half4 _Medium;
			half4 _High;

			half4 frag (v2f i) : SV_Target
			{
				half3 viewDir = normalize(i.viewDir);
				half3 normal = normalize(i.normal);

				half d = min(1, max(0, dot(viewDir, normal)));
				half4 col = d * tex2D(_MainTex, i.uv);
				
				half2 off = half2(0.5f, 0.5f);
				col *= _Coeff; 
				half c2 = _Coeff * 0.5f;
				col += tex2D(_MainTex, i.uv + (off.x / _MainTex_TexelSize.z)) * c2;
				col += tex2D(_MainTex, i.uv - (off.y / _MainTex_TexelSize.w)) * c2;

				half4 pixcol = col; 
				half4 colors[3];
				colors[0] = _Low;
				colors[1] = _Medium;
				colors[2] = _High;

				half f = 3.0f * (_Coeff > 1.0f ? _Coeff : 1.0f);

				half lum = min(1.0f, (pixcol.r + pixcol.g + pixcol.b) / f);
				half ix = (lum < 0.5f) ? 0.0 : 1.0;
				half4 thermal = lerp(colors[ix], colors[ix + 1], (lum - ix * 0.5f) / 0.5);
				return thermal; 
			}
			ENDCG
		}
	}
}
