using ItemsAndOrdersManagementSystem.Models;
using ItemsAndOrdersManagementSystem.Resources;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ItemsAndOrdersManagementSystem.Common.Helper
{
    public static class Extention
    {
        public static string[] SeprateMessageKey(this string message)
        {
            return message.Split(SharedConst.errorMessageKeySeprator);
        }

        public static string[] SeprateNewMessage(this string message)
        {
            return message.Split(SharedConst.errorNewMessageSeprator);
        }

        public static string ErrorAppendMessage(this string message,string entityName,string entityProp,string newMessage) 
        {
            if (string.IsNullOrEmpty( message))
                return $"{entityName}.{entityProp}&&{newMessage}";
            else
                return $"{message}$${entityName}.{entityProp}&&{newMessage}";
        }

        public static string ErrorAppendMessage(this string message, string newMessage)
        {
            if (string.IsNullOrEmpty(message))
                return $"&&{newMessage}";
            else
                return $"{message}&&{newMessage}";
        }

        public static ModelStateDictionary AddErrors(this ModelStateDictionary modelStateDictionary,string message)
        {
            foreach (var newOne in message.SeprateNewMessage())
            {
                string[] str = newOne.SeprateMessageKey();
                modelStateDictionary.AddModelError(str[0], str[1]);
            }

            return modelStateDictionary;
        }
    }
}
