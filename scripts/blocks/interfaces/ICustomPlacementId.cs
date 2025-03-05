using BlockFactory.scripts.player;
using Godot;

namespace BlockFactory.scripts.blocks.interfaces;

public interface ICustomPlacementId
{
    public ulong GetId(Player player, Vector3I pos, Vector3 placementNormal, FactoryTerrain terrain);
}