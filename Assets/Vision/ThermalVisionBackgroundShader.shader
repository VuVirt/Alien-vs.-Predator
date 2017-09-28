Shader "Hidden/ThermalVisionBackgroundShader" 
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
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
			};

			struct v2f
			{
				half2 uv : TEXCOORD0;
				half4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			half4 _MainTex_TexelSize;

			half4 frag (v2f i) : SV_Target
			{
				half4 col = half4(0.0, 0.0, 0.0, 0.0);
				half2 off = half2(0.3, 0.3);
				col += tex2D(_MainTex, i.uv) * 0.1;
				col += tex2D(_MainTex, i.uv + (off / _MainTex_TexelSize.z)) * 0.1;
				col += tex2D(_MainTex, i.uv - (off / _MainTex_TexelSize.w)) * 0.1;

				// gray scale 
				half gray = dot(col.rgb, half3(0.01, 0.01, 0.01));
				half4 res = half4(gray, gray, gray, col.a);
				return res;
			}
			ENDCG
		}
	}
}
