float GetGaussianWeight(float distance)
{
    float dispersion = 32;
    return exp((- distance * distance) / (2 * dispersion * dispersion)) / dispersion;
}

void GaussianBlur_float(float2 uv,UnityTexture2D screenTexture, UnitySamplerState samp, float2 texelSize, float texelInterval, out float4 output){
    float4 color = 0;
    
    for (int j = 0; j < 16; j++) 
    {
        float2 offset = float2(1,0) * texelSize * ((j + 1) * texelInterval - 1) ; //_TexelIntervalでサンプリング距離を調整
        float weight = GetGaussianWeight(j + 1); //ウェイトを計算
        color.rgb += screenTexture.Sample(samp, uv + offset) * weight; //順方向をサンプリング＆重みづけして加算
        color.rgb += screenTexture.Sample(samp, uv - offset) * weight; //逆方向をサンプリング＆重みづけして加算
    }
    for (int j = 0; j < 16; j++) 
    {
        float2 offset = float2(0,1) * texelSize * ((j + 1) * texelInterval - 1) ; //_TexelIntervalでサンプリング距離を調整
        float weight = GetGaussianWeight(j + 1); //ウェイトを計算
        color.rgb += screenTexture.Sample(samp, uv + offset) * weight; //順方向をサンプリング＆重みづけして加算
        color.rgb += screenTexture.Sample(samp, uv - offset) * weight; //逆方向をサンプリング＆重みづけして加算
    }
    output = color / 2;
}