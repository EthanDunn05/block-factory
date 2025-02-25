shader_type spatial;
render_mode blend_mix, depth_draw_opaque, cull_back, diffuse_burley, specular_schlick_ggx;

uniform vec4 albedo : source_color;
uniform sampler2D texture_albedo : source_color, filter_nearest_mipmap_anisotropic, repeat_disable;
uniform ivec2 albedo_texture_size;
uniform float point_size : hint_range(0.1, 128.0, 0.1);

void vertex() {

}

void fragment() {
	// Pixel perfect uvs
	vec2 pixel = UV * vec2(albedo_texture_size);
	vec2 base_uv = (floor(pixel) + 0.5) / vec2(albedo_texture_size);

	vec4 albedo_tex = texture(texture_albedo, base_uv);

	// Vertex Color Use as Albedo: Enabled
	albedo_tex *= COLOR;

	ALBEDO = albedo.rgb * albedo_tex.rgb;
}