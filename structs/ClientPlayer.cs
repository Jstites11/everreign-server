using LiteNetLib;
using LiteNetLib.Utils;

public struct ClientPlayer : INetSerializable
{
    public PlayerState state;
    public string username;

    public void Serialize(NetDataWriter writer)
    {
        state.Serialize(writer);
        writer.Put(username);
    }

    public void Deserialize(NetDataReader reader)
    {
        state.Deserialize(reader);
        username = reader.GetString();
    }
}