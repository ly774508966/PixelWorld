Shader "Craft/Unlit/ColorTexture"
{
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Brightness ("Brightness", Range(1.0, 2.0)) = 1
	}

	SubShader {
		Tags {  "RenderType"="Opaque" }
		LOD 100
		
		Pass {
			ZWrite on  
            ZTest less
			
			CGPROGRAM
			#pragma vertex vert  
			#pragma fragment frag
			#pragma target 3.0
            #include "UnityCG.cginc"
			
			sampler2D _MainTex;
			float4	_Color;
			float _Brightness;
			
			struct appdata {
				float4 vertex : POSITION;
				float3 texcoord : TEXCOORD0;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert(appdata v) 
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.texcoord;
				return o;
			}

			fixed4 frag(v2f i) : COLOR
			{
				fixed4 color = tex2D(_MainTex, i.uv);
				return color * _Brightness * _Color;
			}
			ENDCG
		}
	}
}
