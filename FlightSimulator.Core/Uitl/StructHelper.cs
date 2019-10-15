using System;
using System.Runtime.InteropServices;

namespace THUFlightDataAdapterApp.Util
{
    public static class StructHelper
    {
        /// <summary>
        /// 将字节数组变为类型为type的结构体 
        /// 相当于C语言中一个结构体Strcut和字节组数存放在共用体Union中
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object BytesToStruct(byte[] bytes, Type type)
        {
            int num = Marshal.SizeOf(type);
            if (num > bytes.Length)
            {
                return null;
            }
            IntPtr intPtr = Marshal.AllocHGlobal(num);
            Marshal.Copy(bytes, 0, intPtr, num);
            object result = Marshal.PtrToStructure(intPtr, type);
            Marshal.FreeHGlobal(intPtr);
            return result;
        }

        /// <summary>
        /// 将字节数组变为类型为T的结构体 
        /// 相当于C语言中一个结构体Strcut和字节组数存放在共用体Union中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static T BytesToStruct<T>(byte[] bytes)
        {
            var type = typeof(T);
            int num = Marshal.SizeOf(type);
            if (num > bytes.Length)
            {
                return default;
            }
            IntPtr intPtr = Marshal.AllocHGlobal(num);
            Marshal.Copy(bytes, 0, intPtr, num);
            var result = Marshal.PtrToStructure(intPtr, type);
            Marshal.FreeHGlobal(intPtr);
            return (T)result;
        }

        /// <summary>
        /// 将结构体中的数据类型转变为一个字节数组
        /// </summary>
        /// <param name="structObj"></param>
        /// <returns></returns>
        public static byte[] StructToBytes(object structObj)
        {
            int num = Marshal.SizeOf(structObj);
            byte[] array = new byte[num];
            IntPtr intPtr = Marshal.AllocHGlobal(num);
            Marshal.StructureToPtr(structObj, intPtr, false);
            Marshal.Copy(intPtr, array, 0, num);
            Marshal.FreeHGlobal(intPtr);
            return array;
        }

        /// <summary>
        /// 或者结构体数据类型在内存中需要的大小
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static int GetStructSize<T>()
        {
            var type = typeof(T);
            int num = Marshal.SizeOf(type);
            return num;
        }

    }
}
