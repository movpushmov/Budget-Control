// Updated by XamlIntelliSenseFileGenerator 06.09.2020 1:35:34
#pragma checksum "C:\Users\redfo\source\repos\Salary Control\XAML\SubPages\Events\EventGroup.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "7F5DB4A18F5D1EF0227FC2102DAD0BB3FF9FBDB25E20D6908A4C3E5AE39C6950"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Salary_Control.XAML.SubPages
{
    partial class EventGroup : global::Windows.UI.Xaml.Controls.Page
    {


        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks", " 10.0.19041.1")]
        private global::Windows.UI.Xaml.Controls.TextBox newEventName;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks", " 10.0.19041.1")]
        private global::Windows.UI.Xaml.Controls.TextBox newEventCost;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks", " 10.0.19041.1")]
        private global::Windows.UI.Xaml.Controls.AutoSuggestBox newEventCategory;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks", " 10.0.19041.1")]
        private global::Windows.UI.Xaml.Controls.ListView list;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks", " 10.0.19041.1")]
        private bool _contentLoaded;

        /// <summary>
        /// InitializeComponent()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks", " 10.0.19041.1")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;

            global::System.Uri resourceLocator = new global::System.Uri("ms-appx:///XAML/SubPages/Events/EventGroup.xaml");
            global::Windows.UI.Xaml.Application.LoadComponent(this, resourceLocator, global::Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);
        }

        partial void UnloadObject(global::Windows.UI.Xaml.DependencyObject unloadableObject);

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks", " 10.0.19041.1")]
        private interface IEventGroup_Bindings
        {
            void Initialize();
            void Update();
            void StopTracking();
            void DisconnectUnloadedObject(int connectionId);
        }

        private interface IEventGroup_BindingsScopeConnector
        {
            global::System.WeakReference Parent { get; set; }
            bool ContainsElement(int connectionId);
            void RegisterForElementConnection(int connectionId, global::Windows.UI.Xaml.Markup.IComponentConnector connector);
        }
#pragma warning restore 0649
#pragma warning restore 0169
    }
}

