Shader "Craft/Font/3D Texture Text Shader Simple" 
{
    Properties 
    {
        _MainTex ("Font Texture", 2D) 					= "white" {}
        _Color ("Text Color", Color) 					= (1,1,1,1)
   }
    SubShader 
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Lighting Off Cull Off ZWrite Off Fog { Mode Off }
        Blend SrcAlpha OneMinusSrcAlpha
        Pass 
        {    
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest
            #include "UnityCG.cginc"
            struct appdata_t 
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };
            struct v2f 
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };
            sampler2D _MainTex;
            uniform float4 _Color;
            uniform float4 _MainTex_ST;
			
            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
                return o;
            }
            float4 frag (v2f i) : COLOR
            {
                float4 col = tex2D(_MainTex, i.texcoord);
				col *= _Color;
                return col;
            }
            ENDCG 
        }
    } 
}