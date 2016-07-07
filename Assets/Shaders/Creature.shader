Shader "Custom/CurvedWorld_Creature" {
	Properties{
		// Diffuse texture
		_MainTex("Base (RGB)", 2D) = "white" {}
		_TargetTex("Base (RGB)", 2D) = "white" {}
		_BlendTex("Base (RGB)", 2D) = "white" {}
		_Blend("Blend", Float) = 0

	// Degree of curvature
	_Curvature("Curvature", Float) = 0.001
	}

	SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		// Surface shader function is called surf, and vertex preprocessor function is called vert
		// addshadow used to add shadow collector and caster passes following vertex modification
		#pragma surface surf Lambert vertex:vert addshadow

		// Access the shaderlab properties
		uniform sampler2D _MainTex;
		uniform sampler2D _TargetTex;
		uniform sampler2D _BlendTex;
		uniform float _Blend;
		uniform float _Curvature;

		// Basic input structure to the shader function
		// requires only a single set of UV texture mapping coordinates
		struct Input {
			float2 uv_MainTex;
		};

		// This is where the curvature is applied
		void vert(inout appdata_full v)
		{
			// Transform the vertex coordinates from model space into world space
			float4 vv = mul(_Object2World, v.vertex);

			// Now adjust the coordinates to be relative to the camera position
			vv.xyz -= _WorldSpaceCameraPos.xyz;

			// Reduce the y coordinate (i.e. lower the "height") of each vertex based
			// on the square of the distance from the camera in the z axis, multiplied
			// by the chosen curvature factor
			// one axis
			// vv = float4(0.0f, (vv.z * vv.z) * -_Curvature, 0.0f, 0.0f);
			// two
			vv = float4(0.0f, ((vv.z * vv.z) + (vv.x * vv.x)) * -_Curvature, 0.0f, 0.0f);

			// Now apply the offset back to the vertices in model space
			v.vertex += mul(_World2Object, vv);
		}

		// This is just a default surface shader
		void surf(Input IN, inout SurfaceOutput o) {
			half4 m = tex2D(_MainTex, IN.uv_MainTex);
			half4 t = tex2D(_TargetTex, IN.uv_MainTex);
			half4 blend = tex2D(_BlendTex, IN.uv_MainTex);
			float b = 1-blend.r;
			//b = saturate(_Blend / (b - _Blend));
			b = saturate(_Blend/b);
			b = floor(b * 1.1) / 1.1;
			b = b;
			half4 c = lerp(m, t, b);
			o.Albedo = c.rgb;
			//o.Albedo = b;
			o.Alpha = c.a;
		}
		ENDCG
	}
		// FallBack "Diffuse"
}
