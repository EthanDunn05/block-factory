using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Godot;
using Godot.Collections;
using Godot.NativeInterop;
using Vector2 = Godot.Vector2;

namespace BlockFactory.scripts.block_library;


public static class FactoryData
{
	public static FactoryBlockLibrary BlockLibrary;
	public static ShaderMaterial BlockMaterial;
	
	// Want to do this ASAP
	public static void Initialize(FactoryBlockLibrary library, ShaderMaterial blockMaterial)
	{
		BlockLibrary = library;
		BlockMaterial = blockMaterial;
	}
}
