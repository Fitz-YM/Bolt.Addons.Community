﻿using Unity.VisualScripting;
using Bolt.Addons.Community.Utility;

namespace Bolt.Addons.Community.Fundamentals.Editor
{
    [Editor(typeof(FuncInvokeUnit))]
    public sealed class FuncInvokeUnitEditor : DelegateInvokeUnitEditor<FuncInvokeUnit, IFunc>
    {
        public FuncInvokeUnitEditor(Metadata metadata) : base(metadata)
        {
        }

        protected override string DefaultName => "Func";
    }
}