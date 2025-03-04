using System.Collections.Generic;
using System.Linq;
using Godot;
using MessagePack;

namespace BlockFactory.scripts.serialization;

[MessagePackObject]
public class SaveMp
{
    [Key(0)] public ulong Seed { get; set; } = 0;
    [Key(1)] public Dictionary<string, BlockMp> Blocks { get; set; } = new();
    [Key(2)] public PlayerMp Player { get; set; } = new();
}