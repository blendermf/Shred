﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Shred.Lib {
    class Util {
        public static void DumpGameObject(GameObject gameObject, string indent, bool fields = false) {
            
            Console.WriteLine("{0}+{1}", indent, gameObject.name);



            foreach (Component component in gameObject.GetComponents<Component>()) {
                DumpComponent(component, indent + "  ", fields);
            }

            foreach (Transform child in gameObject.transform) {
                DumpGameObject(child.gameObject, indent + "  ", fields);
            }
        }

        public static string DumpGameObjectToString(GameObject gameObject, string indent, bool fields = false)
        {
            List<string> strings = new List<string>();
            strings.Add(string.Format("{0}+{1}", indent, gameObject.name));
            
            foreach (Component component in gameObject.GetComponents<Component>())
            {
                strings.Add(DumpComponentToString(component, indent + "  ", fields));
            }

            foreach (Transform child in gameObject.transform)
            {
                strings.Add(DumpGameObjectToString(child.gameObject, indent + "  ", fields));
            }

            return string.Join("\n", strings.ToArray());
        }

        private static string FormatDumpValue(object value, string valName, string indent) {
            return value == null ? "(null)" : value.ToString().Replace("\n", "\n" + indent + new string(' ', valName.Length + 4));
        }

        public static void DumpComponent(Component component, string indent, bool fields = false) {
            
            Console.WriteLine("{0}{1}", indent, (component == null ? "(null)" : component.GetType().Name));

            if (fields) {
                List<string> lines = new List<string>();
                foreach (var thisVar in component.GetType().GetFields()) {
                    lines.Add(String.Format("{0}.{1} = {2}", indent + "  ", thisVar.Name, FormatDumpValue(thisVar.GetValue(component), thisVar.Name, indent + "  ")));
                }
                foreach (var thisVar in component.GetType().GetProperties()) {
                    lines.Add(String.Format("{0}.{1} = {2}", indent + "  ", thisVar.Name, FormatDumpValue(thisVar.GetValue(component, null), thisVar.Name, indent + "  ")));
                }

                lines.Sort();
                foreach (string line in lines) {
                    Console.WriteLine(line);
                }
            }
        }

        public static string DumpComponentToString(Component component, string indent, bool fields = false)
        {
            List<string> lines = new List<string>();
            if (fields)
            {
                foreach (var thisVar in component.GetType().GetFields())
                {
                    lines.Add(String.Format("{0}.{1} = {2}", indent + "  ", thisVar.Name, FormatDumpValue(thisVar.GetValue(component), thisVar.Name, indent + "  ")));
                }
                foreach (var thisVar in component.GetType().GetProperties())
                {
                    lines.Add(String.Format("{0}.{1} = {2}", indent + "  ", thisVar.Name, FormatDumpValue(thisVar.GetValue(component, null), thisVar.Name, indent + "  ")));
                }

                lines.Sort();
            }

            return string.Format("{0}{1}", indent, (component == null ? "(null)" : component.GetType().Name)) + (fields ? "\n" + string.Join("\n", lines.ToArray()) : "");
        }

        public static void DumpGameObjectCloneCompare(GameObject o1, GameObject o2, string indent) {
            Console.WriteLine("{0}+{1}", indent, o1.name);

            var o1c = o1.GetComponents<Component>();
            var o2c = o2.GetComponents<Component>();

            for (int i = 0; i < o1c.Length; i++) {
                DumpComponentCloneCompare(o1c[i], o2c[i], indent + "  ");
            }

            for (int i = 0; i < o1.transform.childCount; i++) {
                DumpGameObjectCloneCompare(o1.transform.GetChild(i).gameObject, o2.transform.GetChild(i).gameObject, indent + "  ");
            }
        }

        private static string FormatCompareDumpValues(object v1, object v2, string valName, string indent) {
            string output = "";
            var val1 = (v1 == null ? "(null)" : v1.ToString()).Split('\n');
            var val2 = (v2 == null ? "(null)" : v2.ToString()).Split('\n');

            int maxWidthVal1 = val1.Max(v => v.Length);
            int maxLines = Math.Max(val1.Length, val2.Length);

            for (int i = 0; i < maxLines; i++) {
                if (i < val1.Length) {
                    if (i == 0) {
                        output += val1[0];
                    } else {
                        output += '\n' + indent + new string(' ', valName.Length + 4) + val1[i];
                    }

                    if (i < val2.Length) {
                        output += $"{new string(' ', maxWidthVal1 - val1[i].Length)} | {val2[i]}";
                    }
                } else if (i < val2.Length) {
                    output += $"{indent}{new string(' ', valName.Length + 4 + maxWidthVal1)} | {val2[i]}";
                }
            }
            return output;
        }

        public static void DumpComponentCloneCompare(Component c1, Component c2, string indent) {
            Console.WriteLine("{0}{1}", indent, (c1 == null ? "(null)" : c1.GetType().Name));
            List<string> lines = new List<string>();
            var c1p = c1.GetType().GetProperties();
            var c2p = c2.GetType().GetProperties();
            for (var i = 0; i < c1p.Count(); i++) {
                var c1pv = c1p[i].GetValue(c1, null);
                var c2pv = c2p[i].GetValue(c2, null);
                var name = c1p[i].Name;
                if ((c1pv == null ? "(null)" : c1pv.ToString()) != (c2pv == null ? "(null)" : c2pv.ToString())) {
                    lines.Add(String.Format("{0}.{1} = {2}", indent + "  ", name, FormatCompareDumpValues(c1pv, c2pv, name, indent + "  ")));
                }
            }
            var c1f = c1.GetType().GetFields();
            var c2f = c2.GetType().GetFields();
            for (var i = 0; i < c1f.Count(); i++) {
                var c1fv = c1f[i].GetValue(c1);
                var c2fv = c2f[i].GetValue(c2);
                var name = c1f[i].Name;
                if ((c1fv == null ? "(null)" : c1fv.ToString()) != (c2fv == null ? "(null)" : c2fv.ToString())) {
                    lines.Add(String.Format("{0}.{1} = {2}", indent + "  ", name, FormatCompareDumpValues(c1fv, c2fv, name, indent + "  ")));
                }
            }
            lines.Sort();
            foreach (string line in lines) {
                Console.WriteLine(line);
            }
        }

        public static GameObject FindGameObjectByName(string name)
        {
            foreach (GameObject gameObject in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
            {
                if (gameObject.name == name)
                {
                    return gameObject;
                }
            }
            return null;
        }
    }
}
