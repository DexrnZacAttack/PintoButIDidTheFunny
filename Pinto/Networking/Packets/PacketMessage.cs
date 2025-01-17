﻿using System.IO;

namespace PintoNS.Networking.Packets
{
    public class PacketMessage : IPacket
    {
        public const int PAYLOAD_MAX_LENGTH = 8388608;
        public string ContactName { get; protected set; }
        public string Sender { get; protected set; }
        public PMSGMessage Payload { get; protected set; }

        public PacketMessage() { }

        public PacketMessage(string contactName, PMSGMessage payload)
        {
            ContactName = contactName;
            Sender = "";
            Payload = payload;
        }

        public void Read(BinaryReader reader)
        {
            ContactName = reader.ReadPintoString(NetBaseHandler.USERNAME_MAX);
            Sender = reader.ReadPintoString(NetBaseHandler.USERNAME_MAX);
            int payloadLength = reader.ReadBEInt();
            if (payloadLength > PAYLOAD_MAX_LENGTH) return;
            byte[] payload = reader.ReadBytes(payloadLength);
            Payload = PMSGMessage.Decode(payload);
        }

        public void Write(BinaryWriter writer)
        {
            writer.WritePintoString(ContactName, NetBaseHandler.USERNAME_MAX);
            writer.WritePintoString(Sender, NetBaseHandler.USERNAME_MAX);
            byte[] payload = Payload.Encode();
            writer.WriteInt(payload.Length);
            writer.WriteBytes(payload);
        }

        public int GetPacketSize()
        {
            // TODO: Figure this crap out
            return (NetBaseHandler.USERNAME_MAX * 2) /*+ PAYLOAD_MAX_LENGTH*/;
        }

        public int GetID()
        {
            return 3;
        }
    }
}