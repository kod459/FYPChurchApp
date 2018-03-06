using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing; 
namespace PIMS.Controllers
{
    public static class HtmlExtensions
    {
        /*public static MvcHtmlString CheckBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return CheckBoxFor(html, expression, new RouteDirection());
        }

        public static MvcHtmlString CheckBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel>> expression, object htmlAttributes)
        {
            return CheckBoxFor(html, expression, htmlAttributes, "");
        }*/

        public static MvcHtmlString CheckboxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes, string checkedValue)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);

            TagBuilder tag = new TagBuilder("input");
            tag.Attributes.Add("type", "checkbox");
            tag.Attributes.Add("name", metadata.PropertyName);
            if (!string.IsNullOrEmpty(checkedValue))
            {
                tag.Attributes.Add("value", checkedValue);
            }
            else
            {
                tag.Attributes.Add("value", metadata.Model.ToString());
            }

            if (htmlAttributes != null)
            {
                //tag.MergeAttribute(new RouteValueDictionary(htmlAttributes));
            }

            if (metadata.Model.ToString() == checkedValue)
            {
                tag.Attributes.Add("checked", "checked");
            }
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.SelfClosing));
        }

        public static List<KeyValuePair<string, string>> UnitedStatesDictionary()
        {
            var arrList = new List<KeyValuePair<string, string>>();
            arrList.Add(new KeyValuePair<string, string>("AL", "Alabama"));
            arrList.Add(new KeyValuePair<string, string>("AK", "Alaska"));
            arrList.Add(new KeyValuePair<string, string>("AZ", "Arizona"));
            arrList.Add(new KeyValuePair<string, string>("AR", "Arkansas"));
            arrList.Add(new KeyValuePair<string, string>("CA", "California"));
            arrList.Add(new KeyValuePair<string, string>("CO", "Colorado"));
            arrList.Add(new KeyValuePair<string, string>("CT", "Connecticut"));
            arrList.Add(new KeyValuePair<string, string>("DE", "Delaware"));
            arrList.Add(new KeyValuePair<string, string>("DC", "District Of Columbia"));
            arrList.Add(new KeyValuePair<string, string>("FL", "Florida"));
            arrList.Add(new KeyValuePair<string, string>("GA", "Georgia"));
            arrList.Add(new KeyValuePair<string, string>("HI", "Hawaii"));
            arrList.Add(new KeyValuePair<string, string>("ID", "Idaho"));
            arrList.Add(new KeyValuePair<string, string>("IL", "Illinois"));
            arrList.Add(new KeyValuePair<string, string>("IN", "Indiana"));
            arrList.Add(new KeyValuePair<string, string>("IA", "Iowa"));
            arrList.Add(new KeyValuePair<string, string>("KS", "Kansas"));
            arrList.Add(new KeyValuePair<string, string>("KY", "Kentucky"));
            arrList.Add(new KeyValuePair<string, string>("LA", "Louisiana"));
            arrList.Add(new KeyValuePair<string, string>("ME", "Maine"));
            arrList.Add(new KeyValuePair<string, string>("MD", "Maryland"));
            arrList.Add(new KeyValuePair<string, string>("MA", "Massachusetts"));
            arrList.Add(new KeyValuePair<string, string>("MI", "Michigan"));
            arrList.Add(new KeyValuePair<string, string>("MN", "Minnesota"));
            arrList.Add(new KeyValuePair<string, string>("MS", "Mississippi"));
            arrList.Add(new KeyValuePair<string, string>("MO", "Missouri"));
            arrList.Add(new KeyValuePair<string, string>("MT", "Montana"));
            arrList.Add(new KeyValuePair<string, string>("NE", "Nebraska"));
            arrList.Add(new KeyValuePair<string, string>("NV", "Nevada"));
            arrList.Add(new KeyValuePair<string, string>("NH", "New Hampshire"));
            arrList.Add(new KeyValuePair<string, string>("NJ", "New Jersey"));
            arrList.Add(new KeyValuePair<string, string>("NM", "New Mexico"));
            arrList.Add(new KeyValuePair<string, string>("NY", "New York"));
            arrList.Add(new KeyValuePair<string, string>("NC", "North Carolina"));
            arrList.Add(new KeyValuePair<string, string>("ND", "North Dakota"));
            arrList.Add(new KeyValuePair<string, string>("OH", "Ohio"));
            arrList.Add(new KeyValuePair<string, string>("OK", "Oklahoma"));
            arrList.Add(new KeyValuePair<string, string>("OR", "Oregon"));
            arrList.Add(new KeyValuePair<string, string>("PA", "Pennsylvania"));
            arrList.Add(new KeyValuePair<string, string>("RI", "Rhode Island"));
            arrList.Add(new KeyValuePair<string, string>("SC", "South Carolina"));
            arrList.Add(new KeyValuePair<string, string>("SD", "South Dakota"));
            arrList.Add(new KeyValuePair<string, string>("TN", "Tennessee"));
            arrList.Add(new KeyValuePair<string, string>("TX", "Texas"));
            arrList.Add(new KeyValuePair<string, string>("UT", "Utah"));
            arrList.Add(new KeyValuePair<string, string>("VT", "Vermont"));
            arrList.Add(new KeyValuePair<string, string>("VA", "Virginia"));
            arrList.Add(new KeyValuePair<string, string>("WA", "Washington"));
            arrList.Add(new KeyValuePair<string, string>("WV", "West Virginia"));
            arrList.Add(new KeyValuePair<string, string>("WI", "Wisconsin"));
            arrList.Add(new KeyValuePair<string, string>("WY", "Wyoming"));
            return arrList;
        }
    }
}