using System.Collections.Generic;
using MessagePack;
using Godot;

namespace BlockFactory.scripts.serialization;

[MessagePackObject]
public class BlockMp
{
    [Key(0)] public short SizeX { get; set; }
    [Key(1)] public short SizeY { get; set; }
    [Key(2)] public short SizeZ { get; set; }
    [Key(3)] public byte[] BlockData { get; set; } = [];
}