using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace mtkclient.devicehandler
{

    internal static class Native
    {
        internal struct Ptr<T>
        {
            public IntPtr Pointer;

            public T Get()
            {
                if (Pointer == IntPtr.Zero)
                {
                    throw new ArgumentException("Pointer is null");
                }
                return Marshal.PtrToStructure<T>(Pointer);
            }

            public T[] Get(int count)
            {
                T[] array = new T[count];
                int num = Marshal.SizeOf<T>();
                for (int i = 0; i < count; i++)
                {
                    IntPtr ptr = new IntPtr(Pointer.ToInt64() + i * num);
                    array[i] = Marshal.PtrToStructure<T>(ptr);
                }
                return array;
            }
        }

        internal struct LibUsbContext
        {
            public IntPtr Value;
        }

        internal struct LibUsbDevice
        {
            public IntPtr Value;
        }

        internal struct LibUsbDeviceHandle
        {
            public IntPtr Value;
        }

        internal struct LibUsbDeviceDescriptor
        {
            public byte Length;

            public byte DescriptorType;

            public ushort USB;

            public byte DeviceClass;

            public byte DeviceSubClass;

            public byte DeviceProtocol;

            public byte MaxPacketSize0;

            public ushort IdVendor;

            public ushort IdProduct;

            public ushort Device;

            public byte Manufacturer;

            public byte Product;

            public byte SerialNumber;

            public byte NumConfigurations;
        }

        internal struct LibUsbEndpointDescriptor
        {
            public byte Length;

            public byte DescriptorType;

            public byte EndpointAddress;

            public byte Attributes;

            public ushort MaxPacketSize;

            public byte Interval;

            public byte Refresh;

            public byte SynchAddress;

            public IntPtr Extra;

            public int ExtraLength;
        }

        internal struct LibUsbInterfaceDescriptor
        {
            public byte Length;

            public byte DescriptorType;

            public byte InterfaceNumber;

            public byte AlternateSetting;

            public byte NumEndpoints;

            public byte InterfaceClass;

            public byte InterfaceSubClass;

            public byte InterfaceProtocol;

            public byte Interface;

            public Ptr<LibUsbEndpointDescriptor> Endpoint;

            public IntPtr Extra;

            public int ExtraLength;
        }

        internal struct LibUsbInterface
        {
            public Ptr<LibUsbInterfaceDescriptor> AltSetting;

            public int NumAltSetting;
        }

        internal struct LibUsbConfigDescriptor
        {
            public byte Length;

            public byte DescriptorType;

            public ushort TotalLength;

            public byte NumInterfaces;

            public byte ConfigurationValue;

            public byte Configuration;

            public byte Attributes;

            public byte MaxPower;

            public Ptr<LibUsbInterface> Interface;

            public IntPtr Extra;

            public int ExtraLength;
        }

        internal static class LibUsb1
        {
            // Handle untuk pustaka yang dimuat
            private static IntPtr libusbHandle;
            private static string libusb32_dll = "libusb32-1.0.dll";
            private static string libusb_dll = "libusb-1.0.dll";

            // Delegate Definitions
            private delegate int LibUsbInitDelegate(out LibUsbContext context);

            private delegate IntPtr LibUsbGetDeviceListDelegate(LibUsbContext context, out IntPtr list);
            private delegate void LibUsbFreeDeviceListDelegate(IntPtr list, int count);

            private delegate int LibUsbGetDeviceDescriptorDelegate(LibUsbDevice device, out LibUsbDeviceDescriptor descriptor);
            private delegate int LibUsbGetConfigDescriptorDelegate(LibUsbDevice device, byte index, out IntPtr config);

            private delegate int LibUsbOpenDelegate(LibUsbDevice device, out LibUsbDeviceHandle handle);
            private delegate int LibUsSetOptionDelegate(LibUsbContext context, int option);

            private delegate int LibUsbClaimInterfaceDelegate(LibUsbDeviceHandle handle, int interfaceNumber);
            private delegate int LibUsbReleaseInterfaceDelegate(LibUsbDeviceHandle handle, int interfaceNumber);

            private delegate int LibUsbControlTransferDelegate(LibUsbDeviceHandle handle, byte requestType, byte request, ushort value, ushort index, [Out] byte[] data, ushort dataLength, int timeout);
            private delegate int LibUsbBulkTransferDelegate(LibUsbDeviceHandle handle, byte endpoint, byte[] data, int length, out int transferred, int timeout);

            private delegate int LibUsbResetDeviceDelegate(LibUsbDeviceHandle handle);
            private delegate void LibUsbUnRefDeviceDelegate(LibUsbDevice device);
            private delegate int LibUsbCloseDelegate(LibUsbDeviceHandle handle);
            private delegate int LibUsbExitDelegate(LibUsbContext context);

            // Dynamic delegates
            private static LibUsbInitDelegate iReverse_libusb_init;

            private static LibUsbGetDeviceListDelegate iReverse_libusb_get_device_list;
            private static LibUsbFreeDeviceListDelegate iReverse_libusb_free_device_list;

            private static LibUsbGetDeviceDescriptorDelegate iReverse_libusb_get_device_descriptor;
            private static LibUsbGetConfigDescriptorDelegate iReverse_libusb_get_config_descriptor;

            private static LibUsbOpenDelegate iReverse_libusb_open;
            private static LibUsSetOptionDelegate iReverse_libusb_set_option;

            private static LibUsbClaimInterfaceDelegate iReverse_libusb_claim_interface;
            private static LibUsbReleaseInterfaceDelegate iReverse_libusb_release_interface;

            private static LibUsbControlTransferDelegate iReverse_libusb_control_transfer;
            private static LibUsbBulkTransferDelegate iReverse_libusb_bulk_transfer;

            private static LibUsbResetDeviceDelegate iReverse_libusb_reset_device;
            private static LibUsbUnRefDeviceDelegate iReverse_libusb_unref_device;
            private static LibUsbCloseDelegate iReverse_libusb_close;
            private static LibUsbExitDelegate iReverse_libusb_exit;

            // Load the library dynamically
            public static void LoadLibrary()
            {
                if (SetDllDirectory($@"{Path.GetDirectoryName(Application.ExecutablePath)}\Windows"))
                {
                    if (Environment.Is64BitProcess)
                        libusbHandle = LoadLibrary(libusb_dll);
                    else
                        libusbHandle = LoadLibrary(libusb32_dll);

                    if (libusbHandle == IntPtr.Zero) throw new Exception($"Failed to load {libusb_dll}");

                    // Bind delegates
                    iReverse_libusb_init = GetDelegate<LibUsbInitDelegate>("libusb_init");

                    iReverse_libusb_get_device_list = GetDelegate<LibUsbGetDeviceListDelegate>("libusb_get_device_list");
                    iReverse_libusb_free_device_list = GetDelegate<LibUsbFreeDeviceListDelegate>("libusb_free_device_list");

                    iReverse_libusb_get_device_descriptor = GetDelegate<LibUsbGetDeviceDescriptorDelegate>("libusb_get_device_descriptor");
                    iReverse_libusb_get_config_descriptor = GetDelegate<LibUsbGetConfigDescriptorDelegate>("libusb_get_config_descriptor");

                    iReverse_libusb_open = GetDelegate<LibUsbOpenDelegate>("libusb_open");
                    iReverse_libusb_set_option = GetDelegate<LibUsSetOptionDelegate>("libusb_set_option");

                    iReverse_libusb_claim_interface = GetDelegate<LibUsbClaimInterfaceDelegate>("libusb_claim_interface");
                    iReverse_libusb_release_interface = GetDelegate<LibUsbReleaseInterfaceDelegate>("libusb_release_interface");

                    iReverse_libusb_control_transfer = GetDelegate<LibUsbControlTransferDelegate>("libusb_control_transfer");
                    iReverse_libusb_bulk_transfer = GetDelegate<LibUsbBulkTransferDelegate>("libusb_bulk_transfer");

                    iReverse_libusb_reset_device = GetDelegate<LibUsbResetDeviceDelegate>("libusb_reset_device");
                    iReverse_libusb_unref_device = GetDelegate<LibUsbUnRefDeviceDelegate>("libusb_unref_device");
                    iReverse_libusb_close = GetDelegate<LibUsbCloseDelegate>("libusb_close");
                    iReverse_libusb_exit = GetDelegate<LibUsbExitDelegate>("libusb_exit");
                }
            }


            public static void UnloadLibrary()
            {
                if (libusbHandle != IntPtr.Zero)
                {
                    FreeLibrary(libusbHandle);
                    libusbHandle = IntPtr.Zero;
                }
            }

            private static TDelegate GetDelegate<TDelegate>(string functionName) where TDelegate : Delegate
            {
                IntPtr functionPtr = GetProcAddress(libusbHandle, functionName);
                if (functionPtr == IntPtr.Zero)
                {
                    throw new Exception($"Failed to get function address for {functionName}");
                }
                return Marshal.GetDelegateForFunctionPointer<TDelegate>(functionPtr);
            }

            public static int libusb_init(out LibUsbContext context)
            {
                return iReverse_libusb_init(out context);
            }


            public static IntPtr libusb_get_device_list(LibUsbContext context, out IntPtr list)
            {
                return iReverse_libusb_get_device_list(context, out list);
            }

            public static void libusb_free_device_list(IntPtr list, int count)
            {
                iReverse_libusb_free_device_list(list, count);
            }


            public static int libusb_get_device_descriptor(LibUsbDevice device, out LibUsbDeviceDescriptor descriptor)
            {
                return iReverse_libusb_get_device_descriptor(device, out descriptor);
            }

            public static int libusb_get_config_descriptor(LibUsbDevice device, byte index, out Ptr<LibUsbConfigDescriptor> config)
            {
                int result = iReverse_libusb_get_config_descriptor(device, index, out var conf);
                config = new Ptr<LibUsbConfigDescriptor> { Pointer = conf };
                return result;
            }


            public static int libusb_open(LibUsbDevice device, out LibUsbDeviceHandle handle)
            {
                return iReverse_libusb_open(device, out handle);
            }

            public static int libusb_set_option(LibUsbContext context, int option)
            {
                return iReverse_libusb_set_option(context, option);
            }


            public static int libusb_claim_interface(LibUsbDeviceHandle handle, int interfaceNumber)
            {
                return iReverse_libusb_claim_interface(handle, interfaceNumber);
            }

            public static int libusb_release_interface(LibUsbDeviceHandle handle, int interfaceNumber)
            {
                return iReverse_libusb_release_interface(handle, interfaceNumber);
            }

            public static int libusb_control_transfer(LibUsbDeviceHandle handle, byte requestType, byte request, ushort value, ushort index, [Out] byte[] data, ushort dataLength, int timeout)
            {
                return iReverse_libusb_control_transfer(handle, requestType, request, value, index, data, dataLength, timeout);
            }

            public static int libusb_bulk_transfer(LibUsbDeviceHandle handle, byte endpoint, byte[] data, int length, out int transferred, int timeout)
            {
                return iReverse_libusb_bulk_transfer(handle, endpoint, data, length, out transferred, timeout);
            }

            public static int libusb_reset_device(LibUsbDeviceHandle handle)
            {
                return iReverse_libusb_reset_device(handle);
            }

            public static int libusb_close(LibUsbDeviceHandle handle)
            {
                return iReverse_libusb_close(handle);
            }

            public static void libusb_unref_device(LibUsbDevice device)
            {
                iReverse_libusb_unref_device(device);
            }

            public static int libusb_exit(LibUsbContext context)
            {
                return iReverse_libusb_exit(context);
            }

            // DLL Import for dynamic loading
            [DllImport("kernel32.dll", SetLastError = true)]
            static extern bool SetDllDirectory(string lpPathName);

            [DllImport("kernel32", SetLastError = true)]
            private static extern IntPtr LoadLibrary(string lpFileName);

            [DllImport("kernel32", SetLastError = true)]
            private static extern bool FreeLibrary(IntPtr hModule);

            [DllImport("kernel32", SetLastError = true)]
            private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);
        }
    }
}
