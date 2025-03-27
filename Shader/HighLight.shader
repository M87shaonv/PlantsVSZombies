Shader "HighLight"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}  // Texture属性，默认为白色纹理
        [HDR]_OutlineColor_0 ("Outline Color_0", Color) = (1,1,1,1)  // 大范围动态范围的OutLineColor_0属性，默认为白色
        [HDR]_OutlineColor_1 ("Outline Color_1", Color) = (1,1,1,1)  // 大范围动态范围的OutLineColor_1属性，默认为白色
        _OutlineMinAlpha ("Outline Min Alpha", Range(0,0.5)) = 0.1  // 边缘最小alpha值，默认为0.1
        _OutlineMaxAlpha ("Outline Max Alpha", Range(0,2)) = 0.3  // 边缘最大alpha值，默认为0.3
        _GlowSpeed ("Glow Speed", Range(0,10)) = 2  // 光晕速度，默认为2
        _OutLineWidth ("Outline Width", Range(0,0.1)) = 0.01  // 边缘宽度，默认为0.01
        _Overall_Alpha ("_Overall_Alpha", float) = 1  // 总alpha值，默认为1

      }
    SubShader  // 声明SubShader
    {
        Tags { "RenderType"="Opaque" }  // 设置标签为不透明
        LOD 100  // 设置LOD为100

        Pass  // 声明Pass
        {
            Name "OUTLINE"  // 名称为"OUTLINE"
            Tags {"Queue"="Transparent" "RenderType"="Transparent"}  // 设置标签为透明
            Cull Off  // 关闭背面剔除
            ZWrite Off  // 关闭深度写入
            ZTest Always  // 总是通过深度测试
            Blend SrcAlpha OneMinusSrcAlpha  // 设置混合模式

            CGPROGRAM  // 进入CGPROGRAM代码段
            #pragma vertex vert  // 设置顶点函数为vert
            #pragma fragment frag  // 设置片段函数为frag
            #include "UnityCG.cginc"  // 包含Unity标准CG库
            
            // 声明各种变量
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _OutlineMinAlpha, _OutlineMaxAlpha, _OutLineWidth, _GlowSpeed;
            float4 _OutlineColor_0, _OutlineColor_1;
            float _Overall_Alpha;

            // 定义输入结构体
            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
    
            // 定义输出结构体
            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
    
            // 顶点着色器函数
            v2f vert (appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
    
            // 片段着色器函数
            fixed4 frag (v2f i) : SV_Target {
                // 从纹理中采样颜色
                float4 col = tex2D(_MainTex, i.uv);
                float Alpha = col.a;

                // 计算边缘alpha值
                for (int x = -10 ; x < 10; x++)
                {
                    for(int y = -10; y < 10; y++)
                    {
                        float2 offset = (float2(x, y) * _OutLineWidth)/10;
                        Alpha += tex2D(_MainTex, i.uv + offset).a;
                    }
                }
                Alpha /= 300;

                // 对alpha值进行处理
                clip(Alpha - _OutlineMinAlpha);
                Alpha = clamp(Alpha,0,_OutlineMaxAlpha);

                // 计算边缘颜色
                float3 OutLine =  lerp(_OutlineColor_0, _OutlineColor_1, (0.5 * sin(_GlowSpeed * _Time.y + 2*i.uv.x) + 0.5)) * Alpha;

                // 更新颜色与alpha
                col.a += Alpha;
                col.rgb += OutLine;
                col.a *= pow(_Overall_Alpha,3);

                return col;  // 返回颜色
            }
            ENDCG  // 结束CGPROGRAM代码段
        }
    }
}