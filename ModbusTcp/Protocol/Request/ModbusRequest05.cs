using System;
using System.Net;
using System.Runtime.InteropServices;

namespace ModbusTcp.Protocol.Request
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class ModbusRequest05 : ModbusRequestBase
    {
        public static readonly short HighValue = BitConverter.ToInt16(new byte[] { 0x00, 0xFF }, 0);
        public static readonly short LowValue = BitConverter.ToInt16(new byte[] { 0x00, 0x00 }, 0);

        public ModbusRequest05(byte Unit = 0x01, short TransactionId = 0)
        {
            FunctionCode = 0x05;
            Header.UnitIdentifier = Unit;
            Header.TransactionIdentifier = TransactionId;
            Header.Length = 1 + 3 + 2; // Unit Identifier + ModbusRequestBase + This 
        }

        public ModbusRequest05(int offset, bool value, byte Unit = 0x01, short TransactionId = 0)
            : this(Unit, TransactionId)
        {
            ReferenceNumber = (short)offset;
            Value = value ? HighValue : LowValue;
        }

        [MarshalAs(UnmanagedType.U2)]
        public short Value;

        public override byte[] ToNetworkBuffer()
        {
            var copy = (ModbusRequest05)MemberwiseClone();
            copy.Header = Header.Clone();
            copy.ApplyNetworkOrderForBase();

            copy.Value = IPAddress.HostToNetworkOrder(copy.Value);

            return copy.ToNetworkBytes();
        }
    }
}
