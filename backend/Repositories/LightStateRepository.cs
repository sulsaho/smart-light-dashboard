using System.Collections.Generic;
using System.Threading.Tasks;
using LightWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LightWebAPI.Repositories
{
    public class LightStateRepository : ILightStateRepository
    {
        private readonly LightStateContext _context;

        public LightStateRepository(LightStateContext context)
        {
            this._context = context;
        }
        
        public async Task<IEnumerable<LightState>> Get()
        {
            return await _context.LightStates.ToListAsync();
        }

        public async Task<LightState> Get(int id)
        {
            return await _context.LightStates.FindAsync(id);
        }

        public async Task<LightState> Create(LightState lightState)
        {
            _context.LightStates.Add(lightState);
            await _context.SaveChangesAsync();
            
            return lightState;
        }

        public async Task Update(LightState lightState)
        {
            _context.Entry(lightState).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var lightStateToDelete = await _context.LightStates.FindAsync(id);
            _context.LightStates.Remove(lightStateToDelete);
            await _context.SaveChangesAsync();
        }
    }
    
}