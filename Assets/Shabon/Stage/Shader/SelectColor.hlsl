void selectColor_float(float value, out float4 result)
{
    float a = value - 0.5;
    a *= 2.0;
    // 段階に整値
    a = floor(a * 10) / 10;
    a = abs(a);
    if(a == 0.9) a = 0.5;
    if(a == 0) a = 0.8;
    result = float4(a, a, a, 1);
}