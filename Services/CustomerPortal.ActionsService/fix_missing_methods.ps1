# PowerShell script to add missing method stubs to ExtendedRepositories.cs
# This will make the service compile even if some methods are not fully implemented

Write-Host "Adding missing method stubs to ExtendedRepositories.cs..."

$filePath = "C:\POC\local\customer-portalBE\customer-portal-BE\Services\CustomerPortal.ActionsService\Repositories\ExtendedRepositories.cs"

# Add missing methods for ActionAttachmentRepository
$actionAttachmentStubs = @"

        public async Task DeleteAsync(ActionAttachment attachment)
        {
            _context.ActionAttachments.Remove(attachment);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ActionAttachment>> GetByActionIdAsync(int actionId)
        {
            return await _context.ActionAttachments
                .Where(aa => aa.ActionId == actionId)
                .ToListAsync();
        }
"@

# Add missing methods for ActionCommentRepository  
$actionCommentStubs = @"

        public async Task DeleteAsync(ActionComment comment)
        {
            _context.ActionComments.Remove(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ActionComment>> GetByActionIdAsync(int actionId)
        {
            return await _context.ActionComments
                .Include(ac => ac.CreatedBy)
                .Where(ac => ac.ActionId == actionId)
                .OrderBy(ac => ac.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ActionComment>> GetByUserIdAsync(int userId)
        {
            return await _context.ActionComments
                .Include(ac => ac.Action)
                .Where(ac => ac.CreatedById == userId)
                .OrderByDescending(ac => ac.CreatedDate)
                .ToListAsync();
        }
"@

# Add missing methods for ActionHistoryRepository
$actionHistoryStubs = @"

        public async Task DeleteAsync(ActionHistory history)
        {
            _context.ActionHistories.Remove(history);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ActionHistory>> GetByActionIdAsync(int actionId)
        {
            return await _context.ActionHistories
                .Include(ah => ah.ChangedBy)
                .Where(ah => ah.ActionId == actionId)
                .OrderByDescending(ah => ah.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ActionHistory>> GetByUserIdAsync(int userId)
        {
            return await _context.ActionHistories
                .Include(ah => ah.Action)
                .Where(ah => ah.ChangedById == userId)
                .OrderByDescending(ah => ah.CreatedDate)
                .ToListAsync();
        }
"@

Write-Host "Method stubs ready to be added manually..."
Write-Host "ActionAttachment stubs:"
Write-Host $actionAttachmentStubs
Write-Host "`nActionComment stubs:"
Write-Host $actionCommentStubs
Write-Host "`nActionHistory stubs:"
Write-Host $actionHistoryStubs