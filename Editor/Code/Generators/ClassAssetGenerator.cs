﻿using Bolt.Addons.Community.Generation;
using Bolt.Addons.Libraries.CSharp;
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Bolt.Addons.Community.Code.Editor
{
    [Serializable]
    [CodeGenerator(typeof(ClassAsset))]
    public sealed class ClassAssetGenerator : MemberTypeAssetGenerator<ClassAsset, ClassFieldDeclaration, ClassMethodDeclaration, ClassConstructorDeclaration>
    {
        protected override TypeGenerator OnGenerateType(ref string output, NamespaceGenerator @namespace)
        {
            var @class = ClassGenerator.Class(RootAccessModifier.Public, ClassModifier.None, Data.title.LegalMemberName(), Data.scriptableObject ? typeof(ScriptableObject) : typeof(object));
            if (Data.definedEvent) @class.ImplementInterface(typeof(IDefinedEvent));
            if (Data.inspectable) @class.AddAttribute(AttributeGenerator.Attribute<InspectableAttribute>());
            if (Data.serialized) @class.AddAttribute(AttributeGenerator.Attribute<SerializableAttribute>());
            if (Data.includeInSettings) @class.AddAttribute(AttributeGenerator.Attribute<IncludeInSettingsAttribute>().AddParameter(true));
            if (Data.scriptableObject) @class.AddAttribute(AttributeGenerator.Attribute<CreateAssetMenuAttribute>().AddParameter("menuName", Data.menuName).AddParameter("fileName", Data.fileName).AddParameter("order", Data.order));

            for (int i = 0; i < Data.constructors.Count; i++)
            {
                var constructor = ConstructorGenerator.Constructor(Data.constructors[i].scope, Data.constructors[i].modifier, Data.title.LegalMemberName());
                if (Data.constructors[i].graph.units.Count > 0)
                {
                    var unit = Data.constructors[i].graph.units[0] as FunctionUnit;
                    constructor.Body(FunctionUnitGenerator.GetSingleDecorator(unit, unit).GenerateControl(null, new ControlGenerationData(), 0));

                    for (int pIndex = 0; pIndex < Data.constructors[i].parameters.Count; pIndex++)
                    {
                        if (!string.IsNullOrEmpty(Data.constructors[i].parameters[pIndex].name)) constructor.AddParameter(false, ParameterGenerator.Parameter(Data.constructors[i].parameters[pIndex].name, Data.constructors[i].parameters[pIndex].type, ParameterModifier.None));
                    }
                }

                @class.AddConstructor(constructor);
            }

            for (int i = 0; i < Data.variables.Count; i++)
            {
                if (!string.IsNullOrEmpty(Data.variables[i].name) && Data.variables[i].type != null)
                {
                    if (Data.variables[i].isProperty)
                    {
                        var property = PropertyGenerator.Property(Data.variables[i].scope, Data.variables[i].propertyModifier, Data.variables[i].type, Data.variables[i].name, false);
                        if (Data.serialized)
                        {
                            if (Data.variables[i].inspectable) property.AddAttribute(AttributeGenerator.Attribute<InspectableAttribute>());
                            if (!Data.variables[i].serialized) property.AddAttribute(AttributeGenerator.Attribute<NonSerializedAttribute>());

                            if (Data.variables[i].get)
                            {
                                property.MultiStatementGetter(AccessModifier.Public, UnitGenerator.GetSingleDecorator(Data.variables[i].getter.graph.units[0] as Unit, Data.variables[i].getter.graph.units[0] as Unit)
                                .GenerateControl(null, new ControlGenerationData() { returns = Data.variables[i].type }, 0));
                            }

                            if (Data.variables[i].set)
                            {
                                property.MultiStatementSetter(AccessModifier.Public, UnitGenerator.GetSingleDecorator(Data.variables[i].setter.graph.units[0] as Unit, Data.variables[i].setter.graph.units[0] as Unit)
                                .GenerateControl(null, new ControlGenerationData(), 0));
                            }
                        }

                        @class.AddProperty(property);
                    }
                    else
                    {
                        var field = FieldGenerator.Field(Data.variables[i].scope, Data.variables[i].fieldModifier, Data.variables[i].type, Data.variables[i].name);

                        if (Data.serialized)
                        {
                            if (Data.variables[i].inspectable) field.AddAttribute(AttributeGenerator.Attribute<InspectableAttribute>());
                            if (!Data.variables[i].serialized) field.AddAttribute(AttributeGenerator.Attribute<NonSerializedAttribute>());
                        }

                        @class.AddField(field);
                    }
                }
            }

            for (int i = 0; i < Data.methods.Count; i++)
            {
                if (!string.IsNullOrEmpty(Data.methods[i].name) && Data.methods[i].returnType != null)
                {
                    var method = MethodGenerator.Method(Data.methods[i].scope, Data.methods[i].modifier, Data.methods[i].returnType, Data.methods[i].name);
                    if (Data.methods[i].graph.units.Count > 0)
                    {
                        var unit = Data.methods[i].graph.units[0] as FunctionUnit;
                        method.Body(FunctionUnitGenerator.GetSingleDecorator(unit, unit).GenerateControl(null, new ControlGenerationData(), 0));
                        
                        for (int pIndex = 0; pIndex < Data.methods[i].parameters.Count; pIndex++)
                        {
                            if (!string.IsNullOrEmpty(Data.methods[i].parameters[pIndex].name)) method.AddParameter(ParameterGenerator.Parameter(Data.methods[i].parameters[pIndex].name, Data.methods[i].parameters[pIndex].type, ParameterModifier.None));
                        }
                    }

                    @class.AddMethod(method);
                }
            }

            @namespace.AddClass(@class);

            return @class;
        }
    }
}
