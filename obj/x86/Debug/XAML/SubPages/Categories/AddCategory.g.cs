﻿#pragma checksum "C:\Users\redfo\source\repos\Salary Control\XAML\SubPages\Categories\AddCategory.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "492D78C427C1550CB11A0332423CDE3F38D6A1BEE5C92BF1955596D7571A63D1"
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
    partial class AddCategory : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.19041.1")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // XAML\SubPages\Categories\AddCategory.xaml line 12
                {
                    global::Windows.UI.Xaml.Controls.Button element2 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)element2).Click += this.RedirectBack;
                }
                break;
            case 3: // XAML\SubPages\Categories\AddCategory.xaml line 25
                {
                    this.categoryName = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 4: // XAML\SubPages\Categories\AddCategory.xaml line 30
                {
                    this.categoryColor = (global::Windows.UI.Xaml.Controls.ColorPicker)(target);
                }
                break;
            case 5: // XAML\SubPages\Categories\AddCategory.xaml line 36
                {
                    global::Windows.UI.Xaml.Controls.Button element5 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)element5).Click += this.SaveCategory;
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.19041.1")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}
