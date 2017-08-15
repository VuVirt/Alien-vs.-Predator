Shader "Hidden/NightVisionShader"
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



			float Noise(float2 n, float x)
			{
				n += x;
				return frac(sin(dot(n.xy, float2(12.9898, 78.233))) * 43758.5453);
			}

			float Step1(float2 uv, float n)
			{
				float b = 2.0, c = -12.0;
				return (1.0 / (4.0 + b * 4.0 + abs(c))) * (
					Noise(uv + float2(-1.0, -1.0), n) +
					Noise(uv + float2(0.0, -1.0), n) * b +
					Noise(uv + float2(1.0, -1.0), n) +
					Noise(uv + float2(-1.0, 0.0), n) * b +
					Noise(uv + float2(0.0, 0.0), n) * c +
					Noise(uv + float2(1.0, 0.0), n) * b +
					Noise(uv + float2(-1.0, 1.0), n) +
					Noise(uv + float2(0.0, 1.0), n) * b +
					Noise(uv + float2(1.0, 1.0), n)
					);
			}

			float Step2(float2 uv, float n)
			{
				float b = 2.0, c = 4.0;
				return (1.0 / (4.0 + b * 4.0 + abs(c))) * (
					Step1(uv + float2(-1.0, -1.0), n) +
					Step1(uv + float2(0.0, -1.0), n) * b +
					Step1(uv + float2(1.0, -1.0), n) +
					Step1(uv + float2(-1.0, 0.0), n) * b +
					Step1(uv + float2(0.0, 0.0), n) * c +
					Step1(uv + float2(1.0, 0.0), n) * b +
					Step1(uv + float2(-1.0, 1.0), n) +
					Step1(uv + float2(0.0, 1.0), n) * b +
					Step1(uv + float2(1.0, 1.0), n)
					);
			}

			float Step3BW(float2 uv)
			{
				return Step2(uv, frac(0.25f));
			}

			fixed4 frag (v2f i) : SV_Target
			{
				float4 pix = tex2D(_MainTex, i.uv);
				float grain = Step3BW(i.uv * float2(192.0, 192.0));

				//// noise simulation
				//float2 t = i.uv;
				//float x = fmod(t.x * t.y * 50000, 100000);
				//x = fmod(x, 13) * fmod(x, 123);
				//float dx = fmod(x, 0.05);
				//float dy = fmod(x, 0.05);
				//float4 c = tex2D(_MainTex, t + float2(dx,dy)) * 0.25;
				//pix -= c;

				pix.rb = max(pix.r - 0.75, 0) * 0.1;
				return lerp(pix, float4(grain.xxx, 1.0), 0.5f);
			}
			ENDCG
		}
	}
}
