void scale_uv_height_one_float(float2 uv, float2 size, out float2 output)
{
    uv -= 0.5;
    output = float2(uv.x * size.x / size.y, uv.y);
}

void rescale_uv_height_one_float(float2 uv, float2 size, out float2 output)
{
    output = float2(uv.x * size.y / size.x, uv.y);
    output += 0.5;
}

// scaled_uvはスクリーン座標、中心(0,0)
void distort_uv_float(float2 scaled_uv, float width, float radius, float distort, out float3 output)
{
    float r = length(scaled_uv);
    // radius-width以上radius以下の部分を歪ませる
    float condition = smoothstep(radius - width , radius, r);
    condition *= 1 - condition;
    output = float3(scaled_uv.x , scaled_uv.y + condition * distort, condition);
}

