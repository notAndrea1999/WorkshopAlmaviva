// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function loadPage() {
    $.ajax({
        url: "/Home/_bookList",
        method: "GET",
        success: function (response) {
            $("#partialPage").html(response)
        }
    })
}

function loadAddPage() {
    $.ajax({
        url: "/Home/_addBook",
        method: "GET",
        success: function (response) {
            $("#partialPageAdd").html(response)
        }
    })
}

function loadUpdatePage(id) {
    $.ajax({
        url: "/Home/_detailsBookUpdate?Id=" + id,
        method: "GET",
        success: function (response) {
            $("#partialPageUpdate").html(response)
        }
    })
}

function loadDetailsPage(id) {
    $.ajax({
        url: "/Home/_bookDetails/" + id,
        method: "GET",
        success: function (response) {
            $("#partialDetailPage").html(response)
        }
    })
}


$('#btn').click(function () {
    $('#modelWindow').modal('show');
});

function apriModale(id) {

    loadUpdatePage(id)

    $("#myModal").modal('show');
}

function apriModaleDetails(id) {

    loadDetailsPage(id)

    $("#myDetailModal").modal('show');
}

function apriModaleDelete(id) {

    let href = $("#myModalDelete a").attr("href");

    let hrefId = href + id;

    $("#myModalDelete a").attr("href", hrefId);

    $("#myModalDelete").modal('show');
}



$(document).ready(function () {
    var currentPage = 1; 
    function searchBooks() {
        var value = $("#value").val().toLowerCase();
        console.log("Current Page:", currentPage);
        $.ajax({
            url: `/Home/_bookList?word=${encodeURIComponent(value)}&pageNumber=${currentPage}`,
            method: "GET",
            success: function (response) {
                console.log(response);
                $("#partialPage").html(response);
                $("#currentPage").text(currentPage);
            },
            error: function (error) {
                console.error("Error during search:", error);
            }
        });
    }
    
    $("#btnSearch").on("click", function () {
        currentPage = 1;
        searchBooks();
    });

    $("#btnFirstPage").on("click", function () {
        currentPage = 1;
        searchBooks();
    });

    $("#btnPreviousPage").on("click", function () {
        if (currentPage > 1) {
            currentPage--;
            searchBooks();
        }
    });


    $("#btnNextPage").on("click", function () {
        var maxPages = 5;
        if (currentPage < maxPages) {
            currentPage++;
            searchBooks();
        }
    });

    $("#btnLastPage").on("click", function () {
        var maxPages = 5;
        currentPage = maxPages;
        searchBooks();
    });
});

