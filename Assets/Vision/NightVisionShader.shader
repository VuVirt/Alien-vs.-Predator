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



			half Noise(half2 n, half x)
			{
				n += x;
				return frac(sin(dot(n.xy, half2(12.9898, 78.233))) * 43758.5453);
			}

			half Step1(half2 uv, half n)
			{
				half b = 2.0, c = -12.0;
				return (1.0 / (4.0 + b * 4.0 + abs(c))) * (
					Noise(uv + half2(-1.0, -1.0), n) +
					Noise(uv + half2(0.0, -1.0), n) * b +
					Noise(uv + half2(1.0, -1.0), n) +
					Noise(uv + half2(-1.0, 0.0), n) * b +
					Noise(uv + half2(0.0, 0.0), n) * c +
					Noise(uv + half2(1.0, 0.0), n) * b +
					Noise(uv + half2(-1.0, 1.0), n) +
					Noise(uv + half2(0.0, 1.0), n) * b +
					Noise(uv + half2(1.0, 1.0), n)
					);
			}

			half Step2(half2 uv, half n)
			{
				half b = 2.0, c = 4.0;
				return (1.0 / (4.0 + b * 4.0 + abs(c))) * (
					Step1(uv + half2(-1.0, -1.0), n) +
					Step1(uv + half2(0.0, -1.0), n) * b +
					Step1(uv + half2(1.0, -1.0), n) +
					Step1(uv + half2(-1.0, 0.0), n) * b +
					Step1(uv + half2(0.0, 0.0), n) * c +
					Step1(uv + half2(1.0, 0.0), n) * b +
					Step1(uv + half2(-1.0, 1.0), n) +
					Step1(uv + half2(0.0, 1.0), n) * b +
					Step1(uv + half2(1.0, 1.0), n)
					);
			}

			half Step3BW(half2 uv)
			{
				return Step2(uv, frac(0.25f));
			}

			half4 frag (v2f i) : SV_Target
			{
				half4 pix = tex2D(_MainTex, i.uv);
				half grain = Step3BW(i.uv * half2(192.0, 192.0));

				//// noise simulation
				//half2 t = i.uv;
				//half x = fmod(t.x * t.y * 50000, 100000);
				//x = fmod(x, 13) * fmod(x, 123);
				//half dx = fmod(x, 0.05);
				//half dy = fmod(x, 0.05);
				//half4 c = tex2D(_MainTex, t + half2(dx,dy)) * 0.25;
				//pix -= c;

				pix.rb = max(pix.r - 0.75, 0) * 0.1;
				return lerp(pix, half4(grain.xxx, 1.0), 0.5f);
			}
			ENDCG
		}
	}
}
