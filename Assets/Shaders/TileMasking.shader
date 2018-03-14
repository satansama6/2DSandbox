
Shader "My Shaders/TileMasking"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_MaskTex("Mask Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		//	This the position of the bottom left pixel of the tile(inside tileset) mapped to UV space.
		//	You assign the offset from a script when you assign the tile sprite to the SpriteRenderer component.
		_UVOffset("UV Offset", Vector) = (0, 0, 0, 0)

		//	This is equal to the number of columns(X) and rows(Y) in the tileset.
		//	You assign the scale from a script when you assign the tile sprite to the SpriteRenderer component.
		_UVScale("UV Scale", Vector) = (0, 0, 0, 0)

		//	The width of a tile inside the mask texture.
		//	The shader assumes tiles are tightly packed(no padding between tiles) in an array layout.
		_MaskTileWidth("Mask Tile Width", Float) = 256

		//	The width of the mask texture
		_MaskTextureWidth("Mask Texture Size", Float) = 1024

		//	Index of the top left tile inside the mask texture.
		_TopLeftMaskTile("Top Left Mask Tile", Float) = 0

		//	Index of the top right tile inside the mask texture.
		_TopRightMaskTile("Top Right Mask Tile", Float) = 0

		//	Index of the bottom left tile inside the mask texture.
		_BottomLeftMaskTile("Bottom Left Mask Tile", Float) = 0

		//	Index of the bottom right tile inside the mask texture.
		_BottomRightMaskTile("Bottom Right Mask Tile", Float) = 0

		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 uv  : TEXCOORD0;		//	Tile UVs.
				half2 nuv  : TEXCOORD1;		//	Normalized tile UVs.
			};
			
			fixed4 _Color;
			half4  _UVScale; //_UVOffset
			UNITY_DEFINE_INSTANCED_PROP(fixed4, _UVOffset);
			UNITY_DEFINE_INSTANCED_PROP(float,_TopLeftMaskTile);
			UNITY_DEFINE_INSTANCED_PROP(float,_TopRightMaskTile);
			UNITY_DEFINE_INSTANCED_PROP(float,_BottomLeftMaskTile);
			UNITY_DEFINE_INSTANCED_PROP(float,_BottomRightMaskTile);

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.uv = IN.uv;
				OUT.nuv = (IN.uv - _UVOffset.xy) * _UVScale.xy;	// Convert UVs from tileset space to tile space.
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex, _MaskTex;
			sampler2D _AlphaTex;
			float _AlphaSplitEnabled;
			float _MaskTileWidth;
			float _MaskTextureWidth;
		//	float _TopLeftMaskTile;
		//	float _TopRightMaskTile;
		//	float _BottomLeftMaskTile;
		//	float _BottomRightMaskTile;

			

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);
				if (_AlphaSplitEnabled)
					color.a = tex2D (_AlphaTex, uv).r;

				return color;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				half maskTileUVSize = _MaskTileWidth / _MaskTextureWidth;
				half2 uvTL = half2((_TopLeftMaskTile + saturate(IN.nuv.x / 0.5)) * maskTileUVSize, saturate(IN.nuv.y - 0.5) / 0.5);
				half2 uvTR = half2((_TopRightMaskTile + saturate(IN.nuv.x - 0.5) / 0.5) * maskTileUVSize, saturate(IN.nuv.y - 0.5) / 0.5);
				half2 uvBL = half2((_BottomLeftMaskTile + saturate(IN.nuv.x / 0.5)) * maskTileUVSize, saturate(IN.nuv.y / 0.5));
				half2 uvBR = half2((_BottomRightMaskTile + saturate(IN.nuv.x - 0.5) / 0.5) * maskTileUVSize, saturate(IN.nuv.y / 0.5));

				half influenceTL = (IN.nuv.x < 0.5 && IN.nuv.y >= 0.5) ? 1.0 : 0.0;
				half influenceTR = (IN.nuv.x >= 0.5 && IN.nuv.y >= 0.5) ? 1.0 : 0.0;
				half influenceBL = (IN.nuv.x < 0.5 && IN.nuv.y < 0.5) ? 1.0 : 0.0;
				half influenceBR = (IN.nuv.x >= 0.5 && IN.nuv.y < 0.5) ? 1.0 : 0.0;

				fixed4 c = SampleSpriteTexture(IN.uv) * IN.color;
				fixed4 mTL = tex2D(_MaskTex, uvTL);
				fixed4 mTR = tex2D(_MaskTex, uvTR);
				fixed4 mBL = tex2D(_MaskTex, uvBL);
				fixed4 mBR = tex2D(_MaskTex, uvBR);

				/*fixed4 noColor = fixed4(0, 0, 0, 0);
				return lerp(noColor, mTL, influenceTL) +
					lerp(noColor, mTR, influenceTR) +
					lerp(noColor, mBL, influenceBL) +
					lerp(noColor, mBR, influenceBR);*/

				c.a = lerp(0, mTL.g, influenceTL) +
					  lerp(0, mTR.g, influenceTR) + 
					  lerp(0, mBL.g, influenceBL) + 
					  lerp(0, mBR.g, influenceBR);

				c.rgb *= c.a;

				c.rgb *= lerp(1, mTL.r, influenceTL) *
					lerp(1, mTR.r, influenceTR) *
					lerp(1, mBL.r, influenceBL) *
					lerp(1, mBR.r, influenceBR);

				return c;
			}
		ENDCG
		}
	}
}
