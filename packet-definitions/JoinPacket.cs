using LiteNetLib;
using Godot;

public class JoinPacket
{
    public string username { get; set; }
    public string level { get; set; }
    public Vector2 position { get; set; }
}