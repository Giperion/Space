Shader "Custom/Landspace" {
	Properties{
		_FirstTex("First Tex", 2D) = "white" {}
		_SecondTex("Second Tex", 2D) = "black" {}
		_ThirdTex("Third Tex", 2D) = "white" {}
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 80

		Pass{
			Name "FORWARD"
			Tags{ "LightMode" = "ForwardBase" }
			CGPROGRAM
#pragma vertex vert_surf
#pragma fragment frag_surf
#pragma multi_compile_fwdbase
#pragma multi_compile_fog
#include "HLSLSupport.cginc"
#include "UnityCG.cginc"
#include "Lighting.cginc"
#include "AutoLight.cginc"

			inline float3 LightingLambertVS(float3 normal, float3 lightDir)
			{
				fixed diff = max(0, dot(normal, lightDir));
				return _LightColor0.rgb * diff;
			}

			sampler2D _FirstTex;
			sampler2D _SecondTex;
			sampler2D _ThirdTex;

			struct Input {
				float2 uv_FirstTex;
				fixed4 VertexColor;
			};


			void surf(Input IN, inout SurfaceOutput o) {
					
				half4 final;
				half4 first = tex2D(_FirstTex, IN.uv_FirstTex);
					half4 second = tex2D(_SecondTex, IN.uv_FirstTex);
					half4 third = tex2D(_ThirdTex, IN.uv_FirstTex);

					half4 PreFinal = lerp(first, second, IN.VertexColor.b);
					final = lerp(PreFinal, third, IN.VertexColor.r);

				o.Albedo = final.rgb;
				o.Alpha = first.a;
			}
			struct v2f_surf {
				float4 pos : SV_POSITION;
				float2 pack0 : TEXCOORD0;
#ifdef LIGHTMAP_OFF
				fixed3 normal : TEXCOORD1;
#endif
#ifndef LIGHTMAP_OFF
				float2 lmap : TEXCOORD2;
#endif
#ifdef LIGHTMAP_OFF
				fixed3 vlight : TEXCOORD2;
#endif
				LIGHTING_COORDS(3, 4)
					UNITY_FOG_COORDS(5)
					fixed4 VertColor : COLOR;
			};


			float4 _FirstTex_ST;

			v2f_surf vert_surf(appdata_full v)
			{
				v2f_surf o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.pack0.xy = TRANSFORM_TEX(v.texcoord, _FirstTex);
#ifndef LIGHTMAP_OFF
				o.lmap.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
#endif
				float3 worldN = UnityObjectToWorldNormal(v.normal);
#ifdef LIGHTMAP_OFF
					o.normal = worldN;
#endif
#ifdef LIGHTMAP_OFF
				o.VertColor = v.color;
				o.vlight = ShadeSH9(float4(worldN, 1.0));
				o.vlight += LightingLambertVS(worldN, _WorldSpaceLightPos0.xyz);

#endif // LIGHTMAP_OFF
				TRANSFER_VERTEX_TO_FRAGMENT(o);
				UNITY_TRANSFER_FOG(o, o.pos);
				return o;
			}


			fixed4 frag_surf(v2f_surf IN) : SV_Target
			{
				Input surfIN;
				surfIN.uv_FirstTex = IN.pack0.xy;
				surfIN.VertexColor = IN.VertColor;
				SurfaceOutput o;
				o.Albedo = 0.0;
				o.Emission = 0.0;
				o.Specular = 0.0;
				o.Alpha = 0.0;
				o.Gloss = 0.0;
#ifdef LIGHTMAP_OFF
				o.Normal = IN.normal;
#else
				o.Normal = 0;
#endif
				surf(surfIN, o);
				fixed atten = LIGHT_ATTENUATION(IN);
				fixed4 c = 0;
#ifdef LIGHTMAP_OFF
				c.rgb = o.Albedo * IN.vlight * atten;
#endif // LIGHTMAP_OFF
#ifndef LIGHTMAP_OFF
				fixed3 lm = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, IN.lmap.xy));
#ifdef SHADOWS_SCREEN
				c.rgb += o.Albedo * min(lm, atten * 2);
#else
				c.rgb += o.Albedo * lm;
#endif
				c.a = o.Alpha;
#endif // !LIGHTMAP_OFF
				UNITY_APPLY_FOG(IN.fogCoord, c);
				UNITY_OPAQUE_ALPHA(c.a);

				//debug

				return c;
			}

				ENDCG
		}
	}

	FallBack "Mobile/VertexLit"
}
