﻿using Bolt.Addons.Libraries.CSharp;
using Unity.VisualScripting;
using System;

namespace Bolt.Addons.Community.Fundamentals.Editor.UnitOptions
{
    [FuzzyOption(typeof(ActionInvokeUnit))]
    public sealed class ActionInvokeUnitOption : UnitOption<ActionInvokeUnit>
    {
        [Obsolete()]
        public ActionInvokeUnitOption() : base() { }

        public ActionInvokeUnitOption(ActionInvokeUnit unit) : base(unit)
        {

        }

        protected override string Label(bool human)
        {
            if (unit._delegate == null) return "Invoke Action";
            return $"{LudiqGUIUtility.DimString("Invoke")} { unit._delegate?.DisplayName }";
        }

        protected override UnitCategory Category()
        {
            var @namespace = unit._delegate == null ? string.Empty : unit._delegate.GetDelegateType().Namespace;
            @namespace = (string.IsNullOrEmpty(@namespace) ? string.Empty : @namespace).PeriodsToSlashes();
            return new UnitCategory(base.Category().fullName + (unit._delegate == null ? string.Empty : "/" + @namespace));
        }
    }
}