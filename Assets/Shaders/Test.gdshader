shader_type spatial;
#include "FastNoiseLite.gdshaderinc"

uniform float radius = 600.0f;
uniform float distanceBetweenSamples = 1.0f;
uniform float thingy = 1.0f;

varying vec3 nodePos;

vec3 getDirection(vec3 v0, vec3 v1)
{
	return normalize(v1-v0);
}

float getHeight(fnl_state state, vec3 world_vertex)
{
	return fnlGetNoise3D(state, world_vertex.x + 1.f, world_vertex.y + 1.f, world_vertex.z + 1.f) / 2.f + 0.5f;
}

vec2 getLatLong(vec3 position) {
	float longitude = degrees(atan(position.z, position.x));
	float latitude = degrees(acos(position.y));
	latitude = (latitude - 90.f) * -1.f;
	return vec2(latitude,longitude);
}

vec3 getPosFromLatLong(float lat, float long)
{
	lat = 3.14159f * lat / 180.0f;
    long = 3.14159f * long / 180.0f;

    // adjust position by radians
    lat -= 1.570795765134f; // subtract 90 degrees (in radians)

    // and switch z and y (since z is forward)
    float xPos = (radius) * sin(lat) * cos(long);
    float zPos = (radius) * sin(lat) * sin(long);
    float yPos = (radius) * cos(lat);

	return vec3(xPos,yPos,zPos);
}

// GET DIRECTIONS FROM POINT ON THE FUCKING SPHERE!
vec3 getDirNorth(float lat)
{
	float latRad = radians(lat);
    float x = 0.0f;
    float y = radius * cos(latRad);  // Cos(latitude) gives the y-component
    float z = radius * sin(latRad);  // Sin(latitude) gives the z-component
    return normalize(vec3(x, y, z));
}

vec3 getDirWest(float lon)
{
	float lonRad = radians(lon);
    float x = -radius * sin(lonRad);  // Sin(longitude) gives the x-component
    float y = radius * cos(lonRad);   // Cos(longitude) gives the y-component
    float z = 0.0f;
    return normalize(vec3(x, y, z));
}

vec3 getNormal(vec3 vertex, float adjacentDist, fnl_state state)
{
	vec2 latLong = getLatLong(vertex);

	//vec3 dirWest = getDirWest(latLong.y);
	//vec3 dirNoth = getDirWest(latLong.x);

	//vec3 west_vert = vertex// + dirWest * adjacentDist;
	vec3 west_vert = getPosFromLatLong(latLong.x+1.f, latLong.y);//vec3(1,5,0);//getDirection(vec3(0,0,0),west_vert)*getHeight(state, west_vert);
	//vec3 north_vert = vertex// + dirNoth * adjacentDist;
	vec3 north_vert = getPosFromLatLong(latLong.x, latLong.y+1.f);//vec3(0,0,1);//getDirection(vec3(0,0,0),north_vert)*getHeight(state, north_vert);

	return normalize(
		cross(
			north_vert - vertex,
			west_vert - vertex
		)
	);
}

vec3 to_normalmap(vec3 n){
 n*=vec3(1.0,1.0,-1.0);
 n = n/2.0+0.5;
 n = vec3(n.x,n.z,n.y);
 return n;
}

void vertex() {
	nodePos = NODE_POSITION_WORLD;
}

void fragment() {
    fnl_state state = fnlCreateState(0);
    state.noise_type = FNL_NOISE_OPENSIMPLEX2S;
    state.fractal_type = FNL_FRACTAL_RIDGED;
    state.frequency = 1.f;
    state.octaves = 10;
    state.lacunarity = 2.f;
    state.gain = .5f;
    vec3 world_vertex = (INV_VIEW_MATRIX * vec4(VERTEX, thingy)).xyz;

	// Sample the noise at the current position
    float noise = getHeight(state, world_vertex);

    //ALBEDO = vec3(noise);

	vec3 normal = getNormal(world_vertex, distanceBetweenSamples, state);
	NORMAL_MAP = to_normalmap(normal);
}