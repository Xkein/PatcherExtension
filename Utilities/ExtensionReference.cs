using Extension.Ext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Utilities
{
    class ExtensionReference<TExt> where TExt : class, IExtension
    {
        WeakReference<TExt> weakReference;

        public ExtensionReference(TExt ext)
        {
            weakReference = new WeakReference<TExt>(ext);
        }
        public void Set(TExt ext)
        {
            weakReference.SetTarget(ext);
        }
        public TExt Get()
        {
            if (weakReference.TryGetTarget(out TExt result))
            {
                if (result.OwnerObject != IntPtr.Zero)
                {
                    return result;
                }
            }
            return null;
        }

        public static implicit operator TExt(ExtensionReference<TExt> reference) => reference.Get();
        public static implicit operator ExtensionReference<TExt>(TExt ext) => new ExtensionReference<TExt>(ext);
    }
}
