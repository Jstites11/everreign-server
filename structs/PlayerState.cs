using LiteNetLib;
using LiteNetLib.Utils;
using Godot;

public struct PlayerState : INetSerializable
{
    public uint pid;
    public string username;
    public string level;
    public Vector2 position;
    public int animationState;

    public void Serialize(NetDataWriter writer)
    {
        writer.Put(pid);
        writer.Put(username);
        writer.Put(level);
        writer.Put(position);
        writer.Put(animationState);
    }

    public void Deserialize(NetDataReader reader)
    {
        pid = reader.GetUInt();
        username = reader.GetString();
        level = reader.GetString();
        position = reader.GetVector2();
        animationState = reader.GetInt();
    }
}