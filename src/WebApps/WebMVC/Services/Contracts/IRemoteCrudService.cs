using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnteyaSidOnContainers.WebApps.WebMVC.Services.Contracts
{
    public interface IRemoteCrudService
    {
        Task<string> GetData(string queryParams);

        Task<string> Create(object jsonObject);

        Task<string> Update(object id, string jsonObject);

        Task<string> Delete(object id);
    }
}
