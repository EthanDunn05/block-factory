namespace BlockFactory.scripts.blocks.attributes;

public partial class LodAttribute : FactoryAttribute
{
    public const string Name = "lod";
    public const string Low = "low";
    public const string High = "high";

    public override string AttributeName => Name;
    public override int DefaultValue => 0;
    public override string[] Values => [High, Low];
}