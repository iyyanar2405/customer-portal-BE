using CustomerPortalAPI.Modules.Notifications.Entities;
using CustomerPortalAPI.Modules.Notifications.Repositories;
using HotChocolate;

namespace CustomerPortalAPI.Modules.Notifications.GraphQL
{
    public record NotificationOutput(int Id, string Title, string Message, string Priority, string Status, bool IsActive, DateTime CreatedDate);
    public record CreateNotificationInput(string Title, string Message, int CategoryId, string? Priority);
    public record UpdateNotificationInput(int Id, string? Title, string? Message, string? Priority, string? Status, bool? IsActive);
    public record CreateNotificationPayload(NotificationOutput? Notification, string? Error);
    public record UpdateNotificationPayload(NotificationOutput? Notification, string? Error);
    public record DeletePayload(bool Success, string? Error);

    [ExtendObjectType("Query")]
    public class NotificationsQueries
    {
        public async Task<IEnumerable<NotificationOutput>> GetNotifications([Service] INotificationRepository repository)
        {
            var notifications = await repository.GetAllAsync();
            return notifications.Select(n => new NotificationOutput(n.Id, n.Title, n.Message, n.Priority, n.Status, n.IsActive, n.CreatedDate));
        }

        public async Task<NotificationOutput?> GetNotificationById(int id, [Service] INotificationRepository repository)
        {
            var notification = await repository.GetByIdAsync(id);
            return notification == null ? null : new NotificationOutput(notification.Id, notification.Title, notification.Message, notification.Priority, notification.Status, notification.IsActive, notification.CreatedDate);
        }
    }

    [ExtendObjectType("Mutation")]
    public class NotificationsMutations
    {
        public async Task<CreateNotificationPayload> CreateNotification(CreateNotificationInput input, [Service] INotificationRepository repository)
        {
            try
            {
                var notification = new Notification
                {
                    Title = input.Title,
                    Message = input.Message,
                    CategoryId = input.CategoryId,
                    Priority = input.Priority ?? "Medium",
                    Status = "Active",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };

                var created = await repository.AddAsync(notification);
                return new CreateNotificationPayload(new NotificationOutput(created.Id, created.Title, created.Message, created.Priority, created.Status, created.IsActive, created.CreatedDate), null);
            }
            catch (Exception ex)
            {
                return new CreateNotificationPayload(null, ex.Message);
            }
        }

        public async Task<UpdateNotificationPayload> UpdateNotification(UpdateNotificationInput input, [Service] INotificationRepository repository)
        {
            try
            {
                var notification = await repository.GetByIdAsync(input.Id);
                if (notification == null) return new UpdateNotificationPayload(null, "Notification not found");

                if (input.Title != null) notification.Title = input.Title;
                if (input.Message != null) notification.Message = input.Message;
                if (input.Priority != null) notification.Priority = input.Priority;
                if (input.Status != null) notification.Status = input.Status;
                if (input.IsActive.HasValue) notification.IsActive = input.IsActive.Value;
                notification.ModifiedDate = DateTime.UtcNow;

                await repository.UpdateAsync(notification);
                return new UpdateNotificationPayload(new NotificationOutput(notification.Id, notification.Title, notification.Message, notification.Priority, notification.Status, notification.IsActive, notification.CreatedDate), null);
            }
            catch (Exception ex)
            {
                return new UpdateNotificationPayload(null, ex.Message);
            }
        }

        public async Task<DeletePayload> DeleteNotification(int id, [Service] INotificationRepository repository)
        {
            try
            {
                await repository.DeleteAsync(id);
                return new DeletePayload(true, null);
            }
            catch (Exception ex)
            {
                return new DeletePayload(false, ex.Message);
            }
        }
    }
}