﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Reflection;
using System;
using Unity.VisualScripting;

namespace Bolt.Addons.Community.Utility.Editor
{
    [Inspector(typeof(UnitButton))]
    [RenamedFrom("Bolt.Community.Addons.Utility.Editor.UnitButtonInspector")]
    public class UnitButtonInspector : Inspector
    {
        public UnitButtonInspector(Metadata metadata) : base(metadata) { }

        protected override float GetHeight(float width, GUIContent label)
        {
            return 16;
        }

        protected override void OnGUI(Rect position, GUIContent label)
        {
            BeginBlock(metadata, position);

            var buttonPosition = new Rect(
                position.x,
                position.y,
                position.width + 8,
                16
                );

            if (GUI.Button(buttonPosition, "Trigger", new GUIStyle(UnityEditor.EditorStyles.miniButton)))
            {
                var attribute = metadata.GetAttribute<UnitButtonAttribute>(true);

                if (attribute != null)
                {
                    var method = attribute.action;

                    object typeObject = metadata.parent.value;
                    GraphReference reference = GraphWindow.activeReference;
                    typeObject.GetType().GetMethod(method).Invoke(typeObject, new object[1] { reference });

                }
            }

            if (EndBlock(metadata))
            {
                metadata.RecordUndo();
            }
        }
    }
}