using System.IO;
using BlockFactory.scripts.player;
using Godot;
using MessagePack;

namespace BlockFactory.scripts.serialization;

public partial class FactoryStream : VoxelStreamScript
{
    private string savePath = "user://dev.msgpack";

    private FileInfo saveFile;
    private SaveMp saveData;

    public FactoryStream()
    {
        var fileLoc = ProjectSettings.GlobalizePath(savePath);
        saveFile = new FileInfo(fileLoc);

        if (!saveFile.Exists)
        {
            GD.Print("Making " + fileLoc);
            saveData = new SaveMp();
        }
        else
        {
            using var saveStream = saveFile.OpenRead();
            saveData = MessagePackSerializer.Deserialize<SaveMp>(saveStream);
        }
    }

    public override int _LoadVoxelBlock(VoxelBuffer outBuffer, Vector3I positionInBlocks, int lod)
    {
        if (saveData.Blocks.ContainsKey(positionInBlocks.ToString()))
        {
            var block = saveData.Blocks[positionInBlocks.ToString()];
            VoxelBlockSerializer.DeserializeFromByteArray(block.BlockData, outBuffer, true);

            return (int) ResultCode.BlockFound;
        }

        return (int) ResultCode.BlockNotFound;
    }

    public override void _SaveVoxelBlock(VoxelBuffer buffer, Vector3I positionInBlocks, int lod)
    {
        var block = new BlockMp();

        block.BlockData = VoxelBlockSerializer.SerializeToByteArray(buffer, true);
        saveData.Blocks[positionInBlocks.ToString()] = block;
    }

    public void SavePlayer(Player player)
    {
        saveData.Player.Position = player.Position;
    }

    public void LoadPlayer(Player player)
    {
        player.Position = saveData.Player.Position;
    }

    // Dispose of the stream as well as this object
    protected override void Dispose(bool disposing)
    {
        using (var saveStream = saveFile.OpenWrite())
        {
            MessagePackSerializer.Serialize(saveStream, saveData);
        }
        
        base.Dispose(disposing);
    }
}