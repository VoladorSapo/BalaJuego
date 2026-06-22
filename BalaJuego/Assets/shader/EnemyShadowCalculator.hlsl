float4 _EnemyPositions[100];
int _EnemyCount;

void CalculateEnemyShadows_float(
    float2 WorldPos, 
    float BaseRadius, 
    float Squash, 
    float2 Offset,
    float ShrinkSpeed, // Added to control enemy shrink rate
    out float OutShadow)
{
    float combinedShadow = 0.0;

    for (int i = 0; i < _EnemyCount; i++)
    {
        // 1. Extract position from XY and individual floor distance from W
        float2 enemyFloorPos = _EnemyPositions[i].xy;
        float enemyDistance = _EnemyPositions[i].w;

        // 2. Calculate a unique dynamic radius for this specific enemy
        float localRadius = BaseRadius * saturate(1.0 - (enemyDistance * ShrinkSpeed));
        float localRadiusSq = localRadius * localRadius;

        // 3. Distance and offset math relative to this pixel
        float2 diff = WorldPos - enemyFloorPos;
        diff.x += Offset.x;
        diff.y = (diff.y + Offset.y) * Squash;

        float distSq = dot(diff, diff);

        // 4. Sharp edge evaluation based on this enemy's unique radius
        float isInside = step(distSq, localRadiusSq);

        combinedShadow = max(combinedShadow, isInside);
    }

    OutShadow = combinedShadow;
}