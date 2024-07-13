using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;

namespace ISas.Web.Models
{
    public static class EncDecURL
    {
        public static MvcHtmlString EncodedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            string queryString = string.Empty;
            //string htmlAttributesString = htmlAttributes.ToString();  //string.Empty;
            if (routeValues != null)
            {
                RouteValueDictionary d = new RouteValueDictionary(routeValues);
                for (int i = 0; i < d.Keys.Count; i++)
                {
                    if (i > 0)
                    {
                        queryString += "?";
                    }
                    queryString += d.Keys.ElementAt(i) + "=" + d.Values.ElementAt(i);
                }
            }

            if (htmlAttributes != null)
            {
                //RouteValueDictionary d = new RouteValueDictionary(htmlAttributes);
                //for (int i = 0; i < d.Keys.Count; i++)
                //{
                //    htmlAttributesString += " " + d.Keys.ElementAt(i) + "=" + d.Values.ElementAt(i);
                //}
                //htmlAttributesString = htmlAttributesString.Replace("{", "").Replace("}", "");
            }

            //What is Entity Framework??
            StringBuilder ancor = new StringBuilder();
            ancor.Append("<a ");
            if (htmlAttributes != null)
            {
                RouteValueDictionary d = new RouteValueDictionary(htmlAttributes);
                for (int i = 0; i < d.Keys.Count; i++)
                {
                    ancor.Append(" " + d.Keys.ElementAt(i) + "=\"" + d.Values.ElementAt(i) + "\"");
                }
            }
            ancor.Append(" href='");
            if (controllerName != string.Empty)
            {
                ancor.Append("/" + controllerName);
            }

            if (actionName != "Index")
            {
                ancor.Append("/" + actionName);
            }
            if (queryString != string.Empty)
            {
                //ancor.Append("?q=" + Encrypt(queryString));
                ancor.Append("?q=" + EncryptDecrypt.EncryptString(queryString, true));
            }
            ancor.Append("'");
            ancor.Append(">");
            ancor.Append(linkText);
            ancor.Append("</a>");
            ancor.Append("");
            return new MvcHtmlString(ancor.ToString());
        }


        public static MvcHtmlString EncodedPath(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            string queryString = string.Empty;
            //string htmlAttributesString = htmlAttributes.ToString();  //string.Empty;
            if (routeValues != null)
            {
                RouteValueDictionary d = new RouteValueDictionary(routeValues);
                for (int i = 0; i < d.Keys.Count; i++)
                {
                    if (i > 0)
                    {
                        queryString += "?";
                    }
                    queryString += d.Keys.ElementAt(i) + "=" + d.Values.ElementAt(i);
                }
            }

            if (htmlAttributes != null)
            {
                //RouteValueDictionary d = new RouteValueDictionary(htmlAttributes);
                //for (int i = 0; i < d.Keys.Count; i++)
                //{
                //    htmlAttributesString += " " + d.Keys.ElementAt(i) + "=" + d.Values.ElementAt(i);
                //}
                //htmlAttributesString = htmlAttributesString.Replace("{", "").Replace("}", "");
            }

            //What is Entity Framework??
            StringBuilder ancor = new StringBuilder();
            //ancor.Append("<a ");
            if (htmlAttributes != null)
            {
                RouteValueDictionary d = new RouteValueDictionary(htmlAttributes);
                for (int i = 0; i < d.Keys.Count; i++)
                {
                    ancor.Append(" " + d.Keys.ElementAt(i) + "=\"" + d.Values.ElementAt(i) + "\"");
                }
            }
            // ancor.Append(" href='");
            if (controllerName != string.Empty)
            {
                ancor.Append("/" + controllerName);
            }

            if (actionName != "Index")
            {
                ancor.Append("/" + actionName);
            }
            if (queryString != string.Empty)
            {
                //ancor.Append("?q=" + Encrypt(queryString));
                ancor.Append("?q=" + EncryptDecrypt.EncryptString(queryString, true));
            }
            //ancor.Append("'");
            //   ancor.Append(">");
            //   ancor.Append("</ a >");


            ancor.Append(linkText);
            ancor.Append("");
            return new MvcHtmlString(ancor.ToString());
        }


        public static string EncodedPathString(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            string queryString = string.Empty;
            //string htmlAttributesString = htmlAttributes.ToString();  //string.Empty;
            if (routeValues != null)
            {
                RouteValueDictionary d = new RouteValueDictionary(routeValues);
                for (int i = 0; i < d.Keys.Count; i++)
                {
                    if (i > 0)
                    {
                        queryString += "?";
                    }
                    queryString += d.Keys.ElementAt(i) + "=" + d.Values.ElementAt(i);
                }
            }

            if (htmlAttributes != null)
            {
                //RouteValueDictionary d = new RouteValueDictionary(htmlAttributes);
                //for (int i = 0; i < d.Keys.Count; i++)
                //{
                //    htmlAttributesString += " " + d.Keys.ElementAt(i) + "=" + d.Values.ElementAt(i);
                //}
                //htmlAttributesString = htmlAttributesString.Replace("{", "").Replace("}", "");
            }

            //What is Entity Framework??
            StringBuilder ancor = new StringBuilder();
            //ancor.Append("<a ");
            if (htmlAttributes != null)
            {
                RouteValueDictionary d = new RouteValueDictionary(htmlAttributes);
                for (int i = 0; i < d.Keys.Count; i++)
                {
                    ancor.Append(" " + d.Keys.ElementAt(i) + "=\"" + d.Values.ElementAt(i) + "\"");
                }
            }
            // ancor.Append(" href='");
            if (controllerName != string.Empty)
            {
                ancor.Append("/" + controllerName);
            }

            if (actionName != "Index")
            {
                ancor.Append("/" + actionName);
            }
            if (queryString != string.Empty)
            {
                //ancor.Append("?q=" + Encrypt(queryString));
                ancor.Append("?q=" + EncryptDecrypt.EncryptString(queryString, true));
            }
            //ancor.Append("'");
            //   ancor.Append(">");
            //   ancor.Append("</ a >");


            ancor.Append(linkText);
            ancor.Append("");
            return ancor.ToString();
        }

        public static string EncodedPathString_ForStudentDossier(string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            string queryString = string.Empty;
            //string htmlAttributesString = htmlAttributes.ToString();  //string.Empty;
            if (routeValues != null)
            {
                RouteValueDictionary d = new RouteValueDictionary(routeValues);
                for (int i = 0; i < d.Keys.Count; i++)
                {
                    if (i > 0)
                    {
                        queryString += "?";
                    }
                    queryString += d.Keys.ElementAt(i) + "=" + d.Values.ElementAt(i);
                }
            }

            if (htmlAttributes != null)
            {
                //RouteValueDictionary d = new RouteValueDictionary(htmlAttributes);
                //for (int i = 0; i < d.Keys.Count; i++)
                //{
                //    htmlAttributesString += " " + d.Keys.ElementAt(i) + "=" + d.Values.ElementAt(i);
                //}
                //htmlAttributesString = htmlAttributesString.Replace("{", "").Replace("}", "");
            }

            //What is Entity Framework??
            StringBuilder ancor = new StringBuilder();
            //ancor.Append("<a ");
            if (htmlAttributes != null)
            {
                RouteValueDictionary d = new RouteValueDictionary(htmlAttributes);
                for (int i = 0; i < d.Keys.Count; i++)
                {
                    ancor.Append(" " + d.Keys.ElementAt(i) + "=\"" + d.Values.ElementAt(i) + "\"");
                }
            }
            // ancor.Append(" href='");
            //if (controllerName != string.Empty)
            //{
            //    ancor.Append("/" + controllerName);
            //}

            if (actionName != "Index")
            {
                ancor.Append(actionName);
            }
            if (queryString != string.Empty)
            {
                //ancor.Append("?q=" + Encrypt(queryString));
                ancor.Append("?q=" + EncryptDecrypt.EncryptString(queryString, true));
            }
            //ancor.Append("'");
            //   ancor.Append(">");
            //   ancor.Append("</ a >");


            ancor.Append(linkText);
            ancor.Append("");
            return ancor.ToString();
        }


        public static DataTable ToPivotTable<T, TColumn, TRow, TData>(
    this IEnumerable<T> source,
    Func<T, TColumn> columnSelector,
    Expression<Func<T, TRow>> rowSelector,
    Func<IEnumerable<T>, TData> dataSelector)
        {
            DataTable table = new DataTable();
            var rowName = ((MemberExpression)rowSelector.Body).Member.Name;
            table.Columns.Add(new DataColumn(rowName));
            var columns = source.Select(columnSelector).Distinct();

            foreach (var column in columns)
                table.Columns.Add(new DataColumn(column.ToString()));

            var rows = source.GroupBy(rowSelector.Compile())
                             .Select(rowGroup => new
                             {
                                 Key = rowGroup.Key,
                                 Values = columns.GroupJoin(
                                     rowGroup,
                                     c => c,
                                     r => columnSelector(r),
                                     (c, columnGroup) => dataSelector(columnGroup))
                             });

            foreach (var row in rows)
            {
                var dataRow = table.NewRow();
                var items = row.Values.Cast<object>().ToList();
                items.Insert(0, row.Key);
                dataRow.ItemArray = items.ToArray();
                table.Rows.Add(dataRow);
            }

            return table;
        }
    }


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class EncryptedActionParameterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            Dictionary<string, object> decryptedParameters = new Dictionary<string, object>();
            if (HttpContext.Current.Request.QueryString.Get("q") != null)
            {
                string encryptedQueryString = HttpContext.Current.Request.QueryString.Get("q");
                //string decrptedString = Decrypt(encryptedQueryString.ToString());
                string decrptedString = EncryptDecrypt.DecryptString(encryptedQueryString.ToString(), true);
                string[] paramsArrs = decrptedString.Split('?');

                for (int i = 0; i < paramsArrs.Length; i++)
                {
                    string[] paramArr = paramsArrs[i].Split('=');
                    //decryptedParameters.Add(paramArr[0], Convert.ToInt32(paramArr[1]));
                    decryptedParameters.Add(paramArr[0], paramArr[1]);
                }
            }
            for (int i = 0; i < decryptedParameters.Count; i++)
            {
                filterContext.ActionParameters[decryptedParameters.Keys.ElementAt(i)] = decryptedParameters.Values.ElementAt(i);
            }
            base.OnActionExecuting(filterContext);
        }
    }

    public class EncryptDecrypt
    {
        public static string EncryptString(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            // Get the key from config file
            string key = "KMNOPQASWED";
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

            //set the secret key for the tripleDES algorithm

            tdes.Key = keyArray;

            //mode of operation. there are other 4 modes.

            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;

            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();

            //transform the specified region of bytes array to resultArray

            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0,

                toEncryptArray.Length);

            //Release resources held by TripleDes Encryptor

            tdes.Clear();

            //Return the encrypted data into unreadable string format

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        //As well as when user want to retrive password then required to decrypt that string from database and sent it to UI layer.

        //That decryption is reverse logic of encryption with help of haskey value.
        public static string DecryptString(string cipherString, bool useHashing)
        {
            byte[] keyArray;
            //byte[] toEncryptArray = Convert.FromBase64String(cipherString);
            byte[] toEncryptArray = Convert.FromBase64String(cipherString.Replace(' ', '+'));
            //
            System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            //Get your key from config file to open the lock!
            // string key = (string)settingsReader.GetValue("SecurityKey", typeof(String));
            string key = "KMNOPQASWED";
            if (useHashing)
            {

                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();

                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

                hashmd5.Clear();

            }

            else

                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

            tdes.Key = keyArray;

            tdes.Mode = CipherMode.ECB;

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();

            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            tdes.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }


    public static class DataTableX
    {
        public static IEnumerable<dynamic> AsDynamicEnumerable(this DataTable table)
        {
            // Validate argument here..
            return table.AsEnumerable().Select(row => new DynamicRow(row));
        }



        //public static List<string> ToDynamic(this DataTable dt)
        //{
        //    var dynamicDt = new List<string>();
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        string str = null;
        //        foreach (DataColumn column in dt.Columns)
        //        {
        //            if (string.IsNullOrEmpty(str))
        //                str += "{" + column.ColumnName + "=" + row[column];
        //            else
        //                str += "," + column.ColumnName + "=" + row[column];
        //        }
        //        str += "}";
        //        str = str.Trim('"', ' ');
        //        dynamicDt.Add(str);
        //    }
        //    return dynamicDt;
        //}

        public static List<dynamic> ToDynamic(this DataTable dt)
        {
            var dynamicDt = new List<dynamic>();
            foreach (DataRow row in dt.Rows)
            {
                dynamic dyn = new ExpandoObject();
                dynamicDt.Add(dyn);
                foreach (DataColumn column in dt.Columns)
                {
                    var dic = (IDictionary<string, object>)dyn;
                    dic[column.ColumnName] = row[column];
                }
            }
            return dynamicDt;
        }

        private sealed class DynamicRow : DynamicObject
        {
            private readonly DataRow _row;

            internal DynamicRow(DataRow row) { _row = row; }

            // Interprets a member-access as an indexer-access on the 
            // contained DataRow.
            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                var retVal = _row.Table.Columns.Contains(binder.Name);
                result = retVal ? _row[binder.Name] : null;
                return retVal;
            }
        }
    }


    public class ExpandoJSONConverter : JavaScriptConverter
    {
        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            throw new NotImplementedException();
        }
        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            var result = new Dictionary<string, object>();
            var dictionary = obj as IDictionary<string, object>;
            foreach (var item in dictionary)
                result.Add(item.Key, item.Value);
            return result;
        }
        public override IEnumerable<Type> SupportedTypes
        {
            get
            {
                return new ReadOnlyCollection<Type>(new Type[] { typeof(System.Dynamic.ExpandoObject) });
            }
        }
    }
}