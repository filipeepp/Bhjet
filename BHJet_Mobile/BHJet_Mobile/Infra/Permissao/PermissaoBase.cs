using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;

namespace BHJet_Mobile.Infra.Permissao
{
    public static class PermissaoBase
    {
        public static async void VerificaPermissao(Permission permissao, Action permissaoNegadaAction)
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(permissao);
            if (status != PermissionStatus.Granted)
            {
                await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(permissao);

                var results = await CrossPermissions.Current.RequestPermissionsAsync(permissao);
                
                if (results.ContainsKey(permissao))
                    status = results[permissao];
            }

            if (status != PermissionStatus.Granted)
                permissaoNegadaAction.Invoke();
        }
    }
}
