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

		/*
		Pass {
			Blend SrcAlpha One 
            ZTest Greater
            Lighting Off
            ZWrite Off
			
            CGPROGRAM  
            #pragma vertex vert  
            #pragma fragment frag  
            #include "UnityCG.cginc"  
              
            struct appdata {  
                float4 vertex : POSITION;  
                float2 texcoord : TEXCOORD0;  
                float4 color:COLOR;  
                float4 normal:NORMAL;  
            };
            struct v2f {  
                float4 pos : SV_POSITION;  
                float4 color:COLOR;  
            };  
			
            v2f vert (appdata v)  
            {  
                v2f o;  
                o.pos = mul(UNITY_MATRIX_MVP,v.vertex);  
                float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));  
                float rim = 1 - saturate(dot(viewDir,v.normal));  
                o.color = float4(0.1,0.4,0.7,1)*pow(rim,1);
                return o;  
            }
			
            float4 frag (v2f i) : COLOR  
            {  
                return i.color;   
            }  
            ENDCG  
        }
		*/
		
		/*
		Pass 
		{
			Name "OUTLINE"
			Tags { "LightMode" = "Always" }
			Cull Front
			Blend One OneMinusDstColor
 
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			struct appdata 
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};
			struct v2f 
			{
				float4 pos : POSITION;
				float4 color : COLOR;
			};
			
			uniform float4 _OutlineColor;
			 
			v2f vert(appdata v) {
				v2f o;
				float4 pos = mul( UNITY_MATRIX_MV, v.vertex);
				float3 normal = mul( (float3x3)UNITY_MATRIX_IT_MV, v.normal);
				pos = pos + float4(normalize(normal),0) * 0.02;
				o.pos = mul(UNITY_MATRIX_P, pos);
				//o.color = _OutlineColor;
				o.color = float4(1, 0, 0, 1);
				return o;
			}
			 
			half4 frag(v2f i) :COLOR 
			{
				return i.color;
			}
			ENDCG		
		}
		*/
		
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
