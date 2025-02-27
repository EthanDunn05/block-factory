using Godot;

namespace BlockFactory.scripts.blocks.attributes;

[Tool]
[GlobalClass]
public partial class SlabAttribute : FactoryAttribute
{
    public const string Name = "slab";
    public const string Bottom = "bottom";
    public const string Top = "top";
    public const string North = "north";
    public const string South = "south";
    public const string East = "east";
    public const string West = "west";
    
    public override string AttributeName => Name;
    public override int DefaultValue => 0;
    public override string[] Values => [Bottom, Top, North, South, East, West];
}