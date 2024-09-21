using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public enum Status
    {
        Pending,
        Answered,
        Approved,
        Rejected,
        InProgress,
        AdminReview,
        Published,
        Adminsuspended,
        Managersuspended
    }
}
