using LiteNetLib;
using LiteNetLib.Utils;
using Godot;

public struct PlayerState : INetSerializable
{
    public uint pid;
    public Vector2 position;

    public void Serialize(NetDataWriter writer)
    {
        writer.Put(pid);
        writer.Put(position);
    }

    public void Deserialize(NetDataReader reader)
    {
        pid = reader.GetUInt();
        position = reader.GetVector2();
    }
}