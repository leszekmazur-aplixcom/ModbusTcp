﻿using System;
using System.Net;
using System.Runtime.InteropServices;

namespace ModbusTcp.Protocol.Request
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ModbusRequest01 : ModbusRequestBase
    {
        public ModbusRequest01(byte Unit = 0x01, short TransactionId = 0)
        {
            FunctionCode = 0x01;
            Header.UnitIdentifier = Unit;
            Header.TransactionIdentifier = TransactionId;
            Header.Length = 1 + 3 + 2; // Unit Identifier + ModbusRequestBase + This 
        }

        public ModbusRequest01(int offset, int numberOfCoils, byte Unit = 0x01, short TransactionId = 0)
            : this(Unit, TransactionId)
        {
            ReferenceNumber = (short)offset;
            BitCount = (short)numberOfCoils;
        }

        [MarshalAs(UnmanagedType.U2)]
        public short BitCount;

        public override byte[] ToNetworkBuffer()
        {
            var copy = (ModbusRequest01)MemberwiseClone();
            copy.Header = Header.Clone();
            copy.ApplyNetworkOrderForBase();

            copy.BitCount = IPAddress.HostToNetworkOrder(copy.BitCount);

            return copy.ToNetworkBytes();
        }
    }
}
