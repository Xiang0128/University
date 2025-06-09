Shader"UI/Gradient"
{
    Properties
    {
        _ColorTop ("Top Color", Color) = (1,0,0,1)  // Red
        _ColorBottom ("Bottom Color", Color) = (1,1,0,1)  // Yellow
    }
    SubShader
    {
        Tags { "Queue"="Overlay" }
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
    float4 pos : SV_POSITION;
    float2 uv : TEXCOORD0;
};

fixed4 _ColorTop;
fixed4 _ColorBottom;

v2f vert(appdata v)
{
    v2f o;
    o.pos = UnityObjectToClipPos(v.vertex);
    o.uv = v.uv;
    return o;
}

fixed4 frag(v2f i) : SV_Target
{
    return lerp(_ColorBottom, _ColorTop, i.uv.y);
}
            ENDCG
        }
    }
FallBack"UI/Default"
}
