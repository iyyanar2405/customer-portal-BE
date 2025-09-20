using CustomerPortal.ActionsService.Entities;
using Microsoft.EntityFrameworkCore;
using ActionEntity = CustomerPortal.ActionsService.Entities.Action;

namespace CustomerPortal.ActionsService.Data
{
    public class ActionsDbContext : DbContext
    {
        public ActionsDbContext(DbContextOptions<ActionsDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<ActionEntity> Actions { get; set; }
        public DbSet<ActionType> ActionTypes { get; set; }
        public DbSet<ActionDependency> ActionDependencies { get; set; }
        public DbSet<ActionAttachment> ActionAttachments { get; set; }
        public DbSet<ActionComment> ActionComments { get; set; }
        public DbSet<ActionHistory> ActionHistory { get; set; }
        public DbSet<ActionTemplate> ActionTemplates { get; set; }
        public DbSet<Workflow> Workflows { get; set; }
        public DbSet<WorkflowStep> WorkflowSteps { get; set; }
        public DbSet<WorkflowInstance> WorkflowInstances { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Action entity
            modelBuilder.Entity<ActionEntity>(entity =>
            {
                entity.ToTable("Actions");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ActionNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(2000);
                entity.Property(e => e.Priority).IsRequired().HasMaxLength(20).HasDefaultValue("MEDIUM");
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20).HasDefaultValue("NOT_STARTED");
                entity.Property(e => e.Progress).HasDefaultValue(0);
                entity.Property(e => e.ActualHours).HasColumnType("decimal(8,2)");
                entity.Property(e => e.Comments).HasMaxLength(1000);

                // Relationships
                entity.HasOne(e => e.ActionType)
                    .WithMany(at => at.Actions)
                    .HasForeignKey(e => e.ActionTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.AssignedTo)
                    .WithMany(u => u.AssignedActions)
                    .HasForeignKey(e => e.AssignedToId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.CreatedBy)
                    .WithMany(u => u.CreatedActions)
                    .HasForeignKey(e => e.CreatedById)
                    .OnDelete(DeleteBehavior.Restrict);

                // Index for action number (should be unique)
                entity.HasIndex(e => e.ActionNumber).IsUnique();
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.Priority);
                entity.HasIndex(e => e.DueDate);
            });

            // Configure ActionType entity
            modelBuilder.Entity<ActionType>(entity =>
            {
                entity.ToTable("ActionTypes");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TypeName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Category).IsRequired().HasMaxLength(50).HasDefaultValue("GENERAL");
                entity.Property(e => e.DefaultPriority).IsRequired().HasMaxLength(20).HasDefaultValue("MEDIUM");
                entity.Property(e => e.RequiresApproval).HasDefaultValue(false);
                entity.Property(e => e.ColorCode).HasMaxLength(20);
                entity.Property(e => e.IconName).HasMaxLength(50);
            });

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.Department).HasMaxLength(100);
                entity.Property(e => e.JobTitle).HasMaxLength(100);
                entity.Property(e => e.Role).HasMaxLength(20).HasDefaultValue("USER");

                // Relationship with Team
                entity.HasOne(e => e.Team)
                    .WithMany(t => t.Members)
                    .HasForeignKey(e => e.TeamId)
                    .OnDelete(DeleteBehavior.SetNull);

                // Index for email (should be unique)
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configure Team entity
            modelBuilder.Entity<Team>(entity =>
            {
                entity.ToTable("Teams");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TeamName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Department).HasMaxLength(100);

                // Self-referencing relationship for manager
                entity.HasOne(e => e.Manager)
                    .WithMany()
                    .HasForeignKey(e => e.ManagerId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure ActionDependency entity
            modelBuilder.Entity<ActionDependency>(entity =>
            {
                entity.ToTable("ActionDependencies");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.DependencyType).IsRequired().HasMaxLength(30).HasDefaultValue("FINISH_TO_START");
                entity.Property(e => e.Notes).HasMaxLength(500);

                // Configure relationships
                entity.HasOne(e => e.DependentAction)
                    .WithMany(a => a.Dependencies)
                    .HasForeignKey(e => e.ActionId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.DependsOn)
                    .WithMany(a => a.DependentActions)
                    .HasForeignKey(e => e.DependsOnActionId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Composite unique constraint
                entity.HasIndex(e => new { e.ActionId, e.DependsOnActionId }).IsUnique();
            });

            // Configure ActionAttachment entity
            modelBuilder.Entity<ActionAttachment>(entity =>
            {
                entity.ToTable("ActionAttachments");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FileName).IsRequired().HasMaxLength(255);
                entity.Property(e => e.FilePath).IsRequired().HasMaxLength(500);
                entity.Property(e => e.FileType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.UploadDate).HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(e => e.Action)
                    .WithMany(a => a.Attachments)
                    .HasForeignKey(e => e.ActionId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.UploadedBy)
                    .WithMany()
                    .HasForeignKey(e => e.UploadedById)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure ActionComment entity
            modelBuilder.Entity<ActionComment>(entity =>
            {
                entity.ToTable("ActionComments");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Comment).IsRequired().HasMaxLength(2000);
                entity.Property(e => e.CommentDate).HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(e => e.Action)
                    .WithMany(a => a.ActionComments)
                    .HasForeignKey(e => e.ActionId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.User)
                    .WithMany(u => u.ActionComments)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ParentComment)
                    .WithMany(c => c.Replies)
                    .HasForeignKey(e => e.ParentCommentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure ActionHistory entity
            modelBuilder.Entity<ActionHistory>(entity =>
            {
                entity.ToTable("ActionHistory");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ChangeType).IsRequired().HasMaxLength(100);
                entity.Property(e => e.FieldName).HasMaxLength(100);
                entity.Property(e => e.OldValue).HasMaxLength(500);
                entity.Property(e => e.NewValue).HasMaxLength(500);
                entity.Property(e => e.ChangeDate).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.Notes).HasMaxLength(1000);

                entity.HasOne(e => e.Action)
                    .WithMany(a => a.History)
                    .HasForeignKey(e => e.ActionId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.ChangedBy)
                    .WithMany()
                    .HasForeignKey(e => e.ChangedById)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure ActionTemplate entity
            modelBuilder.Entity<ActionTemplate>(entity =>
            {
                entity.ToTable("ActionTemplates");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TemplateName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Category).IsRequired().HasMaxLength(50).HasDefaultValue("GENERAL");
                entity.Property(e => e.DefaultTitle).IsRequired().HasMaxLength(200);
                entity.Property(e => e.DefaultDescription).HasMaxLength(2000);
                entity.Property(e => e.DefaultPriority).IsRequired().HasMaxLength(20).HasDefaultValue("MEDIUM");
                entity.Property(e => e.Checklist).HasMaxLength(5000);
                entity.Property(e => e.UsageCount).HasDefaultValue(0);

                entity.HasOne(e => e.ActionType)
                    .WithMany(at => at.ActionTemplates)
                    .HasForeignKey(e => e.ActionTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Workflow entity
            modelBuilder.Entity<Workflow>(entity =>
            {
                entity.ToTable("Workflows");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.WorkflowName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.TriggerType).IsRequired().HasMaxLength(50).HasDefaultValue("MANUAL");
                entity.Property(e => e.TriggerConditions).HasMaxLength(1000);
            });

            // Configure WorkflowStep entity
            modelBuilder.Entity<WorkflowStep>(entity =>
            {
                entity.ToTable("WorkflowSteps");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.StepName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.AssigneeRule).IsRequired().HasMaxLength(50).HasDefaultValue("MANUAL");
                entity.Property(e => e.AssigneeValue).HasMaxLength(100);
                entity.Property(e => e.ApprovalRequired).HasDefaultValue(false);
                entity.Property(e => e.Priority).HasMaxLength(20).HasDefaultValue("MEDIUM");
                entity.Property(e => e.StepConditions).HasMaxLength(1000);

                entity.HasOne(e => e.Workflow)
                    .WithMany(w => w.Steps)
                    .HasForeignKey(e => e.WorkflowId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.ActionType)
                    .WithMany()
                    .HasForeignKey(e => e.ActionTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Composite unique constraint for workflow + step number
                entity.HasIndex(e => new { e.WorkflowId, e.StepNumber }).IsUnique();
            });

            // Configure WorkflowInstance entity
            modelBuilder.Entity<WorkflowInstance>(entity =>
            {
                entity.ToTable("WorkflowInstances");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.InstanceNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20).HasDefaultValue("ACTIVE");
                entity.Property(e => e.StartedDate).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.Context).HasMaxLength(1000);

                entity.HasOne(e => e.Workflow)
                    .WithMany(w => w.Instances)
                    .HasForeignKey(e => e.WorkflowId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.StartedBy)
                    .WithMany()
                    .HasForeignKey(e => e.StartedById)
                    .OnDelete(DeleteBehavior.SetNull);

                // Index for instance number (should be unique)
                entity.HasIndex(e => e.InstanceNumber).IsUnique();
            });
        }

        public async Task SeedDataAsync()
        {
            // Seed Teams
            if (!Teams.Any())
            {
                var teams = new List<Team>
                {
                    new Team { TeamName = "Quality Assurance", Description = "Quality management and compliance team", Department = "Quality" },
                    new Team { TeamName = "Operations", Description = "Day-to-day operations team", Department = "Operations" },
                    new Team { TeamName = "Engineering", Description = "Engineering and technical team", Department = "Engineering" }
                };
                Teams.AddRange(teams);
                await SaveChangesAsync();
            }

            // Seed Users
            if (!Users.Any())
            {
                var users = new List<User>
                {
                    new User { FirstName = "John", LastName = "Manager", Email = "john.manager@acme.com", Role = "MANAGER", Department = "Quality", JobTitle = "Quality Manager", TeamId = 1 },
                    new User { FirstName = "Jane", LastName = "Smith", Email = "jane.smith@acme.com", Role = "USER", Department = "Quality", JobTitle = "Quality Analyst", TeamId = 1 },
                    new User { FirstName = "Bob", LastName = "Wilson", Email = "bob.wilson@acme.com", Role = "USER", Department = "Operations", JobTitle = "Operations Specialist", TeamId = 2 },
                    new User { FirstName = "Alice", LastName = "Brown", Email = "alice.brown@acme.com", Role = "USER", Department = "Engineering", JobTitle = "Senior Engineer", TeamId = 3 }
                };
                Users.AddRange(users);
                await SaveChangesAsync();
            }

            // Seed Action Types
            if (!ActionTypes.Any())
            {
                var actionTypes = new List<ActionType>
                {
                    new ActionType { TypeName = "Document Review", Description = "Review and update documentation", Category = "QUALITY", DefaultPriority = "MEDIUM", DefaultEstimatedHours = 8, RequiresApproval = true, ColorCode = "#4CAF50", IconName = "document-text" },
                    new ActionType { TypeName = "Corrective Action", Description = "Address non-conformities and issues", Category = "CORRECTIVE", DefaultPriority = "HIGH", DefaultEstimatedHours = 16, RequiresApproval = true, ColorCode = "#FF9800", IconName = "exclamation-triangle" },
                    new ActionType { TypeName = "Preventive Action", Description = "Prevent potential issues", Category = "PREVENTIVE", DefaultPriority = "MEDIUM", DefaultEstimatedHours = 12, RequiresApproval = true, ColorCode = "#2196F3", IconName = "shield-check" },
                    new ActionType { TypeName = "Training", Description = "Training and development activities", Category = "TRAINING", DefaultPriority = "LOW", DefaultEstimatedHours = 4, RequiresApproval = false, ColorCode = "#9C27B0", IconName = "academic-cap" },
                    new ActionType { TypeName = "Maintenance", Description = "Equipment and system maintenance", Category = "MAINTENANCE", DefaultPriority = "MEDIUM", DefaultEstimatedHours = 6, RequiresApproval = false, ColorCode = "#607D8B", IconName = "cog" }
                };
                ActionTypes.AddRange(actionTypes);
                await SaveChangesAsync();
            }

            // Seed Action Templates
            if (!ActionTemplates.Any())
            {
                var templates = new List<ActionTemplate>
                {
                    new ActionTemplate { TemplateName = "Quality Manual Review", Description = "Annual review of quality management system manual", Category = "QUALITY", ActionTypeId = 1, DefaultTitle = "Review Quality Manual", DefaultDescription = "Conduct annual review of quality management system manual for compliance and updates", DefaultPriority = "HIGH", DefaultEstimatedHours = 16 },
                    new ActionTemplate { TemplateName = "Corrective Action Plan", Description = "Standard corrective action implementation", Category = "CORRECTIVE", ActionTypeId = 2, DefaultTitle = "Implement Corrective Action", DefaultDescription = "Address identified non-conformity and implement corrective measures", DefaultPriority = "HIGH", DefaultEstimatedHours = 24 }
                };
                ActionTemplates.AddRange(templates);
                await SaveChangesAsync();
            }

            // Seed Actions
            if (!Actions.Any())
            {
                var actions = new List<ActionEntity>
                {
                    new ActionEntity 
                    { 
                        ActionNumber = "ACT-2024-001", 
                        Title = "Review Quality Manual", 
                        Description = "Conduct annual review of quality management system manual",
                        ActionTypeId = 1, 
                        AssignedToId = 2, 
                        CreatedById = 1, 
                        Priority = "HIGH", 
                        Status = "IN_PROGRESS", 
                        StartDate = new DateTime(2024, 10, 1, 9, 0, 0), 
                        DueDate = new DateTime(2024, 10, 15, 17, 0, 0), 
                        EstimatedHours = 16, 
                        ActualHours = 8, 
                        Progress = 50 
                    },
                    new ActionEntity 
                    { 
                        ActionNumber = "ACT-2024-002", 
                        Title = "Complete Procedure Updates", 
                        Description = "Update procedures based on audit findings",
                        ActionTypeId = 1, 
                        AssignedToId = 3, 
                        CreatedById = 1, 
                        Priority = "MEDIUM", 
                        Status = "COMPLETED", 
                        StartDate = new DateTime(2024, 9, 15, 9, 0, 0), 
                        DueDate = new DateTime(2024, 9, 30, 17, 0, 0), 
                        CompletedDate = new DateTime(2024, 9, 28, 16, 30, 0), 
                        EstimatedHours = 12, 
                        ActualHours = 10, 
                        Progress = 100 
                    },
                    new ActionEntity 
                    { 
                        ActionNumber = "ACT-2024-003", 
                        Title = "Implement Equipment Maintenance", 
                        Description = "Implement corrective maintenance on production equipment",
                        ActionTypeId = 5, 
                        AssignedToId = 4, 
                        CreatedById = 1, 
                        Priority = "HIGH", 
                        Status = "NOT_STARTED", 
                        StartDate = new DateTime(2024, 10, 20, 9, 0, 0), 
                        DueDate = new DateTime(2024, 10, 25, 17, 0, 0), 
                        EstimatedHours = 20, 
                        Progress = 0 
                    }
                };
                Actions.AddRange(actions);
                await SaveChangesAsync();
            }
        }
    }
}



