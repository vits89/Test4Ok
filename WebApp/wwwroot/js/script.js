$(() => {
    const apiUrl = "/api/news";

    const $news = $("#news"),
        $requestForm = $("#request-form");

    const init = () => {
        $requestForm.on("submit", handleRequestFormSubmit).submit();
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
            $news.append(createTable(data.news));

            if (data.pagingInfo.totalPages > 0) {
                $news.append("<br>")
                    .append(createPaginator(data.pagingInfo, data.requestFormData));
            }
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
            const $element = $("<th></th>").text(value);

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

        return $("<table></table>").append([$head, $body]);
    };
    const createPaginator = (pagingInfo, requestData) => {
        const $paginator = $("<div></div>").on("click", "a", handlePaginatorClick);

        for (let p = 1; p <= pagingInfo.totalPages; p++) {
            if (p === pagingInfo.currentPage) {
                $paginator.append(p);
            } else {
                const anchorDataValues = {
                    newsSource: requestData.newsSource,
                    orderByDate: requestData.orderByDate
                };

                const $anchorElement = $("<a></a>", { href: "#" }).data(anchorDataValues).text(p);

                $paginator.append($anchorElement);
            }

            if (p < pagingInfo.totalPages) {
                $paginator.append(" ");
            }
        }

        return $paginator;
    };

    init();
});
