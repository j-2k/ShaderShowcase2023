# My Shader Showcase Project
*Unity BIRP - Version 2021.3.16f1*  
THIS PROJECT WILL PROBABLY ONLY LAST FOR THE CURRENT YEAR ITS BEING WORKED ON.  
I plan on having a showcase project every year so this will probably be for the year of 2023 & a new project will come in 2024.

This is my project where I will hold many shaders that I have created & show them as a portfolio/showcase.  
This project will hold 2 showcase projects, I explain them below:  
- First part will showcase the Valorant dance effect from the radiant entertainment system with a bunch of my shaders I created & learned throughout the year for fun. This was mainly to re-learn particle effects & learn new shader techniques.  
- Second part will showcase some secret "quake" stuff let's just say that...



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
- Various shaders relating to the Q1 Movement Sim

# WIP Projects
- Arcade Kill Effect Video
- Quake 1 Player Movement Video

# License?
Please be careful since I'm using ripped models from popular games. This isn't commercial just going to be for testing & making a video for fun out of it.

## My Personal Notes
I know this is late but I realized the formula is written on the bottom of the Wikipedia page for UV maps
insanely useful for spherically wrapped objects that need a sphere based UV map

https://en.wikipedia.org/wiki/UV_mapping
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
  <img src="https://github.com/j-2k/ShaderShowcase/assets/52252068/eb68c2ba-d5dc-4ba1-9ab0-dc99dd69c80f" alt="scene img" height="200" width="40%" title="QSIM SCENE img (6/12/2023)"/>
  <img src="https://github.com/j-2k/ShaderShowcase/assets/52252068/3c0ca444-442c-4d38-ae26-2f44d064394d" alt="ig img" height="200" width="30%" title="QSIM INGAME img (6/12/2023)"/> 
  <img src="https://github.com/j-2k/ShaderShowcase/assets/52252068/578bf20b-7349-49dd-b77e-e402912c379a" alt="vhs filter" height="200" width="20%" title="VHS SHADER/FILTER IDEA"/>
</p>

Progress Images 1 |  Progress Images 2 |  Progress Images 3
:-------------------------:|:-------------------------:|:-------------------------:
![Q1P1](https://github.com/j-2k/ShaderShowcase/assets/52252068/6afaa65e-d77e-4d4a-ad8e-4a7efe9637d3) | ![Screenshot_4](https://github.com/j-2k/ShaderShowcase/assets/52252068/b7983f16-1b00-4bcf-b9f4-6bf0f78bec32)
 | 3
