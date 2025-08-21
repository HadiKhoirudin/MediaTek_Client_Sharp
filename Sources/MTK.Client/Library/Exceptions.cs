using System;

namespace mtkclient.library
{
    internal class DeviceSecurityNotSupportedException : Exception
    {
        public DeviceSecurityNotSupportedException()
            : base("Device security not supported") { }
    }
}
