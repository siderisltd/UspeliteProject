namespace Web.Mvc.ViewHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Routing;

    public static class ImageHelper
    {
        public static MvcHtmlString Image<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string alt, string width, string height, object htmlAttributes = null)
        {
            ModelMetadata fieldmetadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            var value = "/" + fieldmetadata.Model;

            TagBuilder imageTag = new TagBuilder("img");
            imageTag.MergeAttribute("src", value);
            imageTag.MergeAttribute("alt", alt);
            imageTag.MergeAttribute("width", width);
            imageTag.MergeAttribute("height", height);
            imageTag.GenerateId(fullName);

            imageTag.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            if (!string.IsNullOrEmpty(fieldName))
            {
                imageTag.MergeAttributes(htmlHelper.GetUnobtrusiveValidationAttributes(fieldName));
            }

            return MvcHtmlString.Create(imageTag.ToString(TagRenderMode.SelfClosing));
        }
    }
}