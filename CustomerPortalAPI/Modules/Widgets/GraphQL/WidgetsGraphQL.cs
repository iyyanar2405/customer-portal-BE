using HotChocolate;
using CustomerPortalAPI.Modules.Shared;

namespace CustomerPortalAPI.Modules.Widgets.GraphQL
{
    public record WidgetConfig(string Id, string Title, string Type, int Position, string Configuration, bool IsVisible);
    
    public class CreateWidgetInput
    {
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Position { get; set; }
        public string Configuration { get; set; } = "{}";
    }
    
    public class UpdateWidgetInput
    {
        public string Id { get; set; } = string.Empty;
        public string? Title { get; set; }
        public int? Position { get; set; }
        public string? Configuration { get; set; }
        public bool? IsVisible { get; set; }
    }
    public record CreateWidgetPayload(WidgetConfig? Widget, string? Error);
    public record UpdateWidgetPayload(WidgetConfig? Widget, string? Error);

    public record ChartData(string Label, double Value, string? Color);
    public record AuditWidget(IEnumerable<ChartData> AuditsByStatus, IEnumerable<ChartData> AuditsByType);
    public record FindingsWidget(IEnumerable<ChartData> FindingsBySeverity, IEnumerable<ChartData> FindingsByCategory);
    public record CertificatesWidget(IEnumerable<ChartData> CertificatesByStatus, IEnumerable<object> ExpiringCertificates);

    public class WidgetsQueries
    {
        public async Task<IEnumerable<WidgetConfig>> GetUserWidgets(string userId)
        {
            // Mock implementation - replace with actual repository calls
            await Task.Delay(1);
            return new List<WidgetConfig>
            {
                new("widget1", "Audit Status", "chart", 1, "{\"chartType\":\"pie\",\"dataSource\":\"audits\"}", true),
                new("widget2", "Recent Findings", "list", 2, "{\"itemCount\":5,\"sortBy\":\"date\"}", true),
                new("widget3", "Certificate Expiry", "alert", 3, "{\"daysAhead\":30,\"alertLevel\":\"warning\"}", true),
                new("widget4", "System Metrics", "gauge", 4, "{\"metric\":\"performance\",\"threshold\":80}", false)
            };
        }

        public async Task<WidgetConfig?> GetWidgetById(string id)
        {
            // Mock implementation - replace with actual repository calls
            await Task.Delay(1);
            return id == "widget1" 
                ? new WidgetConfig("widget1", "Audit Status", "chart", 1, "{\"chartType\":\"pie\",\"dataSource\":\"audits\"}", true)
                : null;
        }

        public async Task<AuditWidget> GetAuditWidgetData()
        {
            // Mock implementation - replace with actual repository calls
            await Task.Delay(1);
            return new AuditWidget(
                new List<ChartData>
                {
                    new("Completed", 75, "#4CAF50"),
                    new("In Progress", 20, "#FF9800"),
                    new("Pending", 5, "#F44336")
                },
                new List<ChartData>
                {
                    new("Internal", 45, "#2196F3"),
                    new("External", 35, "#9C27B0"),
                    new("Compliance", 20, "#607D8B")
                }
            );
        }

        public async Task<FindingsWidget> GetFindingsWidgetData()
        {
            // Mock implementation - replace with actual repository calls
            await Task.Delay(1);
            return new FindingsWidget(
                new List<ChartData>
                {
                    new("Critical", 5, "#F44336"),
                    new("High", 15, "#FF9800"),
                    new("Medium", 25, "#FFEB3B"),
                    new("Low", 55, "#4CAF50")
                },
                new List<ChartData>
                {
                    new("Documentation", 40, "#2196F3"),
                    new("Process", 35, "#9C27B0"),
                    new("Technical", 25, "#FF5722")
                }
            );
        }

        public async Task<CertificatesWidget> GetCertificatesWidgetData()
        {
            // Mock implementation - replace with actual repository calls
            await Task.Delay(1);
            return new CertificatesWidget(
                new List<ChartData>
                {
                    new("Active", 80, "#4CAF50"),
                    new("Expiring Soon", 15, "#FF9800"),
                    new("Expired", 5, "#F44336")
                },
                new List<object>
                {
                    new { CertificateName = "ISO 9001", ExpiryDate = DateTime.UtcNow.AddDays(30), Company = "Company A" },
                    new { CertificateName = "ISO 14001", ExpiryDate = DateTime.UtcNow.AddDays(45), Company = "Company B" }
                }
            );
        }
    }

    public class WidgetsMutations
    {
        public async Task<CreateWidgetPayload> CreateWidget(CreateWidgetInput input, string userId)
        {
            try
            {
                // Mock implementation - replace with actual repository calls
                await Task.Delay(1);
                var widget = new WidgetConfig(
                    Guid.NewGuid().ToString(),
                    input.Title,
                    input.Type,
                    input.Position,
                    input.Configuration,
                    true
                );
                return new CreateWidgetPayload(widget, null);
            }
            catch (Exception ex)
            {
                return new CreateWidgetPayload(null, ex.Message);
            }
        }

        public async Task<UpdateWidgetPayload> UpdateWidget(UpdateWidgetInput input)
        {
            try
            {
                // Mock implementation - replace with actual repository calls
                await Task.Delay(1);
                var widget = new WidgetConfig(
                    input.Id,
                    input.Title ?? "Updated Widget",
                    "chart",
                    input.Position ?? 1,
                    input.Configuration ?? "{}",
                    input.IsVisible ?? true
                );
                return new UpdateWidgetPayload(widget, null);
            }
            catch (Exception ex)
            {
                return new UpdateWidgetPayload(null, ex.Message);
            }
        }

        public async Task<BaseDeletePayload> DeleteWidget(string id)
        {
            try
            {
                // Mock implementation - replace with actual repository calls
                await Task.Delay(1);
                return new BaseDeletePayload(true, null);
            }
            catch (Exception ex)
            {
                return new BaseDeletePayload(false, ex.Message);
            }
        }
    }
}
