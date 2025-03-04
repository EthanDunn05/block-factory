using Godot;
using MessagePack;

namespace BlockFactory.scripts.serialization;

[MessagePackObject]
public class PlayerMp
{
    [Key(0)] public Vector3 Position;
}