using System;
using System.Net;

namespace ModbusTcp.Protocol.Reply
{
    class ModbusWriteSingleResponseBase : ModbusResponseBase
    {
        public short ReferenceNumber;

        public short Value;

        public override void FromNetworkBuffer(ModbusHeader header, byte[] modbusResponse)
        {
            StandardResponseFromNetworkBuffer(header, modbusResponse);
            
            ReferenceNumber = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(modbusResponse, 1));
            Value = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(modbusResponse, 3));
        }
    }
}
