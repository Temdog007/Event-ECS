﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Event_ECS_WPF.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.7.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("500")]
        public uint MaxLogLength {
            get {
                return ((uint)(this["MaxLogLength"]));
            }
            set {
                this["MaxLogLength"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool MultilineLog {
            get {
                return ((bool)(this["MultilineLog"]));
            }
            set {
                this["MultilineLog"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("F5")]
        public global::System.Windows.Forms.Keys ToggleStart {
            get {
                return ((global::System.Windows.Forms.Keys)(this["ToggleStart"]));
            }
            set {
                this["ToggleStart"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Windows\\System32\\notepad.exe")]
        public string ComponentEditor {
            get {
                return ((string)(this["ComponentEditor"]));
            }
            set {
                this["ComponentEditor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool LogEvents {
            get {
                return ((bool)(this["LogEvents"]));
            }
            set {
                this["LogEvents"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool LoadComponentEditor {
            get {
                return ((bool)(this["LoadComponentEditor"]));
            }
            set {
                this["LoadComponentEditor"] = value;
            }
        }

        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Collections.ObjectModel.ObservableCollection<string> RecentProjects
        {
            get
            {
                return ((global::System.Collections.ObjectModel.ObservableCollection<string>)(this["RecentProjects"]));
            }
            set
            {
                this["RecentProjects"] = value;
            }
        }
    }
}
