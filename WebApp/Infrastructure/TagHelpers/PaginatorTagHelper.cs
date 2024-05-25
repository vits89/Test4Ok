using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Test4Ok.WebApp.Models;
using Test4Ok.WebApp.ViewModels;

namespace Test4Ok.WebApp.Infrastructure.TagHelpers;

[HtmlTargetElement("nav", Attributes = "paging-info,request-form-data")]
public class PaginatorTagHelper(IUrlHelperFactory urlHelperFactory) : TagHelper
{
    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; } = null!;

    public PagingInfoViewModel PagingInfo { get; set; } = null!;
    public RequestFormData RequestFormData { get; set; } = null!;

    public string PageAction { get; set; } = "Index";

    public string PagesListClass { get; set; } = "pagination";

    public string PageItemClass { get; set; } = "page-item";
    public string PageItemClassActive { get; set; } = "active";

    public string PageLinkClass { get; set; } = "page-link";

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (PagingInfo.TotalPages <= 1)
        {
            output.SuppressOutput();

            return;
        }

        var urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);

        var tagBuilder = new TagBuilder("ul");

        tagBuilder.AddCssClass(PagesListClass);

        for (var p = 1; p <= PagingInfo.TotalPages; p++)
        {
            tagBuilder.InnerHtml.AppendHtml(GetPageItemContent(p, urlHelper));
        }

        output.Content.AppendHtml(tagBuilder);
    }

    private TagBuilder GetPageItemContent(int page, IUrlHelper urlHelper)
    {
        var tagBuilder = new TagBuilder("li");

        tagBuilder.AddCssClass(PageItemClass);

        if (page == PagingInfo.CurrentPage)
        {
            tagBuilder.AddCssClass(PageItemClassActive);

            tagBuilder.Attributes["aria-current"] = "page";
        }

        tagBuilder.InnerHtml.AppendHtml(GetPageLinkContent(page, urlHelper));

        return tagBuilder;
    }

    private TagBuilder GetPageLinkContent(int page, IUrlHelper urlHelper)
    {
        TagBuilder tagBuilder;

        if (page == PagingInfo.CurrentPage)
        {
            tagBuilder = new TagBuilder("span");
        }
        else
        {
            tagBuilder = new TagBuilder("a");

            tagBuilder.Attributes["href"] = urlHelper.Action(PageAction, GetRouteValues(page));
        }

        tagBuilder.AddCssClass(PageLinkClass);

        tagBuilder.InnerHtml.Append(page.ToString());

        return tagBuilder;
    }

    private object GetRouteValues(int page)
    {
        object routeValues;

        if (RequestFormData.NewsSource > 0)
        {
            if (!RequestFormData.OrderByDate)
            {
                routeValues = new { RequestFormData.NewsSource, RequestFormData.OrderByDate, Page = page };
            }
            else
            {
                routeValues = new { RequestFormData.NewsSource, Page = page };
            }
        }
        else
        {
            if (!RequestFormData.OrderByDate)
            {
                routeValues = new { RequestFormData.OrderByDate, Page = page };
            }
            else
            {
                routeValues = new { Page = page };
            }
        }

        return routeValues;
    }
}
