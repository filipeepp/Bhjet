using System.Collections.Generic;
using Xamarin.Forms;

namespace BHJet_Mobile.Infra
{
    public class GlobalVariablesManager
    {
        public static object GetApplicationCurrentProperty(VariaveisGlobais chave)
        {
            object retValue = null;
            IDictionary<string, object> properties = Application.Current.Properties;
            if (properties.ContainsKey(chave.ToString()))
                retValue = properties[chave.ToString()];

            return retValue;
        }

        public static void SetApplicationCurrentProperty(VariaveisGlobais chave, object obj)
        {
            IDictionary<string, object> properties = Application.Current.Properties;
            if (properties.ContainsKey(chave.ToString()))
                properties[chave.ToString()] = obj;
            else
                properties.Add(chave.ToString(), obj);
        }

        public enum VariaveisGlobais
        {
            Token,
            OcorrenciaID
        }
    }
}
