﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AppTester.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AppTester.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to fetch the executable&apos;s path!.
        /// </summary>
        internal static string CouldNotGetExecutablePathString {
            get {
                return ResourceManager.GetString("CouldNotGetExecutablePathString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There was an error building the solution or could not get executable path!.
        /// </summary>
        internal static string ErrorBuildingOrFetchingPathsInSolutionString {
            get {
                return ResourceManager.GetString("ErrorBuildingOrFetchingPathsInSolutionString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The input and output&apos;s count does not match!
        ///Remove and rearrange from either list to match the inputs and the expected outputs count..
        /// </summary>
        internal static string InputOutputMismatchString {
            get {
                return ResourceManager.GetString("InputOutputMismatchString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select a file to preview or modify the content..
        /// </summary>
        internal static string NoSelectionPreviewString {
            get {
                return ResourceManager.GetString("NoSelectionPreviewString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Choose an (*.sln) file first!.
        /// </summary>
        internal static string SolutionPathString {
            get {
                return ResourceManager.GetString("SolutionPathString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Welcome to AppTester!
        ///How to use:
        ///- Select a solution using the Add Solution button.
        ///    It will try to build the project and notify whether it succeeded.
        ///- Select your input(s) and output(s) with Add File and Add Folder buttons.
        ///    You can also create manual input(s) using New Input/Output button.
        ///    Then you can edit the input/output in preview textbox.
        ///- Clicking on overwrite will overwrite the selected file with the preview&apos;s content.
        ///- The input(s) and the expected output(s) must be in the sa [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string WelcomeString {
            get {
                return ResourceManager.GetString("WelcomeString", resourceCulture);
            }
        }
    }
}
