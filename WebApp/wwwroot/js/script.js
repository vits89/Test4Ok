$(() => {
    const apiUrl = "/api/news";

    const $news = $("#News"),
        $requestForm = $("#RequestForm");

    const init = () => {
        $requestForm.on("submit", handleRequestFormSubmit).trigger("submit");
    };

    const handleRequestFormSubmit = function (event) {
        event.preventDefault();

        const formData = {
            newsSource: $(this).find("select[name=NewsSource]").val(),
            orderByDate: $(this).find("input[name=OrderByDate]:checked").val()
        };

        $.ajax({
            data: formData,
            url: apiUrl
        }).done(showNews);
    };
    const handlePaginatorClick = function (event) {
        event.preventDefault();

        const requestData = {
            newsSource: $(this).data("news-source"),
            orderByDate: $(this).data("order-by-date"),
            page: $(this).text()
        };

        $.ajax({
            data: requestData,
            url: apiUrl
        }).done(showNews);
    };

    const showNews = data => {
        $news.empty();

        if (data.news.length > 0) {
            const $main = $("<main></main>");

            $main.append(createTable(data.news));
            $main.append(createPaginator(data.pagingInfo, data.requestFormData));

            $news.append($main);
        } else {
            $news.append("<p>Нет новостей</p>");
        }
    };
    const createTable = news => {
        const $body = $("<tbody></tbody>"),
            $head = $("<thead></thead>"),
            $row = $("<tr></tr>");

        [
            "Источник",
            "Название новости",
            "Описание новости",
            "Дата публикации"
        ].forEach(value => {
            const $element = $("<th></th>").attr({ scope: "col" }).text(value);

            $row.append($element);
        });

        $head.append($row);

        news.forEach(value => {
            const htmlString = "<td></td>",
                publishDate = new Date(value.publishDate);

            const $row = $("<tr></tr>");

            $row.append([
                $(htmlString).text(value.newsSource),
                $(htmlString).text(value.title),
                $(htmlString).html($.parseHTML(value.description)),
                $(htmlString).text(publishDate.toLocaleString())
            ]);

            $body.append($row);
        });

        return $("<table></table>").addClass("table table-bordered").append([$head, $body]);
    };
    const createPaginator = (pagingInfo, requestData) => {
        const $navigation = $("<nav></nav>", { "aria-label": "News pages" }),
            $paginator = $("<ul></ul>")
                .addClass("pagination m-0")
                .on("click", "a", handlePaginatorClick);

        for (let p = 1; p <= pagingInfo.totalPages; p++) {
            const $pageItem = $("<li></li>").addClass("page-item");

            let $pageLink;

            if (p === pagingInfo.currentPage) {
                $pageItem.addClass("active").attr({ "aria-current": "page" });

                $pageLink = $("<span></span>");
            } else {
                const linkDataValues = {
                    newsSource: requestData.newsSource,
                    orderByDate: requestData.orderByDate
                };

                $pageLink = $("<a></a>", { href: "#" }).data(linkDataValues);
            }

            $pageLink.addClass("page-link").text(p);

            $pageItem.append($pageLink);
            $paginator.append($pageItem);
        }

        return $navigation.append($paginator);
    };

    init();
});
