using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Website_InAnQuangCaoHTD.Data;

public class FlashSaleUpdateService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<FlashSaleUpdateService> _logger;

    public FlashSaleUpdateService(IServiceScopeFactory serviceScopeFactory, ILogger<FlashSaleUpdateService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await UpdateExpiredFlashSales(_context);
            }

            // Chờ một khoảng thời gian trước khi thực hiện kiểm tra lại
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken); // kiểm tra mỗi giờ
        }
    }

    private async Task UpdateExpiredFlashSales(ApplicationDbContext context)
    {
        var now = DateTime.Now;
        var expiredFlashSales = await context.FlashSales
            .Where(fs => fs.IsActive && fs.FlashSaleEndTime <= now)
            .ToListAsync();

        foreach (var flashSale in expiredFlashSales)
        {
            flashSale.IsActive = false;
        }

        await context.SaveChangesAsync();
    }
}
