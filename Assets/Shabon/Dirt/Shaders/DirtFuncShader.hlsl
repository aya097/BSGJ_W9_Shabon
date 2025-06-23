void dirt_float(float noise, float2 uv, float2 center, float radius , float threshold, out float value)
{
    // 中心からの距離
    float dist = length(uv - center);

    dist -= radius;

    // 距離補正



    // 距離だけノイズを減衰
    noise *= dist;

    // 閾値を超えていれば出力
    value = (noise > threshold) ? 1.0 : 0.0;
}
