using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Helpers
{
    public interface IPagination
    {
        int Page { get; }

        int ItemsPerPage { get; }

        int ItemsCount { get; }

        int TotalPages { get; }

        bool HasPreviousPage { get; }

        bool HasNextPage { get; }
    }
}
