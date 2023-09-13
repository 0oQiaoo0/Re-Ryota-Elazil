Shader "PostProcessEx/PostProcessExShader"
{
    Properties
    {
        _MainTex("_MainTex",2D) = "" {}
        _Indensity("Indensity",Range(0,0.01)) = 0
            _Amplitude("_Amplitude",float) = 1
            _Amount("_Amount",float) = 1
        
        _BlockSize("_BlockSize",float) = 1
        
//        _Indensity("Indensity",Range(0,1) ) = 0
        
    }
    SubShader
    {
        // No culling or depth
//        Cull Off ZWrite Off ZTest Always
 ZTest Always Cull Off ZWrite Off
        
 Tags { "RenderPipeline" = "UniversalPipeline" } 
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

            uniform  sampler2D _MainTex;

            float randomNoise(float x,float y);
            
            float randomNoise(float2 xy)
            {
                return randomNoise(xy.x,xy.y);
            }
            
            float randomNoise(float x,float y)
            {
                return frac(sin(dot(float2(x,y), float2(12.9898, 78.233))) * 43758.5453);
            }

            float _Indensity;
            float _Amplitude;
            float _Amount;
            float _BlockSize;
            
            fixed4 frag (v2f i) : SV_Target
            {
                float _TimeX = _Time.x;
                float _Amplitude = 2.02;
                float _Amount = 1.5;
                   float splitAmout = (1.0 + sin(_TimeX * 6.0)) * 0.5;
    splitAmout *= 1.0 + sin(_TimeX * 16.0) * 0.5;
    splitAmout *= 1.0 + sin(_TimeX * 19.0) * 0.5;
    splitAmout *= 1.0 + sin(_TimeX * 27.0) * 0.5;
    splitAmout = pow(splitAmout, _Amplitude);
    splitAmout *= (0.05 * _Amount);


                float splitAmount = _Indensity * randomNoise(_Time.x , 2);

                // splitAmount = splitAmout;
                half2 block = randomNoise(floor(i.uv * _BlockSize));
                half displaceNoise = pow(block.x, 8.0) * pow(block.x, 3.0);
                fixed4 colr = tex2D(_MainTex, i.uv + float2(splitAmount,0));
                 fixed4 colg = tex2D(_MainTex, i.uv);
                 fixed4 colb = tex2D(_MainTex, i.uv + float2(-splitAmount,0));
            float4 resColor = float4(colr.r,colg.g,colb.b,1);
                // just invert the colors
                // col.rgb = 1 - col.rgb;
half4 noiseColor = resColor;
                
        float _Speed =0.1;
                float noiseX = randomNoise(_TimeX * _Speed + i.uv / float2(-213, 5.53));
float noiseY = randomNoise(_TimeX * _Speed - i.uv / float2(213, -5.53));
float noiseZ = randomNoise(_TimeX * _Speed + i.uv / float2(213, 5.53));

                float _LuminanceJitterThreshold = 0.8;

// colr.rgb += 0.25 * float3(noiseX,noiseY,noiseZ) - 0.125;
                               half luminance = dot(noiseColor.rgb, fixed3(0.22, 0.707, 0.071));
if (randomNoise(float2(_TimeX * _Speed, _TimeX * _Speed)) > _LuminanceJitterThreshold)
{
    noiseColor = float4(luminance, luminance, luminance, luminance);
}

                noiseColor.rgb += 0.25 * float3(noiseX,noiseY,noiseZ) - 0.125;
                float _Fading = 0.1;
                noiseColor = lerp(resColor, noiseColor, _Fading);
       resColor = noiseColor;          

                // return float4(1,1,1,1);
                return resColor;
            }
            ENDCG
        }
    }
}
