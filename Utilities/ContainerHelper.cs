using Extension.Ext;
using PatcherYRpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Utilities
{
    static class ContainerHelper
    {
        public static uint Write(this IStream stream, byte[] buffer)
        {
            uint written = 0;
            stream.Write(buffer, buffer.Length, Pointer<uint>.AsPointer(ref written));
            return written;
        }
        public static uint Write<T>(this IStream stream, T obj)
        {
            var ptr = Pointer<T>.AsPointer(ref obj);
            byte[] buffer = new byte[Pointer<T>.TypeSize()];
            Marshal.Copy(ptr, buffer, 0, buffer.Length);
            return stream.Write(buffer);
        }

        public static uint Read(this IStream stream, byte[] buffer)
        {
            uint written = 0;
            stream.Read(buffer, buffer.Length, Pointer<uint>.AsPointer(ref written));
            return written;
        }
        public static uint Read<T>(this IStream stream, ref T obj)
        {
            var ptr = Pointer<T>.AsPointer(ref obj);
            byte[] buffer = new byte[Pointer<T>.TypeSize()];
            uint written = stream.Read(buffer);
            Marshal.Copy(buffer, 0, ptr, buffer.Length);
            return written;
        }

        public static void Swizzle<TBase, T>(this Extension<TBase> ext, ref T obj)
        {
            SwizzleManagerClass.Instance.Swizzle(Pointer<T>.AsPointer(ref obj).Convert<IntPtr>());
        }
        public static void Here_I_Am<TBase, T>(this Extension<TBase> ext, Pointer<T> oldPtr, ref T obj)
        {
            SwizzleManagerClass.Instance.Here_I_Am((int)oldPtr, Pointer<T>.AsPointer(ref obj).Convert<IntPtr>());
        }
        public static void Here_I_Am<TBase, T>(this Extension<TBase> ext, IStream stream, ref T obj)
        {
            Pointer<T> oldPtr = Pointer<T>.Zero;
            stream.Read(ref oldPtr);
            ext.Here_I_Am(oldPtr, ref obj);
        }

        public static void Save<TBase, T>(this Extension<TBase> ext, IStream stream, PointerHandle<T> ptr)
        {
            stream.Write(ptr.Pointer);
        }
        public static void Load<TBase, T>(this Extension<TBase> ext, IStream stream, ref PointerHandle<T> ptr)
        {
            if(ptr == null)
            {
                ptr = new PointerHandle<T>();
            }
            stream.Read(ref ptr.Pointer);
            ext.Swizzle(ref ptr.Pointer);
        }
    }
}
