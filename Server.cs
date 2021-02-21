using Godot;
using System;
using LiteNetLib;
using LiteNetLib.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

public class Server : Node, INetEventListener
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private NetManager server;
    private NetDataWriter writer;
    public static NetPacketProcessor packetProcessor;

    [Export]
    private int port = 25565;

    public void OnConnectionRequest(ConnectionRequest request)
    {
        Console.WriteLine($"Incoming connection from {request.RemoteEndPoint.ToString()}");
        request.Accept();
    }

    public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
    {
        throw new NotImplementedException();
    }

    public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {
        throw new NotImplementedException();
    }

    public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
    {
        packetProcessor.ReadAllPackets(reader, peer);
    }

    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {
        throw new NotImplementedException();
    }

    public void OnPeerConnected(NetPeer peer)
    {
        Console.WriteLine($"{peer.Id} connected!");
    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        Console.WriteLine($"Player (pid: {(uint)peer.Id}) left the game");
    }

    public void SendPacket<T>(T packet, NetPeer peer, DeliveryMethod deliveryMethod) where T : class, new()
    {
        if (peer != null)
        {
            writer.Reset();
            packetProcessor.Write(writer, packet);
            peer.Send(writer, deliveryMethod);
        }
    }


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        writer = new NetDataWriter();
        SetupPacketProcessor();


        server = new NetManager(this)
        {
            AutoRecycle = true,
        };
        server.Start(port);
        GD.Print("Starting Server! (type q to quit)");
    }

    private void SetupPacketProcessor() 
    {
        packetProcessor = new NetPacketProcessor();
        packetProcessor.RegisterNestedType((w, v) => w.Put(v), reader => reader.GetVector2());
        packetProcessor.RegisterNestedType<PlayerState>();
        //packetProcessor.SubscribeReusable<JoinPacket, NetPeer>(OnJoinReceived.Handle);
        packetProcessor.RegisterNestedType<ClientPlayer>();
        packetProcessor.SubscribeReusable<PlayerSendUpdatePacket, NetPeer>(OnPlayerUpdate.Handle);
    }

    public override void _PhysicsProcess(float delta) 
    {
        server.PollEvents();

        HandlePlayerPositions();
    }

    public void HandlePlayerPositions() 
    {

    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
