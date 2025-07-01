void selectColor_float(float value, out float4 result)
{
    float a = value - 0.5;
    a *= 2.0;
    // 段階に整値
    a = floor(a * 10) / 10;
    a = abs(a);
    result = float4(a, a, a, 1);
}