using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Godot;
using Godot.Collections;
using Godot.NativeInterop;
using Vector2 = Godot.Vector2;

namespace BlockFactory.scripts.block_library;


public static class BlockAtlas
{
	public static int AtlasSize;
	public static FactoryBlockLibrary BlockLibrary;
	public static ShaderMaterial BlockMaterial;
	
	// Want to do this ASAP
	public static void Initialize(FactoryBlockLibrary library, ShaderMaterial blockMaterial)
	{
		BlockLibrary = library;
		BlockMaterial = blockMaterial;
		CreateAtlas(library);
		
		foreach (var type in library.Types)
		{
			if (type is FactoryBlockType fb)
			{
				fb.ApplyModel();
			}
		}
	}

	private static void CreateAtlas(FactoryBlockLibrary library)
	{
		var blockTypes = library.Types;
		var textures = new Array<Texture2D>();
		var textureSizes = new List<Vector2>();

		// Load textures
		foreach (var type in blockTypes)
		{
			if (type is FactoryBlockType fb)
			{
				if (fb.Textures == null) continue;
				
				foreach (var tex in fb.Textures)
				{
					if (textures.Contains(tex)) continue;
					
					textureSizes.Add(tex.GetSize());
					textures.Add(tex);
				}
			}
		}

		// // Calculate atlas data
		// var atlasData = Geometry2D.MakeAtlas(textureSizes.ToArray());
		// var atlasPoints = (Vector2[]) atlasData["points"];
		// var minAtlasSizeV = (Vector2I) atlasData["size"];
		// var minAtlasSize = Mathf.Max(minAtlasSizeV.X, minAtlasSizeV.Y);
		// AtlasSize = (int) BitOperations.RoundUpToPowerOf2((uint) minAtlasSize);
		//
		// // Stitch the atlas image
		// var atlas = Image.CreateEmpty(AtlasSize, AtlasSize, true, Image.Format.Rgba8);
		// AtlasMap = new Dictionary<Texture2D, Vector2I>();
		// for (var i = 0; i < atlasPoints.Length; i++)
		// {
		// 	var texture = textures[i].GetImage();
		// 	var point = (Vector2I) atlasPoints[i];
		// 	var texSize = texture.GetSize();
		//
		// 	if (texture.IsCompressed())
		// 	{
		// 		texture.Decompress();
		// 	}
		//
		// 	if (texture.GetFormat() != Image.Format.Rgba8)
		// 	{
		// 		texture.Convert(Image.Format.Rgba8);
		// 	}
		// 	
		// 	atlas.BlitRect(texture, new Rect2I(Vector2I.Zero, texSize), point);
		//
		// 	AtlasMap[textures[i]] = point;
		// }
	}
}
