//using Microsoft.EntityFrameworkCore;

//namespace WorkerDemo.Generic.WorkflowEF;

//public partial class WorkflowContext : DbContext
//{
//    public WorkflowContext(DbContextOptions<WorkflowContext> options)
//        : base(options)
//    {
//    }

//    public virtual DbSet<Event> Events { get; set; }

//    public virtual DbSet<ExecutionError> ExecutionErrors { get; set; }

//    public virtual DbSet<ExecutionPointer> ExecutionPointers { get; set; }

//    public virtual DbSet<ExtensionAttribute> ExtensionAttributes { get; set; }

//    public virtual DbSet<ScheduledCommand> ScheduledCommands { get; set; }

//    public virtual DbSet<Subscription> Subscriptions { get; set; }

//    public virtual DbSet<Workflow> Workflows { get; set; }

//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        modelBuilder.Entity<Event>(entity =>
//        {
//            entity.HasKey(e => e.PersistenceId);

//            entity.ToTable("Event");

//            entity.HasIndex(e => e.EventId, "IX_Event_EventId").IsUnique();

//            entity.HasIndex(e => new { e.EventName, e.EventKey }, "IX_Event_EventName_EventKey");

//            entity.HasIndex(e => e.EventTime, "IX_Event_EventTime");

//            entity.HasIndex(e => e.IsProcessed, "IX_Event_IsProcessed");
//        });

//        modelBuilder.Entity<ExecutionError>(entity =>
//        {
//            entity.HasKey(e => e.PersistenceId);

//            entity.ToTable("ExecutionError");
//        });

//        modelBuilder.Entity<ExecutionPointer>(entity =>
//        {
//            entity.HasKey(e => e.PersistenceId);

//            entity.ToTable("ExecutionPointer");

//            entity.HasIndex(e => e.WorkflowId, "IX_ExecutionPointer_WorkflowId");

//            entity.HasOne(d => d.Workflow).WithMany(p => p.ExecutionPointers).HasForeignKey(d => d.WorkflowId);
//        });

//        modelBuilder.Entity<ExtensionAttribute>(entity =>
//        {
//            entity.HasKey(e => e.PersistenceId);

//            entity.ToTable("ExtensionAttribute");

//            entity.HasIndex(e => e.ExecutionPointerId, "IX_ExtensionAttribute_ExecutionPointerId");

//            entity.HasOne(d => d.ExecutionPointer).WithMany(p => p.ExtensionAttributes).HasForeignKey(d => d.ExecutionPointerId);
//        });

//        modelBuilder.Entity<ScheduledCommand>(entity =>
//        {
//            entity.HasKey(e => e.PersistenceId);

//            entity.ToTable("ScheduledCommand");

//            entity.HasIndex(e => new { e.CommandName, e.Data }, "IX_ScheduledCommand_CommandName_Data").IsUnique();

//            entity.HasIndex(e => e.ExecuteTime, "IX_ScheduledCommand_ExecuteTime");
//        });

//        modelBuilder.Entity<Subscription>(entity =>
//        {
//            entity.HasKey(e => e.PersistenceId);

//            entity.ToTable("Subscription");

//            entity.HasIndex(e => e.EventKey, "IX_Subscription_EventKey");

//            entity.HasIndex(e => e.EventName, "IX_Subscription_EventName");

//            entity.HasIndex(e => e.SubscriptionId, "IX_Subscription_SubscriptionId").IsUnique();
//        });

//        modelBuilder.Entity<Workflow>(entity =>
//        {
//            entity.HasKey(e => e.PersistenceId);

//            entity.ToTable("Workflow");

//            entity.HasIndex(e => e.InstanceId, "IX_Workflow_InstanceId").IsUnique();

//            entity.HasIndex(e => e.NextExecution, "IX_Workflow_NextExecution");
//        });

//        OnModelCreatingPartial(modelBuilder);
//    }

//    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
//}
