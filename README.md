# My Shader Showcase Project
*Unity BIRP - Version 2021.3.16f1*  

This is my project where I will hold many shaders that I have created & show them as a portfolio/showcase.  
Currently it's still a very WIP project, however it already contains a bunch of shaders.  
I will be making a "Game" out of this where I will show all the shaders I made in here, not sure of the exact idea yet but something like that will do.

## My Personal Notes
#### ***FIX FOR SKYBOX STRETCHING THROUGH SHADER CODE***
```
in vert shader:
o.worldPos = mul(unity_ObjectToWorld,v.vertex);

in frag shader:
float3 worldPos = normalize(i.worldPos);
float arcSineY = asin(worldPos.y)/(PI/2); //PI/2;
float arcTan2X = atan2(worldPos.x,worldPos.z)/TAU;
float2 skyUV = float2(arcTan2X,arcSineY);
```


