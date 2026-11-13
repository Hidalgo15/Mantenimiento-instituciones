using Mantenimiento.Core.Domain.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace MantenimientoPresentation.Utilities
{
    internal static class GridPaginationViewData
    {
        public static void Set<T>(Controller controller, PagedResult<T> result)
        {
            controller.ViewData["GridPaginationEnabled"] = true;
            controller.ViewData["GridPaginationUrl"] = $"{controller.Request.Path}{controller.Request.QueryString}";
            controller.ViewData["GridPageNumber"] = result.PageNumber;
            controller.ViewData["GridTotalPages"] = Math.Max(1, result.TotalPages);
            controller.ViewData["GridPageSize"] = result.PageSize;
            controller.ViewData["GridTotalCount"] = result.TotalCount;
        }
    }
}
