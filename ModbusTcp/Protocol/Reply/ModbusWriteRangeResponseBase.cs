using System;
using System.Net;

namespace ModbusTcp.Protocol.Reply
{
    class ModbusWriteRangeResponseBase : ModbusResponseBase
    {
        public short ReferenceNumber;

        public short WordCount;

        public override void FromNetworkBuffer(ModbusHeader header, byte[] modbusResponse)
        {
            StandardResponseFromNetworkBuffer(header, modbusResponse);

            ReferenceNumber = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(modbusResponse, 1));
            WordCount = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(modbusResponse, 3));
        }
    }
}
