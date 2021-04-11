using LiteNetLib;
using System;

public class OnJoinReceived
{
    public static void Handle(JoinPacket packet, NetPeer peer) {
        Console.WriteLine($"Received join from {packet.username} (pid: {(uint)peer.Id})");

        ServerPlayer newPlayer = (Server.players[(uint)peer.Id] = new ServerPlayer 
        {
            peer = peer,
            state = new PlayerState
            {
                pid = (uint)peer.Id,
                username = packet.username,
                position = packet.position,
                level = packet.level,
                animationState = 0,
            },
            username = packet.username,
        });

        // Send server player state back to client
        Server.SendPacket(new JoinAcceptPacket { state = newPlayer.state }, peer, DeliveryMethod.ReliableOrdered);

        // for each player currently in the server, send each player info to the client to render a remoteplayer character
        foreach (ServerPlayer player in Server.players.Values)
        {
            if (player.state.pid != newPlayer.state.pid)
            {
                Server.SendPacket(new PlayerJoinedGamePacket
                {
                    player = new ClientPlayer
                    {
                        username = newPlayer.username,
                        state = newPlayer.state,
                    },
                }, player.peer, DeliveryMethod.ReliableOrdered);

                Server.SendPacket(new PlayerJoinedGamePacket
                {
                    player = new ClientPlayer
                    {
                        username = player.username,
                        state = player.state,
                    },
                }, newPlayer.peer, DeliveryMethod.ReliableOrdered);
            }
        }


    }
}