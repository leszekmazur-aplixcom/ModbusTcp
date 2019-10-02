using System;
using System.Net;
using System.Runtime.InteropServices;

namespace ModbusTcp.Protocol.Request
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class ModbusRequest15 : ModbusRequestBase
    {
        public const int FixedLength = 7;

        private ModbusRequest15(byte Unit = 0x01, short TransactionId = 0)
        {
            FunctionCode = 0x0F;
            Header.UnitIdentifier = Unit;
            Header.TransactionIdentifier = TransactionId;
            Header.Length = 1 + 3 + 3; // Unit Identifier + ModbusRequestBase + This 
        }

        public ModbusRequest15(int offset, short coilQuantity, byte[] values, byte Unit = 0x01, short TransactionId = 0)
            : this(Unit, TransactionId)
        {
            ReferenceNumber = (short)offset;
            CoilQuantity = coilQuantity;
            ByteCount = (byte)values.Length;
            CoilValues = values;

            Header.Length += ByteCount;
        }

        [MarshalAs(UnmanagedType.U2)]
        public short CoilQuantity;

        [MarshalAs(UnmanagedType.U1)]
        public byte ByteCount;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] CoilValues;

        public override byte[] ToNetworkBuffer()
        {
            var copy = (ModbusRequest15)MemberwiseClone();
            copy.Header = Header.Clone();
            copy.ApplyNetworkOrderForBase();

            copy.CoilQuantity = IPAddress.HostToNetworkOrder(copy.CoilQuantity);
            copy.CoilValues = null;

            var buffer = copy.ToNetworkBytes();
            var outputBuffer = new byte[Header.Length - 1 + ModbusHeader.FixedLength];
            Array.Copy(buffer, outputBuffer, outputBuffer.Length - ByteCount);
            Array.Copy(CoilValues, 0, outputBuffer, outputBuffer.Length - ByteCount, ByteCount);
            return outputBuffer;
        }
    }
}
