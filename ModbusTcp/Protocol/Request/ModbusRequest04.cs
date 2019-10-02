using System;
using System.Net;
using System.Runtime.InteropServices;

namespace ModbusTcp.Protocol.Request
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ModbusRequest04 : ModbusRequestBase
    {
        public ModbusRequest04(byte Unit = 0x01, short TransactionId = 0)
        {
            FunctionCode = 0x04;
            Header.UnitIdentifier = Unit;
            Header.TransactionIdentifier = TransactionId;
            Header.Length = 1 + 3 + 2; // Unit Identifier + ModbusRequestBase + This 
        }

        public ModbusRequest04(int offset, int numberOfWords, byte Unit = 0x01, short TransactionId = 0)
            : this(Unit, TransactionId)
        {
            ReferenceNumber = (short)offset;
            WordCount = (short)numberOfWords;
        }

        [MarshalAs(UnmanagedType.U2)]
        public short WordCount;

        public override byte[] ToNetworkBuffer()
        {
            var copy = (ModbusRequest04)MemberwiseClone();
            copy.Header = Header.Clone();
            copy.ApplyNetworkOrderForBase();

            copy.WordCount = IPAddress.HostToNetworkOrder(copy.WordCount);

            return copy.ToNetworkBytes();
        }
    }
}
