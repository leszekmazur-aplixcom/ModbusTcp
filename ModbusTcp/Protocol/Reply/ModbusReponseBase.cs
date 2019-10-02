using System;

namespace ModbusTcp.Protocol.Reply
{
    abstract class ModbusResponseBase : ModbusBase
    {
        private const byte ModbusErrorOffset = 0x80;

        public byte UnitIdentifier;

        public byte FunctionCode;

        public abstract void FromNetworkBuffer(ModbusHeader header, byte[] modbusResponse);

        protected void StandardResponseFromNetworkBuffer(ModbusHeader header, byte[] modbusResponse)
        {
            Header = header;
            
            FunctionCode = modbusResponse[0];

            if (FunctionCode >= 0x80)
            {
                var exceptionCode = modbusResponse[1];
                var orginalFunctionCode = (byte)(FunctionCode - ModbusErrorOffset);

                throw new ModbusReplyException(orginalFunctionCode, exceptionCode);
            }
        }

        public override sealed byte[] ToNetworkBuffer()
        {
            throw new NotImplementedException();
        }
    }
}
