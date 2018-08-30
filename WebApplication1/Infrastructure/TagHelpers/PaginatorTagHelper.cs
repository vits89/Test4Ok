using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;

namespace WebApplication1.Infrastructure.TagHelpers
{
    [HtmlTargetElement("div", Attributes = "paging-info,request-form-data")]
    public class PaginatorTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public PagingInfoViewModel PagingInfo { get; set; }
        public RequestFormData RequestFormData { get; set; }
        public string PaginatorAction { get; set; } = "Index";

        public PaginatorTagHelper(IUrlHelperFactory urlHelperFactory) => _urlHelperFactory = urlHelperFactory;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);

            var divTagBuilder = new TagBuilder("div");

            for (var p = 1; p <= PagingInfo.TotalPages; p++)
            {
                if (p == PagingInfo.CurrentPage)
                {
                    divTagBuilder.InnerHtml.Append(p.ToString());
                }
                else
                {
                    var aTagBuilder = new TagBuilder("a");

                    object routeValues;

                    if (RequestFormData.NewsSource > 0)
                    {
                        if (!RequestFormData.OrderByDate)
                        {
                            routeValues = new { RequestFormData.NewsSource, RequestFormData.OrderByDate, Page = p };
                        }
                        else
                        {
                            routeValues = new { RequestFormData.NewsSource, Page = p };
                        }
                    }
                    else
                    {
                        if (!RequestFormData.OrderByDate)
                        {
                            routeValues = new { RequestFormData.OrderByDate, Page = p };
                        }
                        else
                        {
                            routeValues = new { Page = p };
                        }
                    }

                    aTagBuilder.Attributes["href"] = urlHelper.Action(PaginatorAction, routeValues);
                    aTagBuilder.InnerHtml.Append(p.ToString());

                    divTagBuilder.InnerHtml.AppendHtml(aTagBuilder);
                }

                if (p < PagingInfo.TotalPages)
                {
                    divTagBuilder.InnerHtml.Append(" ");
                }
            }

            output.Content.AppendHtml(divTagBuilder.InnerHtml);
        }
    }
}
