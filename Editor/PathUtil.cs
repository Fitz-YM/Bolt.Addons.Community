﻿using Ludiq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Bolt.Addons.Community
{
    public static partial class PathUtil
    {

        private static string _rootCommunityFolder;
        public static string RootCommunityFolder
        {
            get
            {
                if (string.IsNullOrEmpty(_rootCommunityFolder))
                {
                    _rootCommunityFolder = PathUtil.PathOf("bolt_addons_community_root");
                }

                return _rootCommunityFolder;
            }
        }

        public static string CommunityEditorFolder => RootCommunityFolder + "Community/Editor/";
        public static string CommunityRuntimeFolder => RootCommunityFolder + "Community/Runtime/";

        public static string FundamentalsEditorFolder => CommunityEditorFolder + "Fundamentals/";
        public static string FundamentalsRuntimeFolder => CommunityRuntimeFolder + "Fundamentals/";

        public static string EventsEditorFolder => CommunityEditorFolder + "Events/";
        public static string EventsRuntimeFolder => CommunityRuntimeFolder + "Events/";

        private static Dictionary<string, MultiTexture> collection = new Dictionary<string, MultiTexture>();

        public static EditorTexture Load(string name, CommunityEditorPath path)
        {
            if (collection.ContainsKey(name))
            {
                return GetStateTexture(name);
            }

            var _path = string.Empty;
            var multiTex = new MultiTexture();

            switch (path)
            {
                case CommunityEditorPath.Community:
                    _path = CommunityEditorFolder + "Resources/";
                    break;

                case CommunityEditorPath.Fundamentals:
                    _path = FundamentalsEditorFolder + "Resources/";
                    break;

                case CommunityEditorPath.Events:
                    _path = EventsEditorFolder + "Resources/";
                    break;
            }

            multiTex.personal = EditorTexture.Single(AssetDatabase.LoadAssetAtPath<Texture2D>(_path + name + ".png"));
            multiTex.pro = EditorTexture.Single(AssetDatabase.LoadAssetAtPath<Texture2D>(_path + name + "@Pro.png"));

            collection.Add(name, multiTex);

            return GetStateTexture(name);
        }

        private static EditorTexture GetStateTexture(string name)
        {
            if (EditorGUIUtility.isProSkin)
            {
                if (collection[name].pro == null)
                {
                    return collection[name].personal;
                }

                return collection[name].pro;
            }

            return collection[name].personal;
        }

        private static string PathOf(string fileName)
        {
            var files = UnityEditor.AssetDatabase.FindAssets(fileName);
            if (files.Length == 0) return string.Empty;
            var assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(files[0]).Replace(fileName, string.Empty);
            return assetPath;
        }
    }
}
