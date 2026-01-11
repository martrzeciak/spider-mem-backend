using System.Text.Json;

namespace SpiderMem.API.Extensions;

public static class HttpExtensions
{
    public static void AddPaginationHeader(this HttpResponse response, int currentPage,
        int itemsPerPage, int totalItems, int totalPages)
    {
        var paginationHeader = new
        {
            currentPage,
            itemsPerPage,
            totalItems,
            totalPages
        };

        string serializedHeader = JsonSerializer.Serialize(paginationHeader);

        response.Headers["Pagination"] = serializedHeader;
        response.Headers.Append("Access-Control-Expose-Headers", "Pagination");
    }
}
