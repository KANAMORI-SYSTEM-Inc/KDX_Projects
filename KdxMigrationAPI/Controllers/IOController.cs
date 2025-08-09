using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KdxMigrationAPI.Data;
using Kdx.Contracts.DTOs;

namespace KdxMigrationAPI.Controllers
{
    /// <summary>
    /// IO CRUD API
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class IOController : ControllerBase
    {
        private readonly MigrationDbContext _context;
        private readonly ILogger<IOController> _logger;

        public IOController(MigrationDbContext context, ILogger<IOController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// すべてのIOデータを取得
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IO>>> GetIOs()
        {
            return await _context.IOs.ToListAsync();
        }

        /// <summary>
        /// 特定のIOデータを取得
        /// </summary>
        [HttpGet("{address}/{plcId}")]
        public async Task<ActionResult<IO>> GetIO(string address, int plcId)
        {
            var io = await _context.IOs.FirstOrDefaultAsync(x => x.Address == address && x.PlcId == plcId);

            if (io == null)
            {
                return NotFound();
            }

            return io;
        }

        /// <summary>
        /// IOデータを作成
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<IO>> CreateIO(IO io)
        {
            try
            {
                io.CreatedAt = DateTime.UtcNow;
                io.UpdatedAt = DateTime.UtcNow;
                
                _context.IOs.Add(io);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetIO), new { address = io.Address, plcId = io.PlcId }, io);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error creating IO");
                return Conflict("IO with the same Address and PlcId already exists");
            }
        }

        /// <summary>
        /// IOデータを更新
        /// </summary>
        [HttpPut("{address}/{plcId}")]
        public async Task<IActionResult> UpdateIO(string address, int plcId, IO io)
        {
            if (address != io.Address || plcId != io.PlcId)
            {
                return BadRequest("Address and PlcId cannot be changed");
            }

            var existing = await _context.IOs.FirstOrDefaultAsync(x => x.Address == address && x.PlcId == plcId);
            if (existing == null)
            {
                return NotFound();
            }

            existing.Name = io.Name;
            existing.IOType = io.IOType;
            existing.IsInverted = io.IsInverted;
            existing.IsEnabled = io.IsEnabled;
            existing.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        /// <summary>
        /// IOデータを削除
        /// </summary>
        [HttpDelete("{address}/{plcId}")]
        public async Task<IActionResult> DeleteIO(string address, int plcId)
        {
            var io = await _context.IOs.FirstOrDefaultAsync(x => x.Address == address && x.PlcId == plcId);
            if (io == null)
            {
                return NotFound();
            }

            _context.IOs.Remove(io);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// IOタイプでフィルタリング
        /// </summary>
        [HttpGet("by-type/{ioType}")]
        public async Task<ActionResult<IEnumerable<IO>>> GetIOsByType(int ioType)
        {
            return await _context.IOs.Where(x => x.IOType == ioType).ToListAsync();
        }

        /// <summary>
        /// 有効なIOのみ取得
        /// </summary>
        [HttpGet("enabled")]
        public async Task<ActionResult<IEnumerable<IO>>> GetEnabledIOs()
        {
            return await _context.IOs.Where(x => x.IsEnabled).ToListAsync();
        }
    }
}