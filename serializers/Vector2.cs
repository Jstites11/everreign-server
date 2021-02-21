using LiteNetLib;
using LiteNetLib.Utils;
using Godot;

public static class Vector2Serializer
{
    public static void Put(this NetDataWriter writer, Vector2 vector)
    {
        writer.Put(vector.x);
        writer.Put(vector.y);
    }

    public static Vector2 GetVector2(this NetDataReader reader)
    {
        return new Vector2(reader.GetFloat(), reader.GetFloat());
    }
}

