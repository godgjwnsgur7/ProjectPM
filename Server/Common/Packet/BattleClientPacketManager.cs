using ServerCore;
using System;
using System.Collections.Generic;

public partial class BattlePacketManager : Singleton<BattlePacketManager>
{
	Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>> _makeFunc = new Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>>();
	Dictionary<ushort, Action<PacketSession, IPacket>> _handler = new Dictionary<ushort, Action<PacketSession, IPacket>>();

	public event Action<PacketSession, IPacket> _eventHandler = null;

	protected override void OnAwakeInstance()
    {
        base.OnAwakeInstance();
        RegisterHandler();
    }

	public void RegisterHandler()
	{
		_makeFunc.Add((ushort)BattlePacketID.RES_CONNECTED, MakePacket<RES_CONNECTED>);
		_handler.Add((ushort)BattlePacketID.RES_CONNECTED, ON_RES_CONNECTED);
		_makeFunc.Add((ushort)BattlePacketID.RES_BROADCAST_ENTER_GAME, MakePacket<RES_BROADCAST_ENTER_GAME>);
		_handler.Add((ushort)BattlePacketID.RES_BROADCAST_ENTER_GAME, ON_RES_BROADCAST_ENTER_GAME);
		_makeFunc.Add((ushort)BattlePacketID.RES_BROADCAST_LEAVE_GAME, MakePacket<RES_BROADCAST_LEAVE_GAME>);
		_handler.Add((ushort)BattlePacketID.RES_BROADCAST_LEAVE_GAME, ON_RES_BROADCAST_LEAVE_GAME);
		_makeFunc.Add((ushort)BattlePacketID.RES_PLAYER_LIST, MakePacket<RES_PLAYER_LIST>);
		_handler.Add((ushort)BattlePacketID.RES_PLAYER_LIST, ON_RES_PLAYER_LIST);
		_makeFunc.Add((ushort)BattlePacketID.RES_FRAME_INPUT, MakePacket<RES_FRAME_INPUT>);
		_handler.Add((ushort)BattlePacketID.RES_FRAME_INPUT, ON_RES_FRAME_INPUT);

	}

	public void OnReceivePacket(PacketSession session, ArraySegment<byte> buffer)
	{
		ushort count = 0;

		ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
		count += 2;
		ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
		count += 2;

		Func<PacketSession, ArraySegment<byte>, IPacket> func = null;
		if (_makeFunc.TryGetValue(id, out func))
		{
			IPacket packet = func.Invoke(session, buffer);
			HandlePacket(session, packet);
		}
	}

	private T MakePacket<T>(PacketSession session, ArraySegment<byte> buffer) where T : IPacket, new()
	{
		T pkt = new T();
		pkt.Read(buffer);
		return pkt;
	}

	public void HandlePacket(PacketSession session, IPacket packet)
	{
		if (_handler.TryGetValue(packet.Protocol, out var action))
		{
			action.Invoke(session, packet);
		}

		_eventHandler?.Invoke(session, packet);
	}
}