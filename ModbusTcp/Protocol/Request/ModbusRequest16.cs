using System;
using System.Net;
using System.Runtime.InteropServices;

namespace ModbusTcp.Protocol.Request
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class ModbusRequest16 : ModbusRequestBase
    {
        public ModbusRequest16(byte Unit = 0x01, short TransactionId = 0)
        {
            FunctionCode = 0x10;
            Header.UnitIdentifier = Unit;
            Header.TransactionIdentifier = TransactionId;
            Header.Length = 1 + 3 + 3; // Unit Identifier + ModbusRequestBase + This 
        }

        public ModbusRequest16(int offset, float[] values, byte Unit = 0x01, short TransactionId = 0)
            : this(Unit, TransactionId)
        {

            ReferenceNumber = (short)offset;
            WordCount = (short)(values.Length * 2);
            RegisterValues = values.ToNetworkBytes();
            ByteCount = (byte)RegisterValues.Length;

            Header.Length += (short)RegisterValues.Length;
        }

        [MarshalAs(UnmanagedType.U2)]
        public short WordCount;

        [MarshalAs(UnmanagedType.U1)]
        public byte ByteCount;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] RegisterValues;

        public override byte[] ToNetworkBuffer()
        {
            var copy = (ModbusRequest16)MemberwiseClone();
            copy.Header = Header.Clone();
            copy.Header = Header.Clone();
            copy.ApplyNetworkOrderForBase();

            copy.WordCount = IPAddress.HostToNetworkOrder(copy.WordCount);

            var buffer = copy.ToNetworkBytes();
            var outputBuffer = new byte[buffer.Length - 2 + RegisterValues.Length];
            Array.Copy(buffer, outputBuffer, buffer.Length - 2);
            Array.Copy(RegisterValues, 0, outputBuffer, buffer.Length - 2, RegisterValues.Length);

            return outputBuffer;
        }
    }
}
