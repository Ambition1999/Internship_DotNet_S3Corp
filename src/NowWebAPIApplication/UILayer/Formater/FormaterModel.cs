using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UILayer.Formater
{
    public class FormaterModel
    {
        public FormaterModel() { }

        public static string convertListIdToString(List<int> listId)
        {
            string strId = "";
            foreach (var item in listId)
            {
                strId += item + ",";
            }
            if (strId != "" && strId.Contains(','))
                strId = strId.Substring(0, strId.Length - 1);
            return strId;
        }
    }
}