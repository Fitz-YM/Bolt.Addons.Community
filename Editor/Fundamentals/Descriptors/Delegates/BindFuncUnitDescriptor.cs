﻿using Bolt.Addons.Community.Utility;
using Unity.VisualScripting;

namespace Bolt.Addons.Community.Fundamentals.Editor
{
    [Descriptor(typeof(BindFuncUnit))]
    public sealed class BindFuncUnitDescriptor : BindDelegateUnitDescriptor<BindFuncUnit, IFunc>
    {
        public BindFuncUnitDescriptor(BindFuncUnit target) : base(target)
        {
        }

        protected override EditorTexture DefaultIcon()
        {
            return PathUtil.Load("func_bind", CommunityEditorPath.Fundamentals);
        }

        protected override string DefaultName()
        {
            return "Bind";
        }

        protected override EditorTexture DefinedIcon()
        {
            return PathUtil.Load("func_bind", CommunityEditorPath.Fundamentals);
        }

        protected override string DefinedName()
        {
            return "Bind";
        }
    }
}