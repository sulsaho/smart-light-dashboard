using System.Collections.Generic;
using System.Threading.Tasks;
using LightWebAPI.Models;

namespace LightWebAPI.Repositories
{
    public interface ILightStateRepository
    {
        Task<IEnumerable<LightState>> Get();

        Task<LightState> Get(int id);

        Task<LightState> Create(LightState lightState);

        Task Update(LightState lightState);

        Task Delete(int id);
    }
}