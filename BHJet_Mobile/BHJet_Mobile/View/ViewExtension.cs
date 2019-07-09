using System;
using System.Collections.Generic;
using System.Text;

namespace BHJet_Mobile.View
{
    public static class ViewExtension
    {
        public static bool VerificaMainPage(Type PageType)
        {
            var mainType = App.Current.MainPage.GetType().Name;
            if (mainType == PageType.Name)
                return true;
            return false;
        }
    }
}
