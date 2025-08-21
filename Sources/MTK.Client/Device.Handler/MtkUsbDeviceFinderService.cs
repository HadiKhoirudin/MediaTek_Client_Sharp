using mtkclient.library.xflash;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using static mtkclient.gui;

namespace mtkclient.devicehandler
{
    internal class MtkUsbDeviceFinderService
    {
        private static Native.LibUsbContext? m_context { get; set; }
        private const int LIBUSB_OPTION_USE_USBDK = 1;
        static MtkUsbDeviceFinderService()
        {
            Console.WriteLine("MtkUsbDeviceFinderService initialized...");

            Native.LibUsbContext context = new Native.LibUsbContext();
            if (Native.LibUsb1.libusb_init(out context) != 0)
            {
                Richlog("Unable to init libusb", Color.Black, false, true);
                return;
            }
            Console.WriteLine("LibUSB Initialized");
            int num = Native.LibUsb1.libusb_set_option(context, LIBUSB_OPTION_USE_USBDK);
            if (num != 0)
            {
                Richlog("Unable to enable usbdk: " + num, Color.Black, false, true);
            }
            else
            {
                Console.WriteLine("LibUSB USBDK Initialized");
                m_context = context;
            }
        }

        private static Native.LibUsbDeviceDescriptor? GetDescriptor(Native.LibUsbDevice device)
        {
            Native.LibUsbDeviceDescriptor descriptor = new Native.LibUsbDeviceDescriptor();
            if (Native.LibUsb1.libusb_get_device_descriptor(device, out descriptor) != 0)
            {
                return null;
            }
            return descriptor;
        }

        private static IMtkUsbDevice[] Find()
        {
            if (!m_context.HasValue)
            {
                Console.WriteLine("Failed to initialize libusb");
            }
            Native.Ptr<Native.LibUsbDevice> list = new Native.Ptr<Native.LibUsbDevice>();
            int count = Native.LibUsb1.libusb_get_device_list(m_context.Value, out list.Pointer).ToInt32();
            try
            {
                Native.LibUsbDevice[] array = list.Get(count);
                List<MtkUsbDevice> list2 = new List<MtkUsbDevice>();
                for (int i = 0; i < array.Length; i++)
                {
                    Native.LibUsbDeviceDescriptor? descriptor = GetDescriptor(array[i]);
                    if (
                        descriptor.HasValue
                        && descriptor.Value.IdVendor == 3725
                        && descriptor.Value.IdProduct == 3
                    )
                    {
                        list2.Add(new MtkUsbDevice(m_context.Value, array[i], descriptor.Value));
                    }
                }
                return list2.ToArray();
            }
            finally
            {
                Native.LibUsb1.libusb_free_device_list(list.Pointer, 0);
            }
        }

        public static Task<IMtkUsbDevice[]> FindAsync()
        {
            return Task.Run((Func<IMtkUsbDevice[]>)Find);
        }
    }
}
