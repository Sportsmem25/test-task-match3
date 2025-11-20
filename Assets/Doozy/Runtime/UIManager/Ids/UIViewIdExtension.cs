// Copyright (c) 2015 - 2021 Doozy Entertainment. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

//.........................
//.....Generated Class.....
//.........................
//.......Do not edit.......
//.........................

using System.Collections.Generic;
// ReSharper disable All
namespace Doozy.Runtime.UIManager.Containers
{
    public partial class UIView
    {
        public static IEnumerable<UIView> GetViews(UIViewId.MainMenu id) => GetViews(nameof(UIViewId.MainMenu), id.ToString());
        public static void Show(UIViewId.MainMenu id, bool instant = false) => Show(nameof(UIViewId.MainMenu), id.ToString(), instant);
        public static void Hide(UIViewId.MainMenu id, bool instant = false) => Hide(nameof(UIViewId.MainMenu), id.ToString(), instant);

        public static IEnumerable<UIView> GetViews(UIViewId.MainMenuView id) => GetViews(nameof(UIViewId.MainMenuView), id.ToString());
        public static void Show(UIViewId.MainMenuView id, bool instant = false) => Show(nameof(UIViewId.MainMenuView), id.ToString(), instant);
        public static void Hide(UIViewId.MainMenuView id, bool instant = false) => Hide(nameof(UIViewId.MainMenuView), id.ToString(), instant);
    }
}

namespace Doozy.Runtime.UIManager
{
    public partial class UIViewId
    {
        public enum MainMenu
        {
            MainMenu
        }

        public enum MainMenuView
        {
            MainMenu
        }    
    }
}