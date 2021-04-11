using LiteNetLib;
public class OnPlayerUpdate
{
    public static void Handle(PlayerSendUpdatePacket packet, NetPeer peer) {
        Server.players[(uint)peer.Id].state.position = packet.position;
        Server.players[(uint)peer.Id].state.animationState = packet.animationState;
    }
}