#ifndef CG_RANDOM_INCLUDED
// Upgrade NOTE: excluded shader from DX11, OpenGL ES 2.0 because it uses unsized arrays
#pragma exclude_renderers d3d11 gles
// Upgrade NOTE: excluded shader from DX11 because it uses wrong array syntax (type[size] name)
#pragma exclude_renderers d3d11
#define CG_RANDOM_INCLUDED

// Returns a psuedo-random float between -1 and 1 for a given float c
float random(float c)
{
    return -1.0 + 2.0 * frac(43758.5453123 * sin(c));
}

// Returns a psuedo-random float2 with componenets between -1 and 1 for a given float2 c 
float2 random2(float2 c)
{
    c = float2(dot(c, float2(127.1, 311.7)), dot(c, float2(269.5, 183.3)));

    float2 v = -1.0 + 2.0 * frac(43758.5453123 * sin(c));
    return v;
}

// Returns a psuedo-random float3 with componenets between -1 and 1 for a given float3 c 
float3 random3(float3 c)
{
    float j = 4096.0 * sin(dot(c, float3(17.0, 59.4, 15.0)));
    float3 r;
    r.z = frac(512.0*j);
    j *= .125;
    r.x = frac(512.0*j);
    j *= .125;
    r.y = frac(512.0*j);
    r = -1.0 + 2.0 * r;
    return r.yzx;
}

// Interpolates a given array v of 4 float2 values using bicubic interpolation
// at the given ratio t (a float2 with components between 0 and 1)
//
// [0]=====o==[1]
//         |
//         t
//         |
// [2]=====o==[3]
//
float bicubicInterpolation(float2 v[4], float2 t)
{
    float2 u = t * t * (3.0 - 2.0 * t); // Cubic interpolation

    // Interpolate in the x direction
    float x1 = lerp(v[0], v[1], u.x);
    float x2 = lerp(v[2], v[3], u.x);

    // Interpolate in the y direction and return
    return lerp(x1, x2, u.y);
}

// Interpolates a given array v of 4 float2 values using biquintic interpolation
// at the given ratio t (a float2 with components between 0 and 1)
float biquinticInterpolation(float2 v[4], float2 t)
{
    // Your implementation
    float2 u = 6.0 * pow(t,5) - 15.0 * pow(t, 4) + 10.0 * pow(t,3); // Quintic interpolation
  
    // Interpolate in the x direction
    float x1 = lerp(v[0], v[1], u.x);
    float x2 = lerp(v[2], v[3], u.x);

    // Interpolate in the y direction and return
    return lerp(x1, x2, u.y);
}

// Interpolates a given array v of 8 float3 values using triquintic interpolation
// at the given ratio t (a float3 with components between 0 and 1)
float triquinticInterpolation(float3 v[8], float3 t)
{
    // Your implementation
    
    float3 u = 6.0 * pow(t, 5) - 15.0 * pow(t, 4) + 10.0 * pow(t, 3); // Quintic interpolation
    // Interpolate in the x direction
    float x1 = lerp(v[0], v[1], u.x);
    float x2 = lerp(v[2], v[3], u.x);
    float x3 = lerp(v[4], v[5], u.x);
    float x4 = lerp(v[6], v[7], u.x);

    // Interpolate in the y direction
    float x5 = lerp(x1, x2, u.y);
    float x6 = lerp(x3, x4, u.y);

    // Interpolate in the z direction and return
    return lerp(x5, x6, u.z);
    
}

// Returns the value of a 2D value noise function at the given coordinates c
float value2d(float2 c)
{
    // Your implementation 
    float2 corners[4];
    float2 v[4];
   
    // Finding corners 
    corners[0] = float2(floor(c.x), floor(c.y));
    corners[1] = float2(floor(c.x) + 1 , floor(c.y));
    corners[2] = float2(floor(c.x), floor(c.y) + 1);
    corners[3] = float2(floor(c.x) + 1, floor(c.y) + 1);
    
    //Finding t
    float2 t = float2(frac(c.x),frac(c.y)); 

    // Filling v array 
    for (int i = 0; i < 4; i++) {
        v[i] = random2(corners[i]);
        
    }

    // Using bicubic interpolation to calculate the color
    float color = bicubicInterpolation(v, t);
    return color;
}

// Returns the value of a 2D Perlin noise function at the given coordinates c
float perlin2d(float2 c)
{
    // Your implementation
    float2 corners[4];
    float2 randomCornerVectors[4];
    float2 distanceVectors[4];
    float2 v[4];

    // Finding corners 
    corners[0] = float2(floor(c.x), floor(c.y));
    corners[1] = float2(floor(c.x) + 1, floor(c.y));
    corners[2] = float2(floor(c.x), floor(c.y) + 1);
    corners[3] = float2(floor(c.x) + 1, floor(c.y) + 1);

    //Finding t
    float2 t = float2(frac(c.x), frac(c.y));

    // Filling Corners random vectors array
    for (int i = 0; i < 4; i++) {
        randomCornerVectors[i] =  random2(corners[i]);
    }

    // Filling "Distance" vectors array 
    for (int i = 0; i < 4; i++) {
        distanceVectors[i] = (c - corners[i]);
    }
 
    // Dot product - Filling v array 
    for (int i = 0; i < 4; i++) {
        v[i] = dot(randomCornerVectors[i], distanceVectors[i]) * float2(1.0,1.0);
    }

    float color = bicubicInterpolation(v, t); // between  [-1,1] 

    return color;
}

// Returns the value of a 3D Perlin noise function at the given coordinates c
float perlin3d(float3 c)
{                 

    float3 corners[8];
    float3 randomCornerVectors[8];
    float3 distanceVectors[8];
    float3 v[8];

    // Finding corners 
    corners[0] = float3(floor(c.x), floor(c.y),floor(c.z));
    corners[1] = float3(floor(c.x) + 1, floor(c.y),floor(c.z));
    corners[2] = float3(floor(c.x), floor(c.y) + 1, floor(c.z));
    corners[3] = float3(floor(c.x) + 1, floor(c.y) + 1, floor(c.z));
    corners[4] = float3(floor(c.x), floor(c.y),floor(c.z) + 1);
    corners[5] = float3(floor(c.x) + 1, floor(c.y), floor(c.z) + 1);
    corners[6] = float3(floor(c.x), floor(c.y) + 1, floor(c.z) + 1);
    corners[7] = float3(floor(c.x) + 1, floor(c.y) + 1, floor(c.z) + 1);

    float3 t = float3(frac(c.x), frac(c.y) , frac(c.z));

    // Filling Corners random vectors array
    for (int i = 0; i < 8; i++) {
        randomCornerVectors[i] = random3(corners[i]);
    }

    // Filling "Distance" vectors array 
    for (int i = 0; i < 8; i++) {
        distanceVectors[i] = (c - corners[i]);
    }

    // Dot product - Filling v array 
    for (int i = 0; i < 8; i++) {
        v[i] = dot(randomCornerVectors[i], distanceVectors[i]) * float3(1.0, 1.0, 1.0);
    }
    float color = triquinticInterpolation(v, t); // between  [-1,1] 

    return color;
}


#endif // CG_RANDOM_INCLUDED
