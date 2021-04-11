using LiteNetLib;
using Godot;

public class OnLevelChange
{
    public static void Handle(LevelChangePacket packet, NetPeer peer) {
        GD.Print($"Changed level: {packet.level} (pid: {(uint)peer.Id})");
        ServerPlayer updatedPlayer = Server.players[(uint)peer.Id];
        updatedPlayer.state.level = packet.level;
        // send a packet to everyone saying that a player has changed levels
        foreach (ServerPlayer player in Server.players.Values)
        {
            Server.SendPacket(new PlayerChangedLevelPacket { username =  updatedPlayer.username, state = updatedPlayer.state }, player.peer, DeliveryMethod.ReliableOrdered);
        }
        
    }
}