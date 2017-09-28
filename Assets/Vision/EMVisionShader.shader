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
			half4 _MainTex_TexelSize;
			half _Coeff;
			half4 _Low;
			half4 _High;

			half4 frag (v2f i) : SV_Target
			{
				half4 colOrg = tex2D(_MainTex, i.uv);

				// filter background
				if (colOrg.r < 0.02)
					return colOrg;


				half d = min(1, max(0, dot(i.viewDir, i.normal)));
				half4 col = d * colOrg;

				half2 off = half2(0.05f, 0.05f); 
				col *= _Coeff; 
				half c2 = _Coeff * 0.5f;
				col += tex2D(_MainTex, i.uv + (off.x / _MainTex_TexelSize.z)) * c2;
				col += tex2D(_MainTex, i.uv - (off.y / _MainTex_TexelSize.w)) * c2;

				half lum = min(3.0f, (col.r + col.g + col.b) / 3.0f);

				half ix = (lum < 0.5f) ? 0.0 : 1.0;

				half xSpace = _MainTex_TexelSize.z * 0.5f;
				half ySpace = _MainTex_TexelSize.w * 0.5f;
				if (frac(i.uv.x * xSpace) <= 0.5f && frac(i.uv.y * ySpace) <= 0.5f)
					return lerp(_High, _Low, (lum - ix * 0.5f) / 0.5); //even line 
				else
					return lerp(_Low, _High, (lum - ix * 0.5f) / 0.5); //even line 
			}

			ENDCG
		}
	}
}
