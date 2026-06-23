float4 _EnemyPositions[100];
int _EnemyCount;

void CalculateEnemyShadows_float(
    float2 WorldPos,
    float BaseRadius,
    float Squash,
    float3 SurfaceColor,
    float3 TargetColor,
    float ColorThreshold,
    float2 Offset,
    float ShrinkSpeed,
    out float OutShadow)
{
    // 1. CRITICAL FIX: Move the color check OUTSIDE the loop.
    // SurfaceColor and TargetColor do not change per enemy. Checking this once per pixel
    // saves your GPU from running 100 redundant distance calculations every frame.
    float colorDiff = distance(SurfaceColor, TargetColor);
    float colorMask = step(colorDiff, ColorThreshold);
    
    if (colorMask <= 0.0)
    {
        OutShadow = 0.0;
        return; // Skip processing completely for non-shadow pixels
    }
        
    float combinedShadow = 0.0;

    // 2. Loop only executes if the pixel color matches our target
    for (int i = 0; i < _EnemyCount; i++)
    {
        // Extract position from XY and individual floor distance from W
        float2 enemyFloorPos = _EnemyPositions[i].xy;
        float enemyDistance = _EnemyPositions[i].w;

        // Calculate a unique dynamic radius for this specific enemy
        float localRadius = BaseRadius * saturate(1.0 - (enemyDistance * ShrinkSpeed)) * _EnemyPositions[i].z;
        float localRadiusSq = localRadius * localRadius;

        // Distance and offset math relative to this pixel
        float2 diff = WorldPos - enemyFloorPos;
        diff.x += Offset.x;
        diff.y = (diff.y + Offset.y) * Squash;

        float distSq = dot(diff, diff);

        // Sharp edge evaluation based on this enemy's unique radius
        float isInside = step(distSq, localRadiusSq);

        combinedShadow = max(combinedShadow, isInside);
    }

    OutShadow = combinedShadow;
}