using Godot;
using MessagePack;
using MessagePack.Resolvers;
using MessagePackGodot;

namespace BlockFactory.scripts;

[GlobalClass]
public partial class Startup : Node
{
    public Startup()
    {
        var resolver = CompositeResolver.Create(
            GodotResolver.Instance,
            StandardResolver.Instance
        );
        var options = MessagePackSerializerOptions.Standard
            .WithSecurity(MessagePackSecurity.UntrustedData)
            .WithCompression(MessagePackCompression.Lz4Block)
            .WithResolver(resolver);
        MessagePackSerializer.DefaultOptions = options;
    }
}