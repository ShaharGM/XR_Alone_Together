#ifndef CG_UTILS_INCLUDED
#define CG_UTILS_INCLUDED

#define PI 3.141592653

// A struct containing all the data needed for bump-mapping
struct bumpMapData
{ 
    float3 normal;       // Mesh surface normal at the point
    float3 tangent;      // Mesh surface tangent at the point
    float2 uv;           // UV coordinates of the point
    sampler2D heightMap; // Heightmap texture to use for bump mapping
    float du;            // Increment size for u partial derivative approximation
    float dv;            // Increment size for v partial derivative approximation
    float bumpScale;     // Bump scaling factor
};


// Receives pos in 3D cartesian coordinates (x, y, z)
// Returns UV coordinates corresponding to pos using spherical texture mapping
float2 getSphericalUV(float3 pos)
{
    // Your implementation
    float r = sqrt(pos.x * pos.x + pos.y * pos.y + pos.z * pos.z);
    float teta = atan2(pos.z, pos.x);
    float phi = acos(pos.y / r);
    float u = 0.5 + teta / (2.0 * PI);
    float v = 1 - phi / PI;
    float2 uv= float2(u, v);
    return uv;
}

// Implements an adjusted version of the Blinn-Phong lighting model
fixed3 blinnPhong(float3 n, float3 v, float3 l, float shininess, fixed4 albedo, fixed4 specularity, float ambientIntensity)
{
    // Your implementation 
    float4 ambient = ambientIntensity * albedo;
    float4 diffuse =max(0, dot(n , l)) * albedo;
    float3 h = normalize((l + v) / 2);
    float4 specular = pow(max(0, dot(n , h)), shininess) * specularity;
    return ambient + diffuse + specular;
}

// Returns the world-space bump-mapped normal for the given bumpMapData
float3 getBumpMappedNormal(bumpMapData i)
{
    // Your implementation
   
     
    float f_tag_u = (tex2D(i.heightMap,i.uv + i.du) - tex2D(i.heightMap,i.uv)) / i.du;
    float f_tag_v = (tex2D(i.heightMap, i.uv + i.dv) - tex2D(i.heightMap, i.uv)) / i.dv;


    float3 normal_h = float3(-f_tag_u * i.bumpScale, -f_tag_v * i.bumpScale, 1);
    normal_h = normalize(normal_h);

    // Converting to world space 
    float3 b = i.tangent * i.normal;
    float3 n_world = i.tangent * normal_h.x + i.normal * normal_h.z + b * normal_h.y;
    return n_world;
}


#endif // CG_UTILS_INCLUDED
