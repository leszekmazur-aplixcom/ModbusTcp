using System;
using System.Net;
using System.Runtime.InteropServices;

namespace ModbusTcp.Protocol.Request
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class ModbusRequest15 : ModbusRequestBase
    {
        public const int FixedLength = 7;

        public ModbusRequest15(byte Unit = 0x01)
        {
            FunctionCode = 0x0F;
            UnitIdentifier = Unit;
        }

        public ModbusRequest15(int offset, short coilQuantity, byte[] values, byte Unit = 0x01)
            : this(Unit)
        {
            ReferenceNumber = (short)offset;
            CoilQuantity = coilQuantity;
            ByteCount = (byte)values.Length;
            RegisterValues = values.ToNetworkBytes();

            Header.Length = (short) (FixedLength + RegisterValues.Length);
        }

        [MarshalAs(UnmanagedType.U2)]
        public short CoilQuantity;

        [MarshalAs(UnmanagedType.U1)]
        public byte ByteCount;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] RegisterValues;

        public override byte[] ToNetworkBuffer()
        {
            var copy = (ModbusRequest15)MemberwiseClone();
            copy.Header = Header.Clone();
            copy.ApplyNetworkOrderForBase();

            copy.CoilQuantity = IPAddress.HostToNetworkOrder(copy.CoilQuantity);

            var buffer = copy.ToNetworkBytes();
            var outputBuffer = new byte[buffer.Length - 2 + RegisterValues.Length];
            Array.Copy(buffer, outputBuffer, buffer.Length - 2);
            Array.Copy(RegisterValues, 0, outputBuffer, buffer.Length - 2, RegisterValues.Length);

            return outputBuffer;
        }
    }
}
