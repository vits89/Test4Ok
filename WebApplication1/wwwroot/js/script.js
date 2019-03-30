"use strict";

const apiUrl = "http://localhost:5728/api/newsapi";

$(() => {
    $("form#request-form").on("submit", function(event) {
        event.preventDefault();

        let formData = {
            newsSource: $(this).find("select[name=NewsSource]").val(),
            orderByDate: $(this).find("input[type=radio][name=OrderByDate]:checked").val()
        };

        $.ajax({
            data: formData,
            url: apiUrl
        }).done(showResponse);
    }).submit();
});

function showResponse(data) {
    let container = $("div#news");

    $(container).empty();

    if (data.news.length > 0) {
        $(container).append(createTable(data.news));

        if (data.pagingInfo.totalPages > 0) {
            $(container).append("<br>");
            $(container).append(createPaginator(data.pagingInfo, data.requestFormData));
        }
    } else {
        $(container).append("<p>Нет новостей</p>");
    }
}
function createTable(news) {
    let body = $("<tbody></tbody>"),
        head = $("<thead></thead>"),
        row = $("<tr></tr>");

    [
        "Источник",
        "Название новости",
        "Описание новости",
        "Дата публикации"
    ].forEach(value => {
        let element = $("<th></th>").text(value);

        $(row).append(element);
    });

    $(head).append(row);

    news.forEach(value => {
        const htmlString = "<td></td>";

        let row = $("<tr></tr>"),
            publishDate = new Date(value.publishDate);

        $(row).append([
            $(htmlString).text(value.newsSource.name),
            $(htmlString).text(value.title),
            $(htmlString).html($.parseHTML(value.description)),
            $(htmlString).text(publishDate.toLocaleString())
        ]);

        $(body).append(row);
    });

    return $("<table></table>").append([head, body]);
}
function createPaginator(pagingInfo, requestData) {
    let anchorElement,
        anchorDataValues = { },
        paginator = $("<div></div>").on("click", "a", paginatorClickEventHandler);

    for (let p = 1; p <= pagingInfo.totalPages; p++) {
        if (p === pagingInfo.currentPage) {
            $(paginator).append(p);
        } else {
            anchorDataValues.newsSource = requestData.newsSource;
            anchorDataValues.orderByDate = requestData.orderByDate;

            anchorElement = $("<a></a>", { href: "#" }).data(anchorDataValues).text(p);

            $(paginator).append(anchorElement);
        }
        if (p < pagingInfo.totalPages) {
            $(paginator).append(" ");
        }
    }

    return paginator;
}

function paginatorClickEventHandler(event) {
    event.preventDefault();

    let request = {
        newsSource: $(this).data("news-source"),
        orderByDate: $(this).data("order-by-date"),
        page: $(this).text()
    };

    $.ajax({
        data: request,
        url: apiUrl
    }).done(showResponse);
}
