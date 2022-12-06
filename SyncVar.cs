using System;
using System.Runtime.InteropServices;
using LiteEntitySystem.Internal;

namespace LiteEntitySystem
{
    [Flags]
    public enum SyncFlags : byte
    {
        None                = 0,
        Interpolated        = 1,
        LagCompensated      = 1 << 1,
        OnlyForOtherPlayers = 1 << 2,
        OnlyForOwner        = 1 << 3,
        AlwaysPredict       = 1 << 4
    }
    
    [AttributeUsage(AttributeTargets.Field)]
    public class SyncVarFlags : Attribute
    {
        internal readonly SyncFlags Flags;

        public SyncVarFlags(SyncFlags flags)
        {
            Flags = flags;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct SyncVar<T> where T : unmanaged
    {
        public T Value;

        public SyncVar(T value)
        {
            Value = value;
        }

        public static implicit operator T(SyncVar<T> sv)
        {
            return sv.Value;
        }
        
        public static implicit operator SyncVar<T>(T v)
        {
            return new SyncVar<T> { Value = v };
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
        
        public override bool Equals(object o)
        {
            return this == (SyncVar<T>)o;
        }

        private static readonly UIntPtr SizeU = new ((uint)sizeof(T));

        public static bool operator==(SyncVar<T> a, SyncVar<T> b)
        {
            return Utils.memcmp(&a, &b, SizeU) == 0;
        }
        
        public static bool operator!=(SyncVar<T> a, SyncVar<T> b)
        {
            return Utils.memcmp(&a, &b, SizeU) != 0;
        }
        
        public static bool operator==(T a, SyncVar<T> b)
        {
            return Utils.memcmp(&a, &b.Value, SizeU) == 0;
        }
        
        public static bool operator!=(T a, SyncVar<T> b)
        {
            return Utils.memcmp(&a, &b.Value, SizeU) != 0;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct SyncVarWithNotify<T> where T : unmanaged
    {
        public T Value;
        internal byte FieldId;
        
        public SyncVarWithNotify(T value)
        {
            Value = value;
            FieldId = 0;
        }
        
        public static implicit operator T(SyncVarWithNotify<T> sv)
        {
            return sv.Value;
        }
        
        public static implicit operator SyncVarWithNotify<T>(T v)
        {
            return new SyncVarWithNotify<T> { Value = v };
        }
        
        public static implicit operator SyncVar<T>(SyncVarWithNotify<T> sv)
        {
            return new SyncVar<T>(sv.Value);
        }
        
        public static implicit operator SyncVarWithNotify<T>(SyncVar<T> v)
        {
            return new SyncVarWithNotify<T> { Value = v.Value };
        }
        
        public override string ToString()
        {
            return Value.ToString();
        }
        
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
        
        public override bool Equals(object o)
        {
            return this == (SyncVarWithNotify<T>)o;
        }

        private static readonly UIntPtr SizeU = new ((uint)sizeof(T));
        
        public static bool operator==(SyncVarWithNotify<T> a, SyncVarWithNotify<T> b)
        {
            return Utils.memcmp(&a.Value, &b.Value, SizeU) == 0;
        }
        
        public static bool operator!=(SyncVarWithNotify<T> a, SyncVarWithNotify<T> b)
        {
            return Utils.memcmp(&a.Value, &b.Value, SizeU) != 0;
        }
        
        public static bool operator==(SyncVar<T> a, SyncVarWithNotify<T> b)
        {
            return Utils.memcmp(&a.Value, &b.Value, SizeU) == 0;
        }
        
        public static bool operator!=(SyncVar<T> a, SyncVarWithNotify<T> b)
        {
            return Utils.memcmp(&a.Value, &b.Value, SizeU) != 0;
        }
        
        public static bool operator==(T a, SyncVarWithNotify<T> b)
        {
            return Utils.memcmp(&a, &b.Value, SizeU) == 0;
        }
        
        public static bool operator!=(T a, SyncVarWithNotify<T> b)
        {
            return Utils.memcmp(&a, &b.Value, SizeU) != 0;
        }
    }
}