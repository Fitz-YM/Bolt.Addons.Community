﻿using Unity.VisualScripting;
using System;
using UnityEngine;
using Bolt.Addons.Integrations.Continuum;

namespace Bolt.Addons.Community.Code.Editor
{
    [Serializable][Inspectable]
    public sealed class FieldDeclaration : ISerializationCallbackReceiver
    {
        [Inspectable]
        public string name;
        [Inspectable]
        public Type type = typeof(int);
        [Inspectable]
        public bool inspectable = true;
        [Inspectable]
        public bool serialized = true;

        [SerializeField]
        [HideInInspector]
        private string qualifiedName;

        public void OnAfterDeserialize()
        {
            if (!(string.IsNullOrWhiteSpace(qualifiedName) || string.IsNullOrEmpty(qualifiedName)))
            {
                type = Type.GetType(qualifiedName);
            }
        } 

        public void OnBeforeSerialize()
        {
            if (type == null)
            {
                qualifiedName = string.Empty;
                return;
            }

            qualifiedName = type.AssemblyQualifiedName;
        }
    }
}
