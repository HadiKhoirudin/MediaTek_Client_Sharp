using mtkclient.library.xflash;
using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace mtkclient.devicehandler
{
    internal class MtkSerialDevice : IMtkSerialDevice, IMtkDevice, IDisposable
    {
        private readonly object m_disposeLock;

        private SerialPort m_port;

        public MtkSerialDevice(SerialPort port)
        {
            m_disposeLock = new object();
            m_port = port;
        }

        public Task ConnectAsync()
        {
            if (m_port == null)
            {
                throw new ObjectDisposedException("MtkSerialDevice");
            }
            m_port.Open();
            m_port.BaseStream.WriteTimeout = m_port.WriteTimeout;
            m_port.BaseStream.ReadTimeout = m_port.ReadTimeout;
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            lock (m_disposeLock)
            {
                if (m_port != null)
                {
                    try
                    {
                        if (m_port.IsOpen)
                        {
                            m_port.DiscardInBuffer();
                        }
                    }
                    catch { }
                    try
                    {
                        if (m_port.IsOpen)
                        {
                            m_port.DiscardOutBuffer();
                        }
                    }
                    catch { }
                    try
                    {
                        m_port.Dispose();
                    }
                    catch { }
                    m_port = null;
                }
            }
        }

        public async Task<int> ReadAsync(
            byte[] buffer,
            int offset,
            int length,
            CancellationToken cancellationToken
        )
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (m_port != null)
            {
                CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(
                    cancellationToken
                );
                Task<int> readTask = m_port.BaseStream.ReadAsync(buffer, offset, length, cts.Token);
                Task timeoutTask = Task.Delay(m_port.ReadTimeout);
                await Task.WhenAny(readTask, timeoutTask);
                if (timeoutTask.IsCompleted && !readTask.IsCompleted)
                {
                    cts.Cancel();
                    throw new TimeoutException();
                }
                return await readTask;
            }
            throw new InvalidOperationException("Port not connected");
        }

        public async Task<byte[]> ReadCustomAsync(CancellationToken cancellationToken, int len)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (m_port != null)
            {
                CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(
                    cancellationToken
                );
                byte[] buffer = new byte[len];
                Task<int> readTask = m_port.BaseStream.ReadAsync(
                    buffer,
                    0,
                    buffer.Length,
                    cts.Token
                );
                Task timeoutTask = Task.Delay(m_port.ReadTimeout);
                await Task.WhenAny(readTask, timeoutTask);
                if (timeoutTask.IsCompleted && !readTask.IsCompleted)
                {
                    cts.Cancel();
                    throw new TimeoutException();
                }
                int total = (await readTask);
                var response = new byte[total];
                Array.Copy(buffer, 0, response, 0, total);
                return response;
            }
            throw new InvalidOperationException("Port not connected");
        }

        public async Task WriteAsync(
            byte[] buffer,
            int offset,
            int length,
            CancellationToken cancellationToken
        )
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (m_port != null)
            {
                CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(
                    cancellationToken
                );
                Task writeTask = m_port.BaseStream.WriteAsync(buffer, offset, length, cts.Token);
                Task timeoutTask = Task.Delay(m_port.WriteTimeout);
                await Task.WhenAny(writeTask, timeoutTask);
                if (timeoutTask.IsCompleted && !writeTask.IsCompleted)
                {
                    cts.Cancel();
                    throw new TimeoutException();
                }
                return;
            }
            throw new InvalidOperationException("Port not connected");
        }

        public override string ToString()
        {
            SerialPort port = m_port;
            object obj = null;
            if (port != null)
            {
                obj = port.PortName;
                if (obj != null)
                {
                    return (string)obj;
                }
            }
            else
            {
                obj = null;
            }
            obj = "MtkSerialDevice";
            return (string)obj;
        }
    }
}
