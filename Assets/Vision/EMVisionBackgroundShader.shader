Shader "Hidden/EMVisionBackgroundShader" 
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		//Cull Off ZWrite Off ZTest Always
		//Tags{ "Queue" = "Default" }
		//Blend SrcAlpha OneMinusSrcAlpha

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
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float4 _MainTex_TexelSize;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed2 off = fixed2(1.5, 1.5);
				fixed4 col = tex2D(_MainTex, i.uv) * 0.2;
				col += tex2D(_MainTex, i.uv + (off / _MainTex_TexelSize.z)) * 0.1;
				col += tex2D(_MainTex, i.uv - (off / _MainTex_TexelSize.w)) * 0.1;

				// gray scale 
				float gray = dot(col.rgb, fixed3(0.01, 0.01, 0.01));
				gray = min(0.01f, gray);
				return fixed4(gray, gray, gray, col.a * 0.01);
				//return fixed4(0, 0, 0, 0.1);
			}
			ENDCG
		}
	}
}
