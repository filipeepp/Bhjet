using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace BHJet_Core.Extension
{
    public static class EnumExtension
    {
        public static string RetornaDescricaoEnum(this System.Enum enumValue, Type enumType)
        {
            string displayName = "";
            MemberInfo info = enumType.GetMember(enumValue.ToString()).First();

            if (info != null && info.CustomAttributes.Any())
            {
                DisplayAttribute nameAttr = info.GetCustomAttribute<DisplayAttribute>();
                displayName = nameAttr != null ? nameAttr.Name : enumValue.ToString();
            }
            else
            {
                
                displayName = enumValue.ToString();
            }
            return displayName;
        }

		public static string RetornaDisplayNameEnum(this System.Enum enumValue)
		{
			return enumValue.GetType()
							.GetMember(enumValue.ToString())
							.First()
							.GetCustomAttribute<DisplayAttribute>()
							.GetName();
		}

	}
}
