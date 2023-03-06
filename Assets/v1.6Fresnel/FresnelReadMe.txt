Fresnel Shader

(In addition to directional lighting calculations in the scene,
Turn off & on the directional lighting to see difference)

This is a normal simple fresnel shader using world normals of the object & the view dir

taking the dot product of the 2 will produce the needed value of 1 to -1 depending on the view dir

if the view dir & world normal are pointing in the same dir the value is 1 else if it is orthogonal its 0
it is also -1 if the view dir & world dir are opposite (180 degrees)

taking this produced value from the dot product we add it to the emission channel of our shader & get the fresnel effect

currently it is also taking into account the scene lighting disable it to view a better effect of the fresnel shader.



