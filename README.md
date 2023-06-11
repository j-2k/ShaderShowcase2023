# My Shader Showcase Project
*Unity BIRP - Version 2021.3.16f1*  

This is my project where I will hold many shaders that I have created & show them as a portfolio/showcase.  
Currently it's still a very WIP project, however it already contains a bunch of shaders.  
I will be making a "Game" out of this where I will show all the shaders I made in here, not sure of the exact idea yet but something like that will do.

# Shaders currently featured
A lot of these shaders are incomplete but I will add more & finish them up when I prepare a final scene or if I ever need to showcase properly.
- Spherical Wrapping
- Dynamic Decal (Source/Valorant Sprays)
- Stencil Shader (Stencil has an illusion effect)
- Dynamic Moving Normals (Learning)
- Skybox Shader
- Outlines (Using Normals)
- Fresnel
- SDF Testing (Learning Signed Distance Fields)
- Mandelbrot Fractal Shader (Learning Fractals)
- Valorant Arcade Kill Effect (Dance Version)
- More to come later...

# Other WIP Projects
- Quake 1 Player Sim

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

table header col 1 |  table header col 2
:-------------------------:|:-------------------------:
Table Test | Table Test (Image commented here in source >) <!--![vhsimage](https://github.com/j-2k/ShaderShowcase/assets/52252068/578bf20b-7349-49dd-b77e-e402912c379a)-->

<p float="left" align="middle">
  <img src="https://github.com/j-2k/ShaderShowcase/assets/52252068/eb68c2ba-d5dc-4ba1-9ab0-dc99dd69c80f" alt="scene img" height="200" width="40%" />
  <img src="https://github.com/j-2k/ShaderShowcase/assets/52252068/3c0ca444-442c-4d38-ae26-2f44d064394d" alt="ig img" height="200" width="35%" /> 
  <img src="https://github.com/j-2k/ShaderShowcase/assets/52252068/578bf20b-7349-49dd-b77e-e402912c379a" alt="vhs filter" height="200" width="24%" />
</p>
