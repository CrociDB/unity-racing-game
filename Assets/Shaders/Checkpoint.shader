Shader "Custom/Checkpoint"
{
    Properties
    {
        _Noise ("Noise", 2D) = "white" {}
        _MainTex ("MainTex", 2D) = "white" {}
        _Color ("Color", Color) = (0, 0, 0, 0)
        _Scroll ("Scroll", Vector) = (0, 1, 0, 0)
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _Noise;
            float4 _Noise_ST;

            float4 _Color;
            float4 _Scroll;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed2 scroll1 = i.uv + _Scroll.xy * _Time.x;
                fixed2 scroll2 = 0.3 * i.uv + fixed2(.3, .6) + _Scroll.zw * _Time.x;
                fixed4 dist = smoothstep(0.1, 0.5, tex2D(_Noise, scroll1) * tex2D(_Noise, scroll2));
                fixed4 tex = tex2D(_MainTex, i.uv + dist.xy * .2);

                fixed4 col = _Color * tex;
                col.a = (1.0 - i.uv.y) * 0.8;
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
