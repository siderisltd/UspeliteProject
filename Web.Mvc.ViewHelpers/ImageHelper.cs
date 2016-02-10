namespace Web.Mvc.ViewHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Web.Mvc;

    public static class ImageHelper
    {
        public static MvcHtmlString Image<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, string alt, string width, string height)
        {
            return htmlHelper.Image(expression, alt, width, height, null);
        }

        public static MvcHtmlString Image<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string alt, string width, string height, IDictionary<string, object> htmlAttributes)
        {
            ModelMetadata fieldmetadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            var value = fieldmetadata.Model.ToString();

            TagBuilder imageTag = new TagBuilder("img");
            imageTag.MergeAttribute("src", value);
            imageTag.MergeAttribute("alt", alt);
            imageTag.MergeAttribute("width", width);
            imageTag.MergeAttribute("height", height);
            imageTag.GenerateId(fullName);

            imageTag.MergeAttributes(htmlAttributes);

            if (!string.IsNullOrEmpty(fieldName))
            {
                imageTag.MergeAttributes(htmlHelper.GetUnobtrusiveValidationAttributes(fieldName));
            }

            return MvcHtmlString.Create(imageTag.ToString(TagRenderMode.SelfClosing));
        }
    }
}