using System;

namespace ModbusTcp.Protocol.Reply
{
    class ModbusReadResponseBase : ModbusResponseBase
    {
        public byte Length;

        public byte[] Data;

        public override void FromNetworkBuffer(ModbusHeader header, byte[] modbusResponse)
        {
            StandardResponseFromNetworkBuffer(header, modbusResponse);

            Length = modbusResponse[1];
            int len = Length;

            Data = new byte[len];
            Buffer.BlockCopy(modbusResponse, 2, Data, 0, len);
        }
    }
}
