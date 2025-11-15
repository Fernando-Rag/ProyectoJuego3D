Shader "Custom/RoundedRectangle"
{
    Properties
    {
        _Color ("Background Color", Color) = (0.1, 0.1, 0.1, 0.9)
        _BorderColor ("Border Color", Color) = (1, 1, 1, 1)
        _Radius ("Corner Radius", Range(0, 0.5)) = 0.1
        _BorderWidth ("Border Width", Range(0, 0.1)) = 0.02
        _EdgeSmoothness ("Edge Smoothness", Range(0.001, 0.1)) = 0.01
    }
    
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off
        
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
            
            float4 _Color;
            float4 _BorderColor;
            float _Radius;
            float _BorderWidth;
            float _EdgeSmoothness;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            float roundedRectangle(float2 uv, float radius)
            {
                // Centrar UV
                uv = uv * 2.0 - 1.0;
                
                // Calcular distancia desde el borde
                float2 d = abs(uv) - (1.0 - radius);
                float dist = length(max(d, 0.0)) + min(max(d.x, d.y), 0.0) - radius;
                
                return dist;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float dist = roundedRectangle(i.uv, _Radius);
                
                // Crear el borde exterior
                float outerAlpha = 1.0 - smoothstep(-_EdgeSmoothness, _EdgeSmoothness, dist);
                
                // Crear el borde interior (para el contorno)
                float innerDist = dist + _BorderWidth;
                float innerAlpha = 1.0 - smoothstep(-_EdgeSmoothness, _EdgeSmoothness, innerDist);
                
                // Mezclar colores
                fixed4 col = _Color;
                
                // Si estamos en la zona del borde, usar color de borde
                if (outerAlpha > 0.0 && innerAlpha < 1.0)
                {
                    col = _BorderColor;
                }
                
                col.a *= outerAlpha;
                
                return col;
            }
            ENDCG
        }
    }
}