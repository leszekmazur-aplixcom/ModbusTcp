﻿using System;
using System.Net.Sockets;
using ModbusTcp.Protocol.Request;
using System.Threading.Tasks;
using ModbusTcp.Protocol;
using System.Linq;
using ModbusTcp.Protocol.Reply;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace ModbusTcp
{
    public class ModbusClient
    {
        public bool Connected => tcpClient?.Connected ?? false;

        private int socketTimeout;
        private readonly int port;
        private TcpClient tcpClient;
        private NetworkStream transportStream;
        private readonly string ipAddress;

        // Let's wait for 60 seconds for the socket if socketTimeout
        // isn't passed by caller
        public ModbusClient(string ipAddress, int port, int socketTimeout = 60000)
        {
            this.ipAddress = ipAddress;
            this.port = port;
            this.socketTimeout = socketTimeout;
        }

        public void Init()
        {
            try
            {
                tcpClient = new TcpClient(ipAddress, port);
                transportStream = tcpClient.GetStream();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to open TCP client - {e}");
                throw e;
            }
        }

        /// <summary>
        /// Reads words holding registers
        /// </summary>
        /// <param name="offset">The register offset</param>
        /// <param name="count">Number of words to read</param>
        /// <returns>The words read</returns>
        public async Task<short[]> ReadRegistersAsync(int offset, int count, byte unit = 0x01)
        {
            if (tcpClient == null)
                throw new Exception("Object not intialized");

            var request = new ModbusRequest03(offset, count, unit);
            var buffer = request.ToNetworkBuffer();

            using (var cancellationTokenSource = new CancellationTokenSource(socketTimeout))
            {
                using (cancellationTokenSource.Token.Register(() => transportStream.Close()))
                {
                    await transportStream.WriteAsync(buffer, 0, buffer.Length, cancellationTokenSource.Token);
                }
            }
            var response = await ReadResponseAsync<ModbusReply03>();

            return ReadAsShort(response.Data);
        }

        /// <summary>
        /// Reads words holding registers
        /// </summary>
        /// <param name="offset">The register offset</param>
        /// <param name="count">Number of words to read</param>
        /// <returns>The words read</returns>
        public short[] ReadRegisters(int offset, int count, byte unit = 0x01)
        {
            if (tcpClient == null)
                throw new Exception("Object not intialized");

            var request = new ModbusRequest03(offset, count, unit);
            var buffer = request.ToNetworkBuffer();

            using (var cancellationTokenSource = new CancellationTokenSource(socketTimeout))
            {
                using (cancellationTokenSource.Token.Register(() => transportStream.Close()))
                {
                    transportStream.Write(buffer, 0, buffer.Length);
                }
            }
            var response = ReadResponse<ModbusReply03>();

            return ReadAsShort(response.Data);
        }

        /// <summary>
        /// Reads words input registers
        /// </summary>
        /// <param name="offset">The register offset</param>
        /// <param name="count">Number of words to read</param>
        /// <returns>The words read</returns>
        public async Task<short[]> ReadInputRegistersAsync(int offset, int count, byte unit = 0x01)
        {
            if (tcpClient == null)
                throw new Exception("Object not intialized");

            var request = new ModbusRequest04(offset, count, unit);
            var buffer = request.ToNetworkBuffer();

            using (var cancellationTokenSource = new CancellationTokenSource(socketTimeout))
            {
                using (cancellationTokenSource.Token.Register(() => transportStream.Close()))
                {
                    await transportStream.WriteAsync(buffer, 0, buffer.Length, cancellationTokenSource.Token);
                }
            }
            var response = await ReadResponseAsync<ModbusReply04>();
            return ReadAsShort(response.Data);
        }

        /// <summary>
        /// Reads words input registers
        /// </summary>
        /// <param name="offset">The register offset</param>
        /// <param name="count">Number of words to read</param>
        /// <returns>The words read</returns>
        public short[] ReadInputRegisters(int offset, int count, byte unit = 0x01)
        {
            if (tcpClient == null)
                throw new Exception("Object not intialized");

            var request = new ModbusRequest04(offset, count, unit);
            var buffer = request.ToNetworkBuffer();

            using (var cancellationTokenSource = new CancellationTokenSource(socketTimeout))
            {
                using (cancellationTokenSource.Token.Register(() => transportStream.Close()))
                {
                    transportStream.Write(buffer, 0, buffer.Length);
                }
            }
            var response = ReadResponse<ModbusReply04>();
            return ReadAsShort(response.Data);
        }

        /// <summary>
        /// Reads words input registers
        /// </summary>
        /// <param name="offset">The register offset</param>
        /// <param name="count">Number of words to read</param>
        /// <returns>The words read</returns>
        public async Task<byte[]> ReadInputsAsync(int offset, int count, byte unit = 0x01)
        {
            if (tcpClient == null)
                throw new Exception("Object not intialized");

            var request = new ModbusRequest02(offset, count, unit);
            var buffer = request.ToNetworkBuffer();

            using (var cancellationTokenSource = new CancellationTokenSource(socketTimeout))
            {
                using (cancellationTokenSource.Token.Register(() => transportStream.Close()))
                {
                    await transportStream.WriteAsync(buffer, 0, buffer.Length, cancellationTokenSource.Token);
                }
            }
            var response = await ReadResponseAsync<ModbusReply02>();
            return response.Data;
        }

        /// <summary>
        /// Reads words input registers
        /// </summary>
        /// <param name="offset">The register offset</param>
        /// <param name="count">Number of words to read</param>
        /// <returns>The words read</returns>
        public byte[] ReadInputs(int offset, int count, byte unit = 0x01)
        {
            if (tcpClient == null)
                throw new Exception("Object not intialized");

            var request = new ModbusRequest02(offset, count, unit);
            var buffer = request.ToNetworkBuffer();

            using (var cancellationTokenSource = new CancellationTokenSource(socketTimeout))
            {
                using (cancellationTokenSource.Token.Register(() => transportStream.Close()))
                {
                    transportStream.WriteAsync(buffer, 0, buffer.Length);
                }
            }
            var response = ReadResponse<ModbusReply02>();
            return response.Data;
        }

        /// <summary>
        /// Reads words input registers
        /// </summary>
        /// <param name="offset">The register offset</param>
        /// <param name="count">Number of words to read</param>
        /// <returns>The words read</returns>
        public async Task<byte[]> ReadCoilsAsync(int offset, int count, byte unit = 0x01)
        {
            if (tcpClient == null)
                throw new Exception("Object not intialized");

            var request = new ModbusRequest01(offset, count, unit);
            var buffer = request.ToNetworkBuffer();

            using (var cancellationTokenSource = new CancellationTokenSource(socketTimeout))
            {
                using (cancellationTokenSource.Token.Register(() => transportStream.Close()))
                {
                    await transportStream.WriteAsync(buffer, 0, buffer.Length, cancellationTokenSource.Token);
                }
            }
            var response = await ReadResponseAsync<ModbusReply01>();
            return response.Data;
        }

        /// <summary>
        /// Reads words input registers
        /// </summary>
        /// <param name="offset">The register offset</param>
        /// <param name="count">Number of words to read</param>
        /// <returns>The words read</returns>
        public byte[] ReadCoils(int offset, int count, byte unit = 0x01)
        {
            if (tcpClient == null)
                throw new Exception("Object not intialized");

            var request = new ModbusRequest01(offset, count, unit);
            var buffer = request.ToNetworkBuffer();

            using (var cancellationTokenSource = new CancellationTokenSource(socketTimeout))
            {
                using (cancellationTokenSource.Token.Register(() => transportStream.Close()))
                {
                    transportStream.Write(buffer, 0, buffer.Length);
                }
            }

            var response = ReadResponse<ModbusReply01>();
            return response.Data;
        }

        /// <summary>
        /// Reads floats from holding registers
        /// </summary>
        /// <param name="offset">The register offset</param>
        /// <param name="count">Number of floats to read</param>
        /// <returns>The floats read</returns>
        public async Task<float[]> ReadRegistersFloatsAsync(int offset, int count, byte unit = 0x01)
        {
            if (tcpClient == null)
                throw new Exception("Object not intialized");

            var request = new ModbusRequest03(offset, count * 2 /* Float is 2 word */, unit);

            var buffer = request.ToNetworkBuffer();

            using (var cancellationTokenSource = new CancellationTokenSource(socketTimeout))
            {
                using (cancellationTokenSource.Token.Register(() => transportStream.Close()))
                {
                    await transportStream.WriteAsync(buffer, 0, buffer.Length, cancellationTokenSource.Token);
                }
            }

            var response = await ReadResponseAsync<ModbusReply03>();
            return ReadAsFloat(response.Data);
        }

        /// <summary>
        /// Reads floats from holding registers
        /// </summary>
        /// <param name="offset">The register offset</param>
        /// <param name="count">Number of floats to read</param>
        /// <returns>The floats read</returns>
        public float[] ReadRegistersFloats(int offset, int count, byte unit = 0x01)
        {
            if (tcpClient == null)
                throw new Exception("Object not intialized");

            var request = new ModbusRequest03(offset, count * 2 /* Float is 2 word */, unit);

            var buffer = request.ToNetworkBuffer();

            using (var cancellationTokenSource = new CancellationTokenSource(socketTimeout))
            {
                using (cancellationTokenSource.Token.Register(() => transportStream.Close()))
                {
                    transportStream.Write(buffer, 0, buffer.Length);
                }
            }

            var response = ReadResponse<ModbusReply03>();
            return ReadAsFloat(response.Data);
        }

        public async Task<byte[]> ReadHoldingRegistersAsync(int offset, int count, byte unit = 0x01)
        {
            if (tcpClient == null)
                throw new Exception("Object not intialized");

            var request = new ModbusRequest03(offset, count, unit);
            var buffer = request.ToNetworkBuffer();

            using (var cancellationTokenSource = new CancellationTokenSource(socketTimeout))
            {
                using (cancellationTokenSource.Token.Register(() => transportStream.Close()))
                {
                    await transportStream.WriteAsync(buffer, 0, buffer.Length, cancellationTokenSource.Token);
                }
            }

            var response = await ReadResponseAsync<ModbusReply03>();
            return response.Data;
        }

        public byte[] ReadHoldingRegisters(int offset, int count, byte unit = 0x01)
        {
            if (tcpClient == null)
                throw new Exception("Object not intialized");

            var request = new ModbusRequest03(offset, count, unit);
            var buffer = request.ToNetworkBuffer();

            using (var cancellationTokenSource = new CancellationTokenSource(socketTimeout))
            {
                using (cancellationTokenSource.Token.Register(() => transportStream.Close()))
                {
                    transportStream.Write(buffer, 0, buffer.Length);
                }
            }

            var response = ReadResponse<ModbusReply03>();
            return response.Data;
        }

        /// <summary>
        /// Writes floats to holding registers
        /// </summary>
        /// <param name="offset">The first register offset</param>
        /// <param name="values">The values to write</param>
        /// <returns>Awaitable task</returns>
        public async Task WriteRegistersAsync(int offset, float[] values, byte unit = 0x01)
        {
            if (tcpClient == null)
                throw new Exception("Object not intialized");

            var request = new ModbusRequest16(offset, values, unit);
            var buffer = request.ToNetworkBuffer();

            using (var cancellationTokenSource = new CancellationTokenSource(socketTimeout))
            {
                using (cancellationTokenSource.Token.Register(() => transportStream.Close()))
                {
                    await transportStream.WriteAsync(buffer, 0, buffer.Length, cancellationTokenSource.Token);
                }
            }
            var response = await ReadResponseAsync<ModbusReply16>();
        }

        /// <summary>
        /// Writes words to holding registers
        /// </summary>
        /// <param name="offset">The first register offset</param>
        /// <param name="values">The values to write</param>
        /// <returns>Awaitable task</returns>
        public async Task WriteRegistersAsync(int offset, short[] values, byte unit = 0x01)
        {
            if (tcpClient == null)
                throw new Exception("Object not intialized");

            var request = new ModbusRequest16(unit);
            request.WordCount = (short)(values.Length * 2);
            request.RegisterValues = values.ToNetworkBytes();

            var buffer = request.ToNetworkBuffer();

            using (var cancellationTokenSource = new CancellationTokenSource(socketTimeout))
            {
                using (cancellationTokenSource.Token.Register(() => transportStream.Close()))
                {
                    await transportStream.WriteAsync(buffer, 0, buffer.Length, cancellationTokenSource.Token);
                }
            }

            await ReadResponseAsync<ModbusReply16>();
        }

        /// <summary>
        /// Writes words to holding registers
        /// </summary>
        /// <param name="offset">The first register offset</param>
        /// <param name="values">The values to write</param>
        /// <returns>Awaitable task</returns>
        public void WriteRegisters(int offset, short[] values, byte unit = 0x01)
        {
            if (tcpClient == null)
                throw new Exception("Object not intialized");

            var request = new ModbusRequest16(unit);
            request.WordCount = (short)(values.Length * 2);
            request.RegisterValues = values.ToNetworkBytes();

            var buffer = request.ToNetworkBuffer();

            using (var cancellationTokenSource = new CancellationTokenSource(socketTimeout))
            {
                using (cancellationTokenSource.Token.Register(() => transportStream.Close()))
                {
                    transportStream.Write(buffer, 0, buffer.Length);
                }
            }

            ReadResponse<ModbusReply16>();
        }

        /// <summary>
        /// Writes multiple coils
        /// </summary>
        /// <param name="offset">The first register offset</param>
        /// <param name="coilQuantity">How many coils to write</param>
        /// <param name="values">The values to write</param>
        /// <returns>Awaitable task</returns>
        public async Task WriteCoilsAsync(int offset, short coilQuantity, byte[] values, byte unit = 0x01)
        {
            if (tcpClient == null)
                throw new Exception("Object not intialized");

            var request = new ModbusRequest15(offset, coilQuantity, values, unit);
            var buffer = request.ToNetworkBuffer();

            using (var cancellationTokenSource = new CancellationTokenSource(socketTimeout))
            {
                using (cancellationTokenSource.Token.Register(() => transportStream.Close()))
                {
                    await transportStream.WriteAsync(buffer, 0, buffer.Length, cancellationTokenSource.Token);
                }
            }

            await ReadResponseAsync<ModbusReply15>();
        }

        /// <summary>
        /// Writes multiple coils
        /// </summary>
        /// <param name="offset">The first register offset</param>
        /// <param name="coilQuantity">How many coils to write</param>
        /// <param name="values">The values to write</param>
        /// <returns>Awaitable task</returns>
        public void WriteCoils(int offset, short coilQuantity, byte[] values, byte unit = 0x01)
        {
            if (tcpClient == null)
                throw new Exception("Object not intialized");

            var request = new ModbusRequest15(offset, coilQuantity, values, unit);
            var buffer = request.ToNetworkBuffer();

            using (var cancellationTokenSource = new CancellationTokenSource(socketTimeout))
            {
                using (cancellationTokenSource.Token.Register(() => transportStream.Close()))
                {
                    transportStream.Write(buffer, 0, buffer.Length);
                }
            }

            ReadResponse<ModbusReply15>();
        }

        /// <summary>
        /// Writes bit to coil
        /// </summary>
        /// <param name="offset">The first register offset</param>
        /// <param name="values">The values to write</param>
        /// <returns>Awaitable task</returns>
        public async Task WriteCoilAsync(int offset, bool value, byte unit = 0x01)
        {
            if (tcpClient == null)
                throw new Exception("Object not intialized");

            var request = new ModbusRequest05(offset, value, unit);

            var buffer = request.ToNetworkBuffer();

            using (var cancellationTokenSource = new CancellationTokenSource(socketTimeout))
            {
                using (cancellationTokenSource.Token.Register(() => transportStream.Close()))
                {
                    await transportStream.WriteAsync(buffer, 0, buffer.Length, cancellationTokenSource.Token);
                }
            }

            await ReadResponseAsync<ModbusReply05>();
        }

        /// <summary>
        /// Writes bit to coil
        /// </summary>
        /// <param name="offset">The first register offset</param>
        /// <param name="values">The values to write</param>
        /// <returns>Awaitable task</returns>
        public void WriteCoil(int offset, bool value, byte unit = 0x01)
        {
            if (tcpClient == null)
                throw new Exception("Object not intialized");

            var request = new ModbusRequest05(offset, value, unit);

            var buffer = request.ToNetworkBuffer();

            using (var cancellationTokenSource = new CancellationTokenSource(socketTimeout))
            {
                using (cancellationTokenSource.Token.Register(() => transportStream.Close()))
                {
                    transportStream.Write(buffer, 0, buffer.Length);
                }
            }

            ReadResponse<ModbusReply05>();
        }

        /// <summary>
        /// Writes single register to holding registers
        /// </summary>
        /// <param name="offset">The first register offset</param>
        /// <param name="values">The values to write</param>
        /// <returns>Awaitable task</returns>
        public async Task WriteRegisterAsync(int offset, short value, byte unit = 0x01)
        {
            if (tcpClient == null)
                throw new Exception("Object not intialized");

            var request = new ModbusRequest06(unit, value, unit);

            var buffer = request.ToNetworkBuffer();

            using (var cancellationTokenSource = new CancellationTokenSource(socketTimeout))
            {
                using (cancellationTokenSource.Token.Register(() => transportStream.Close()))
                {
                    await transportStream.WriteAsync(buffer, 0, buffer.Length, cancellationTokenSource.Token);
                }
            }

            await ReadResponseAsync<ModbusReply06>();
        }

        /// <summary>
        /// Writes single register to holding registers
        /// </summary>
        /// <param name="offset">The first register offset</param>
        /// <param name="values">The values to write</param>
        /// <returns>Awaitable task</returns>
        public void WriteRegister(int offset, short value, byte unit = 0x01)
        {
            if (tcpClient == null)
                throw new Exception("Object not intialized");

            var request = new ModbusRequest06(unit, value, unit);

            var buffer = request.ToNetworkBuffer();

            using (var cancellationTokenSource = new CancellationTokenSource(socketTimeout))
            {
                using (cancellationTokenSource.Token.Register(() => transportStream.Close()))
                {
                    transportStream.WriteAsync(buffer, 0, buffer.Length);
                }
            }

            ReadResponse<ModbusReply06>();
        }

        /// <summary>
        /// Terminates the session
        /// </summary>
        public void Terminate()
        {
            tcpClient?.Close();
            tcpClient?.Dispose();
            tcpClient = null;
        }

        private short[] ReadAsShort(byte[] data)
        {
            var idx = 0;
            var output = new List<short>();

            while (idx < data.Length)
            {
                var value = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(data, idx));
                idx += 2;

                output.Add(value);
            }

            return output.ToArray();
        }

        private float[] ReadAsFloat(byte[] data)
        {
            var idx = 0;
            var output = new List<float>();

            while (idx < data.Length)
            {
                var value = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(data, idx));
                var f = BitConverter.ToSingle(BitConverter.GetBytes(value), 0);
                idx += 4;

                output.Add(f);
            }

            return output.ToArray();
        }

        private async Task<T> ReadResponseAsync<T>() where T : ModbusResponseBase
        {
            var headerBytes = await ReadFromBufferAsync(ModbusHeader.FixedLength);
            var header = ModbusHeader.FromNetworkBuffer(headerBytes);

            var dataBytes = await ReadFromBufferAsync(header.Length - 1); //Ommit Unit Identifier from MBAP Header
            
            var response = Activator.CreateInstance<T>();

            try
            {
                response.FromNetworkBuffer(header, dataBytes);
            }
            catch (ModbusReplyException mre)
            {
                throw new ModbusReplyException(mre, $"{ ipAddress }:{ port }");
            }

            return response;
        }

        private T ReadResponse<T>() where T : ModbusResponseBase
        {
            var headerBytes = ReadFromBuffer(ModbusHeader.FixedLength);
            var header = ModbusHeader.FromNetworkBuffer(headerBytes);

            var dataBytes = ReadFromBuffer(header.Length - 1); //Ommit Unit Identifier from MBAP Header
            
            var response = Activator.CreateInstance<T>();
            try
            {
                response.FromNetworkBuffer(header, dataBytes);
            }
            catch (ModbusReplyException mre)
            {
                throw new ModbusReplyException(mre, $"{ ipAddress }:{ port }");
            }

            return response;
        }

        private async Task<byte[]> ReadFromBufferAsync(int totalSize)
        {
            var buffer = new byte[totalSize];

            var idx = 0;
            var remainder = totalSize;

            while (remainder > 0)
            {
                int readBytes = 0;
                using (var cancellationTokenSource = new CancellationTokenSource(socketTimeout))
                {
                    using (cancellationTokenSource.Token.Register(() => transportStream.Close()))
                    {
                        readBytes = await transportStream.ReadAsync(buffer, idx, remainder, cancellationTokenSource.Token);
                    }
                }
                remainder -= readBytes;
                idx += readBytes;

                if (readBytes == 0)
                    throw new SocketException((int)SocketError.ConnectionReset);
            }

            return buffer;
        }

        private byte[] ReadFromBuffer(int totalSize)
        {
            var buffer = new byte[totalSize];

            var idx = 0;
            var remainder = totalSize;

            while (remainder > 0)
            {
                int readBytes = 0;
                using (var cancellationTokenSource = new CancellationTokenSource(socketTimeout))
                using (cancellationTokenSource.Token.Register(() => transportStream.Close()))
                    readBytes = transportStream.Read(buffer, idx, remainder);

                remainder -= readBytes;
                idx += readBytes;

                if (readBytes == 0)
                    throw new SocketException((int)SocketError.ConnectionReset);
            }

            return buffer;
        }
    }
}
