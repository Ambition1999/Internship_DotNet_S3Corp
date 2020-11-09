using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace APILayer.Models
{
    public class CsvIntModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var key = bindingContext.ModelName;
            var valueProviderResult = bindingContext.ValueProvider.GetValue(key);
            if (valueProviderResult == null)
            {
                return false;
            }

            var attemptedValue = valueProviderResult.AttemptedValue;
            if (attemptedValue != null)
            {
                var list = attemptedValue.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).
                           Select(v => int.Parse(v.Trim())).ToList();

                bindingContext.Model = list;
            }
            else
            {
                bindingContext.Model = new List<int>();
            }
            return true;
        }
    }
}