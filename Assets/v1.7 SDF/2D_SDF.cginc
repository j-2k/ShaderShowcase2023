#ifndef SDF_2D
#define SDF_2D

float2 rotate(float2 samplePosition, float rotation){
    const float PI = 3.14159;
    float angle = rotation * PI * 2 * -1;
    float sine, cosine;
    sincos(angle, sine, cosine);
    return float2(cosine * samplePosition.x + sine * samplePosition.y, cosine * samplePosition.y - sine * samplePosition.x);
}

float circle(float2 samplePos, float radius)
{
    return length(samplePos) - radius;
}

float rectangle(float2 samplePos, float2 halfSize)
{
    float2 edgeDist = abs(samplePos) - halfSize;
    float outsideDist = length(max(edgeDist,0));
    float insideDist = min(max(edgeDist.x,edgeDist.y),0);
    return outsideDist + insideDist;
}

float2 translate(float2 samplePos, float2 offset)
{
    return samplePos - offset;
}

float scene(float2 position) 
{
    float2 circlePosition = translate(position, float2(3,3));
    float sceneDistance = circle(circlePosition, 2);
    return sceneDistance;
}

float2 scale(float2 pos, float scale)
{
    return float2(pos) / scale;
}

float sceneRect(float2 position)
{
    float2 rPos = position;
    rPos = translate(rPos, float2(2,1));
    rPos = rotate(rPos, _Time.y/8);
    float pulse = (sin(_Time.y) + 2)/4;
    rPos = scale(rPos,pulse);
    float sceneDistance = rectangle(rPos,float2(2,4)) * pulse;
    return sceneDistance;
}

#endif