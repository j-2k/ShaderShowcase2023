# My Shader Showcase Project
*Unity BIRP - Version 2021.3.16f1*  
THIS PROJECT WILL PROBABLY ONLY LAST FOR THE CURRENT YEAR ITS BEING WORKED ON.  
I plan on having a showcase project every year so this will probably be for the year of 2023 & a new project will come in 2024.

This is my project where I will hold many shaders that I have created & show them as a portfolio/showcase.  
This project will hold 2 showcase projects, I explain them below:  
- First part will showcase the Valorant dance effect from the radiant entertainment system with a bunch of my shaders I created & learned throughout the year for fun. This was mainly to re-learn particle effects & learn new shader techniques.  
- Second part will showcase some secret "quake" stuff let's just say that...

# Showcase Videos

Part 1 |  Part 2
:-------------------------:|:-------------------------:
**Valorant Dance Project** <br><br> [![Valorant Dance Project Thumbnail](https://img.youtube.com/vi/HFGB74eJgD0/0.jpg)](https://www.youtube.com/watch?v=HFGB74eJgD0) | Part 2 Coming soon...


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
- Arcade Kill Effect Video // DONE
- Quake 1 Player Movement Video
- Water Shader Scene Showcase // to be done before the second project a part 1.5-type idea
- Shell Texture Shader Scene Showcase // to be done before the second project a part 1.5-type idea?? maybe this will replace the water shader

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

## Shell Texture Shader (128 Textures)

Finished Shell Texturing project (Submitted for #acerolafurrychallenge)  
Twitter submission with video > [click here](https://x.com/jumaalremeithi/status/1725938324248867175?s=20)

<p float="left" align="middle">
  <img src="https://github.com/j-2k/ShaderShowcase/assets/52252068/02f1718e-4fb2-446d-b7b6-5ba37dbfdf27" alt="Grass Shells" height="" width="49%" title="Grass Shells"/>
  <img src="https://github.com/j-2k/ShaderShowcase/assets/52252068/d749de30-5177-4c06-a864-19242e9d1402" alt="RNGCol Shell" height="" width="49%" title="RNGCol Shell"/> 
</p>

<p float="left" align="middle">
  <img src="https://github.com/j-2k/ShaderShowcase/assets/52252068/1d0c09bc-3f7f-4b2d-afd9-9e665c241f5a" alt="Combined Shell" height="" width="49%" title="Combined Shell"/>
    <img src="https://github.com/j-2k/ShaderShowcase/assets/52252068/44d0b30c-f9aa-494e-a5b6-7a5ae1667a92" alt="Combined Shell" height="" width="49%" title="Final"/>
</p>

<img align="right" width="49%" src="Acerolakindagiga.png" />

# Acerola 🛐
The first like GIGACHAD & thanks to the other random guy :)  
My motivation for the video after seeing this 📈📈📈  
Will post my promised video when its done.  
*twitter post likes on my shell texturing video using the acerolafurrychallenge hashtag*
  
# Picture Dump

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
