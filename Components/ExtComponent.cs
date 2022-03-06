using Extension.Ext;
using Extension.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Components
{
    [Serializable]
    public class ExtComponent<TExt> : Component where TExt : class, IExtension
    {
        public ExtComponent(TExt owner, int id, string name) : base(id)
        {
            _owner = owner;
            Name = name;
        }

        public TExt Owner => _owner;
        public event Action OnAwake;

        public override void Awake()
        {
            base.Awake();

            _awaked = true;
            OnAwake?.Invoke();
            ForeachChild(c => c.Awake());
        }

        /// <summary>
        /// return myself and ensure awaked
        /// </summary>
        /// <returns></returns>
        public ExtComponent<TExt> GetEnsureAwaked()
        {
            if (!_awaked)
                Awake();

            return this;
        }

        bool _awaked = false;
        ExtensionReference<TExt> _owner;
    }
}
