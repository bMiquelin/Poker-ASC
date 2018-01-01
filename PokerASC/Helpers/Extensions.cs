using PokerASC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace PokerASC.Helpers
{
    public static class Extensions
    {
        //public static string Encrypt(this string orig_data)
        //{
        //    var dataBytes = Encoding.UTF8.GetBytes(orig_data);
        //    var salt = Encoding.UTF8.GetBytes("PokerASCSalt");
        //    byte[] saltedValue = dataBytes.Concat(salt).ToArray();
        //    var hashed = new SHA256Managed().ComputeHash(saltedValue);
        //    return Encoding.UTF8.GetString(hashed);
        //}

        //public static IEnumerable<T> Pop<T>(this List<T> list, int qtdPop)
        //{
        //    if (list == null) throw new ArgumentNullException();
        //    if (qtdPop > list.Count()) throw new IndexOutOfRangeException();
        //    if (qtdPop == 0) throw new ArgumentOutOfRangeException();
        //
        //    foreach (T elemento in list)
        //    {
        //        yield return elemento;
        //        list.Remove(elemento);
        //    }
        //
        //}
        public static IEnumerable<T> Embaralhar<T>(this IEnumerable<T> collection)
        {
            return collection.OrderBy(v => Guid.NewGuid());
        }

        public static string Encrypt(this string data)
        {
            data = "PokerASCSalt" + data;
            SHA256Managed crypt = new SHA256Managed();
            StringBuilder hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(data), 0, Encoding.UTF8.GetByteCount(data));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

        public static string GetDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }
}