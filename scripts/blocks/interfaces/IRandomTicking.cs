using Godot;

namespace BlockFactory.scripts.blocks.interfaces;

public interface IRandomTicking
{
    public void OnRandomTick(FactoryTerrain terrain, Vector3I pos);
}