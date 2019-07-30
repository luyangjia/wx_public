namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data.SqlClient;
    using System.Linq.Expressions;
    using System.Collections.ObjectModel;
    using System.Collections.Generic;
    using System.Collections;
    using System.ComponentModel;


    public partial class EmpContext : DbContext
    {
        private string _userName;

        public EmpContext()
            : base("name=EmpContext")
        {
        }

        public EmpContext(string userName)
            : this()
        {
            this._userName = userName;
        }

        public virtual DbSet<Feedback> Feedback { get; set; }
        public virtual DbSet<Building> BuildingsDbSet { get; set; }

        public virtual IQueryable<Building> Buildings
        {
            get
            {
                if (string.IsNullOrEmpty(_userName)) return this.BuildingsDbSet.Where(x => true);
                return this.BuildingsDbSet.Where(b => b.Organization.Users.Any(u => u.UserName == this._userName));
            }
        }

        public virtual DbSet<Coordinate> Coordinates { get; set; }
        public virtual DbSet<Dictionary> Dictionaries { get; set; }
        public virtual DbSet<RatedParameter> RatedParameters { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Meter> MetersDbSet { get; set; }
        public virtual DbSet<CommandQueue> CommandQueues { get; set; }
        public virtual IQueryable<Meter> Meters
        {
            get
            {
                if (string.IsNullOrEmpty(_userName)) return this.MetersDbSet.Where(x => true);
                return this.MetersDbSet.Where(m => m.Building.Organization.Users.Any(u => u.UserName == this._userName));
            }
        }
        public virtual DbSet<MetersAction> MetersActions { get; set; }
        public virtual DbSet<MeterDailyResult> MeterDailyResults { get; set; }
        public virtual DbSet<MeterHourlyResult> MeterHourlyResults { get; set; }
        public virtual DbSet<MeterMonthlyResult> MeterMonthlyResults { get; set; }
        public virtual DbSet<MomentaryValue> MomentaryValues { get; set; }
        public virtual DbSet<OriginalData> OriginalDatas { get; set; }
        public virtual DbSet<Parameter> Parameters { get; set; }
        public virtual DbSet<Organization> OrganizationsDbSet { get; set; }
        public virtual IQueryable<Organization> Organizations
        {
            get
            {
                if (string.IsNullOrEmpty(_userName)) return this.OrganizationsDbSet.Where(x => true);
                return this.OrganizationsDbSet.Where(o => o.Users.Any(u => u.UserName == this._userName));
            }
        }

        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserClaim> UserClaims { get; set; }
        public virtual DbSet<UserLogin> UserLogins { get; set; }
        public virtual DbSet<ExtensionField> ExtensionFields { get; set; }
        public virtual DbSet<MeterFullInfo> MeterFullInfo { get; set; }
        public virtual DbSet<BuildingFullInfo> BuildingFullInfo { get; set; }
        public virtual DbSet<StatisticalRelation> StatisticalRelation { get; set; }

        public virtual DbSet<Attachment> Attachment { get; set; }
        public virtual DbSet<MonitoringConfig> MonitoringConfig { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<MessageRecord> MessageRecord { get; set; }
        public virtual DbSet<Subscribe> Subscribe { get; set; }
        public virtual DbSet<MeterStatus> MeterStatus { get; set; }
        public virtual DbSet<HistoryBill> HistoryBill { get; set; }
        public virtual DbSet<UserAccount> UserAccount { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Province> Provinces { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<Balance> Balances { get; set; }
        public virtual DbSet<BalanceDetail> BalanceDetials { get; set; }

        // Message.Templete
        //public virtual DbSet<Template> Templates { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<ActivityRecord> ActivityRecords { get; set; }
        public virtual DbSet<Maintenance> Maintenances { get; set; }
        public virtual DbSet<Purchase> Purchases { get; set; }
        public virtual DbSet<ConfigCycleSetting> ConfigCycleSetting { get; set; }
        public virtual DbSet<ConfigDetail> ConfigDetail { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dictionary>()
                .Property(e => e.EquValue)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.FeedbackType)
                .WithRequired(e => e.Type)
                .HasForeignKey(e => e.StateId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.FeedbackState)
                .WithRequired(e => e.State)
                .HasForeignKey(e => e.TypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.FeedbackUser)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.HandleUserId);

            modelBuilder.Entity<User>()
                .HasMany(e => e.FeedbackHandleUser)
                .WithRequired(e => e.HandleUser)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
              .HasMany(e => e.Payer)
              .WithRequired(e => e.Payer)
              .HasForeignKey(e => e.PayerId)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Receiver)
                .WithRequired(e => e.Receiver)
                .HasForeignKey(e => e.ReceiverId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
               .HasMany(e => e.BillOperators)
               .WithRequired(e => e.Operator)
               .HasForeignKey(e => e.OperatorId)
               .WillCascadeOnDelete(false);


            modelBuilder.Entity<User>()
                .HasMany(e => e.UserAccount)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dictionary>()
               .HasMany(e => e.PayMethod)
               .WithOptional(e => e.PayMethod)
               .HasForeignKey(e => e.PayMethodId);

            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.BillType)
                .WithRequired(e => e.BillType)
                .HasForeignKey(e => e.BillTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.PayType)
                .WithOptional(e => e.PayType)
                .HasForeignKey(e => e.PayTypeId);

            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.BalanceType)
                .WithRequired(e => e.BalanceType)
                .HasForeignKey(e => e.BalanceTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.BalanceDetailsByCategory)
                .WithRequired(e => e.EnergyCategoryDict)
                .HasForeignKey(e => e.EnergyCategory)
                .WillCascadeOnDelete(false);

            //Message.Templete 外键引用Dictionary
            //modelBuilder.Entity<Dictionary>()
            //    .HasMany(e => e.TemplatesByCategory)
            //    .WithRequired(e => e.CategoryDict)
            //    .HasForeignKey(e => e.Category)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Dictionary>()
            //    .HasMany(e => e.BalanceByCategory)
            //    .WithRequired(e => e.BalanceCategory)
            //    .HasForeignKey(e => e.TargetCategory)
            //    .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserAccount>()
             .Property(e => e.Balance)
             .IsUnicode(false);

            modelBuilder.Entity<HistoryBill>()
               .Property(e => e.subject)
               .IsUnicode(false);

            modelBuilder.Entity<HistoryBill>()
                .Property(e => e.Body)
                .IsUnicode(false);

            modelBuilder.Entity<HistoryBill>()
                .Property(e => e.TransNumber)
                .IsUnicode(false);

            modelBuilder.Entity<MeterFullInfo>()
              .Property(e => e.BranchName)
              //.IsFixedLength()
              ;

            modelBuilder.Entity<MeterFullInfo>()
                .Property(e => e.Rate)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Building>()
                .HasMany(e => e.Children)
                .WithOptional(e => e.Parent)
                .HasForeignKey(e => e.ParentId);

            modelBuilder.Entity<Building>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Buildings)
                .Map(m => m.ToTable("UserBuildings", "User").MapLeftKey("BuildingId").MapRightKey("UserId"));

            modelBuilder.Entity<Coordinate>()
                .Property(e => e.X)
                .HasPrecision(28, 14);

            modelBuilder.Entity<Coordinate>()
                .Property(e => e.Y)
                .HasPrecision(28, 14);

            modelBuilder.Entity<Coordinate>()
                .HasMany(e => e.BuildingsBy2d)
                .WithOptional(e => e.Coordinate2d)
                .HasForeignKey(e => e.Coordinate2dId);

            modelBuilder.Entity<Coordinate>()
                .HasMany(e => e.BuildingsBy3d)
                .WithOptional(e => e.Coordinate3d)
                .HasForeignKey(e => e.Coordinate3dId);

            modelBuilder.Entity<Coordinate>()
                .HasMany(e => e.BuildingsByMap)
                .WithOptional(e => e.CoordinateMap)
                .HasForeignKey(e => e.CoordinateMapId);

            modelBuilder.Entity<Coordinate>()
                .HasMany(e => e.MetersBy2d)
                .WithOptional(e => e.Coordinate2d)
                .HasForeignKey(e => e.Coordinate2dId);

            modelBuilder.Entity<Coordinate>()
                .HasMany(e => e.MetersBy3d)
                .WithOptional(e => e.Coordinate3d)
                .HasForeignKey(e => e.Coordinate3dId);

            modelBuilder.Entity<Coordinate>()
                .HasMany(e => e.MetersByMap)
                .WithOptional(e => e.CoordinateMap)
                .HasForeignKey(e => e.CoordinateMapId);

            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.OrganizationByType)
                .WithRequired(e => e.TypeDict)
                .HasForeignKey(e => e.Type)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.BuildingsByCategory)
                .WithRequired(e => e.BuildingCategoryDict)
                .HasForeignKey(e => e.BuildingCategoryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.BuildingsByType)
                .WithRequired(e => e.TypeDict)
                .HasForeignKey(e => e.Type)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.Coordinates)
                .WithRequired(e => e.TypeDict)
                .HasForeignKey(e => e.Type)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.Brands)
                .WithRequired(e => e.TypeDict)
                .HasForeignKey(e => e.MeterType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.MeterDailyResults)
                .WithRequired(e => e.StatusDict)
                .HasForeignKey(e => e.Status)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.MetersByEnergyCategory)
                .WithRequired(e => e.EnergyCategoryDict)
                .HasForeignKey(e => e.EnergyCategoryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.MeterHourlyResults)
                .WithRequired(e => e.StatusDict)
                .HasForeignKey(e => e.Status)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.MeterMonthlyResults)
                .WithRequired(e => e.StatusDict)
                .HasForeignKey(e => e.Status)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.Parameters)
                .WithRequired(e => e.Type)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Brand>()
                .HasMany(e => e.Meters)
                .WithRequired(e => e.Brand)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Brand>()
                .HasMany(e => e.Parameters)
                .WithRequired(e => e.Brand)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Meter>()
                .Property(e => e.Access)
                //.IsFixedLength() //长度不足，用空格补充
                ;

            modelBuilder.Entity<Meter>()
                .Property(e => e.BranchName)
                //.IsFixedLength()
                ;

            modelBuilder.Entity<Meter>()
                .Property(e => e.Rate)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Meter>()
                .HasMany(e => e.Children)
                .WithOptional(e => e.Parent)
                .HasForeignKey(e => e.ParentId);

            //modelBuilder.Entity<Meter>()
            //    .HasOptional(e => e.Building)
            //    .WithMany(e => e.Meters)
            //    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Meter>()
                .HasMany(e => e.MeterDailyResults)
                .WithRequired(e => e.Meter)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Meter>()
                .HasMany(e => e.MeterHourlyResults)
                .WithRequired(e => e.Meter)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Meter>()
                .HasMany(e => e.MeterMonthlyResults)
                .WithRequired(e => e.Meter)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Meter>()
                .HasMany(e => e.MomentaryValues)
                .WithRequired(e => e.Meter)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Meter>()
                .HasMany(e => e.OriginalDatas)
                .WithRequired(e => e.Meter)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MeterDailyResult>()
                .Property(e => e.Total)
                .HasPrecision(18, 4);

            modelBuilder.Entity<MeterHourlyResult>()
                .Property(e => e.Total)
                .HasPrecision(18, 4);

            modelBuilder.Entity<MeterMonthlyResult>()
                .Property(e => e.Total)
                .HasPrecision(18, 4);

            modelBuilder.Entity<MomentaryValue>()
                .Property(e => e.Value)
                .HasPrecision(18, 4);

            modelBuilder.Entity<OriginalData>()
                .Property(e => e.Parameter01)
                .HasPrecision(18, 4);

            modelBuilder.Entity<OriginalData>()
                .Property(e => e.Parameter02)
                .HasPrecision(18, 4);

            modelBuilder.Entity<OriginalData>()
                .Property(e => e.Parameter03)
                .HasPrecision(18, 4);

            modelBuilder.Entity<OriginalData>()
                .Property(e => e.Parameter04)
                .HasPrecision(18, 4);

            modelBuilder.Entity<OriginalData>()
                .Property(e => e.Parameter05)
                .HasPrecision(18, 4);

            modelBuilder.Entity<OriginalData>()
                .Property(e => e.Parameter06)
                .HasPrecision(18, 4);

            modelBuilder.Entity<OriginalData>()
                .Property(e => e.Parameter07)
                .HasPrecision(18, 4);

            modelBuilder.Entity<OriginalData>()
                .Property(e => e.Parameter08)
                .HasPrecision(18, 4);

            modelBuilder.Entity<OriginalData>()
                .Property(e => e.Parameter09)
                .HasPrecision(18, 4);

            modelBuilder.Entity<OriginalData>()
                .Property(e => e.Parameter10)
                .HasPrecision(18, 4);

            modelBuilder.Entity<OriginalData>()
                .Property(e => e.Parameter11)
                .HasPrecision(18, 4);

            modelBuilder.Entity<OriginalData>()
                .Property(e => e.Parameter12)
                .HasPrecision(18, 4);

            modelBuilder.Entity<OriginalData>()
                .Property(e => e.Parameter13)
                .HasPrecision(18, 4);

            modelBuilder.Entity<OriginalData>()
                .Property(e => e.Parameter14)
                .HasPrecision(18, 4);

            modelBuilder.Entity<OriginalData>()
                .Property(e => e.Parameter15)
                .HasPrecision(18, 4);

            modelBuilder.Entity<OriginalData>()
                .Property(e => e.Parameter16)
                .HasPrecision(18, 4);

            modelBuilder.Entity<OriginalData>()
                .Property(e => e.Parameter17)
                .HasPrecision(18, 4);

            modelBuilder.Entity<OriginalData>()
                .Property(e => e.Parameter18)
                .HasPrecision(18, 4);

            modelBuilder.Entity<OriginalData>()
                .Property(e => e.Parameter19)
                .HasPrecision(18, 4);

            modelBuilder.Entity<OriginalData>()
                .Property(e => e.Parameter20)
                .HasPrecision(18, 4);

            modelBuilder.Entity<OriginalData>()
                .Property(e => e.Parameter21)
                .HasPrecision(18, 4);

            modelBuilder.Entity<OriginalData>()
                .Property(e => e.Parameter22)
                .HasPrecision(18, 4);

            modelBuilder.Entity<OriginalData>()
                .Property(e => e.Parameter23)
                .HasPrecision(18, 4);

            modelBuilder.Entity<OriginalData>()
                .Property(e => e.Parameter24)
                .HasPrecision(18, 4);

            modelBuilder.Entity<OriginalData>()
                .Property(e => e.Parameter25)
                .HasPrecision(18, 4);

            modelBuilder.Entity<OriginalData>()
                .Property(e => e.Parameter26)
                .HasPrecision(18, 4);

            modelBuilder.Entity<OriginalData>()
                .Property(e => e.Parameter27)
                .HasPrecision(18, 4);

            modelBuilder.Entity<OriginalData>()
                .Property(e => e.Parameter28)
                .HasPrecision(18, 4);

            modelBuilder.Entity<OriginalData>()
                .Property(e => e.Parameter29)
                .HasPrecision(18, 4);

            modelBuilder.Entity<OriginalData>()
                .Property(e => e.Parameter30)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Parameter>()
                .HasMany(e => e.MomentaryValues)
                .WithRequired(e => e.Parameter)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Organization>()
                .HasMany(e => e.Children)
                .WithOptional(e => e.Parent)
                .HasForeignKey(e => e.ParentId);

            modelBuilder.Entity<Organization>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Organizations)
                .Map(m => m.ToTable("UserOrganizations", "User").MapLeftKey("OrganizationId").MapRightKey("ManagerId"));

            modelBuilder.Entity<Permission>()
                .HasMany(e => e.Children)
                .WithOptional(e => e.Parent)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Permission>()
                .HasMany(e => e.Roles)
                .WithMany(e => e.Permissions)
                .Map(m => m.ToTable("RolePermissions", "User").MapLeftKey("Permission").MapRightKey("RoleId"));

            modelBuilder.Entity<Role>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Roles)
                .Map(m => m.ToTable("UserRoles", "User").MapLeftKey("RoleId").MapRightKey("UserId"));


            modelBuilder.Entity<Balance>()
                .HasMany(e => e.BalanceDetails)
                .WithRequired(e => e.Balance)
                .WillCascadeOnDelete(false);

            #region 0421 by chenwei for messageManager
            modelBuilder.Entity<Attachment>()
                 .Property(e => e.Description)
                 .IsUnicode(false);

            modelBuilder.Entity<Attachment>()
                .Property(e => e.Path)
                .IsUnicode(false);

            modelBuilder.Entity<Attachment>()
                .Property(e => e.OriginalName)
                .IsUnicode(false);

            modelBuilder.Entity<Attachment>()
                .Property(e => e.LogicalName)
                .IsUnicode(false);



            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.AttachmentsByFormat)
                .WithRequired(e => e.AttachmentFormat)
                .HasForeignKey(e => e.AttachmentFormatId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.AttachmentsByType)
                .WithRequired(e => e.AttachmentType)
                .HasForeignKey(e => e.AttachmentTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.MessagesByType)
                .WithRequired(e => e.MessageType)
                .HasForeignKey(e => e.MessageTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.MessageBySourceType)
                .WithRequired(e => e.MessageSourceType)
                .HasForeignKey(e => e.MessageSourceTypeId)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.MeterStatusByMessageType)
                .WithRequired(e => e.MeterMessageType)
                .HasForeignKey(e => e.MeterMessageTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dictionary>()
              .HasMany(e => e.MonitoringConfigsByTargetType)
              .WithRequired(e => e.TargetType)
              .HasForeignKey(e => e.TargetTypeId)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.MonitoringConfigsByConfigType)
                .WithRequired(e => e.ConfigType)
                .HasForeignKey(e => e.ConfigTypeId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Dictionary>()
              .HasMany(e => e.MonitoringConfigsByValidType)
              .WithRequired(e => e.ValidType)
              .HasForeignKey(e => e.ValidTypeId)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.MonitoringConfigsByWay)
                .WithOptional(e => e.Way)
                .HasForeignKey(e => e.WayId);

            //modelBuilder.Entity<MonitoringConfig>()
            //   .HasMany(e => e.MeterStatuses)
            //   .WithOptional(e => e.MonitoringConfig)
            //   .HasForeignKey(e => e.MonitoringConfigId);


            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.MonitoringConfigsByCycleType)
                .WithOptional(e => e.CycleType)
                .HasForeignKey(e => e.CycleTypeId);

            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.MonitoringConfigsByAlarmLevel)
                .WithOptional(e => e.AlarmLevel)
                .HasForeignKey(e => e.AlarmLevelId);

            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.MonitoringConfigsByEnergyCategory)
                .WithOptional(e => e.EnergyCategory)
                .HasForeignKey(e => e.EnergyCategoryId);

            modelBuilder.Entity<Parameter>()
             .HasMany(e => e.MonitoringConfigs)
             .WithOptional(e => e.Parameter)
             .HasForeignKey(e => e.ParameterId);

            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.SubscribesByTargetType)
                .WithRequired(e => e.TargetType)
                .HasForeignKey(e => e.TargetTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.SubscribesByType)
                .WithRequired(e => e.Type)
                .HasForeignKey(e => e.TypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.MetersByType)
                .WithRequired(e => e.TypeDict)
                .HasForeignKey(e => e.Type)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dictionary>()
               .HasMany(e => e.SubscribesByReceivingMode)
               .WithRequired(e => e.ReceivingModel)
               .HasForeignKey(e => e.ReceivingModelId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.SubscribesByMessageType)
                .WithRequired(e => e.MessageType)
                .HasForeignKey(e => e.MessageTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MonitoringConfig>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<MonitoringConfig>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<MonitoringConfig>()
                .Property(e => e.UnitValue)
                .HasPrecision(18, 4);

            modelBuilder.Entity<MonitoringConfig>()
                .Property(e => e.LowerLimit)
                .HasPrecision(18, 4);

            modelBuilder.Entity<MonitoringConfig>()
                .Property(e => e.UpperLimit)
                .HasPrecision(18, 4);

            modelBuilder.Entity<MonitoringConfig>()
                .Property(e => e.Value)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Message>()
                .HasMany(e => e.MessageRecords)
                .WithRequired(e => e.Message)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Subscribe>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Subscribe>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Subscribe>()
                .Property(e => e.TargetId)
                .IsUnicode(false);

            modelBuilder.Entity<Meter>()
                .HasMany(e => e.MeterStatus)
                .WithRequired(e => e.Meter)
                .WillCascadeOnDelete(false);


            //modelBuilder.Entity<Building>()
            //    .HasMany(e => e.Meters)
            //    .WithRequired(e => e.Building)
            //    .WillCascadeOnDelete(false);

            modelBuilder.Entity<MeterStatus>()
                .Property(e => e.Value)
                .HasPrecision(18, 4);


            modelBuilder.Entity<User>()
                .HasMany(e => e.MessageRecords)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);
            #endregion
            #region 0912 by iefhong for Mainteance
            //modelBuilder.Entity<Dictionary>()
            //    .HasMany(e => e.ActivityRecordType)
            //    .WithRequired(e => e.TargetType)
            //    .HasForeignKey(e => e.StateId)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Dictionary>()
            //    .HasMany(e => e.ActivityRecordState)
            //    .WithRequired(e => e.ActivityState)
            //    .HasForeignKey(e => e.TargetTypeId)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Dictionary>()
            //    .HasMany(e => e.MaintenanceCategory)
            //    .WithRequired(e => e.MaintenanceCategory)
            //    .HasForeignKey(e => e.MaintenanceCategoryId)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Dictionary>()
            //    .HasMany(e => e.MaintenanceState)
            //    .WithRequired(e => e.MaintenanceState)
            //    .HasForeignKey(e => e.ObjectCategoryId)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Dictionary>()
            //    .HasMany(e => e.MaintenanceObjectCategory)
            //    .WithRequired(e => e.MaintenanceObjectCategory)
            //    .HasForeignKey(e => e.StateId)
            //    .WillCascadeOnDelete(false);
            //modelBuilder.Entity<Maintenance>()
            //    .HasMany(e => e.Purchase)
            //    .WithRequired(e => e.Maintenance)
            //    .WillCascadeOnDelete(false);
            //modelBuilder.Entity<User>()
            //    .HasMany(e => e.ActivityRecordCurrentOperator)
            //    .WithRequired(e => e.CurrentOperator)
            //    .HasForeignKey(e => e.CurrentOperatorId)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<User>()
            //    .HasMany(e => e.MaintenanceUser)
            //    .WithRequired(e => e.User)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<User>()
            //    .HasMany(e => e.PurchaseCurrentOperator)
            //    .WithOptional(e => e.PurchaseCurrentOperator)
            //    .HasForeignKey(e => e.ApproverId);

            //modelBuilder.Entity<User>()
            //    .HasMany(e => e.PurchaseApprover)
            //    .WithRequired(e => e.PurchaseApprover)
            //    .HasForeignKey(e => e.CurrentOperatorId)
            //    .WillCascadeOnDelete(false);            


            modelBuilder.Entity<Attachment>()
                .HasMany(e => e.Maintenances)
                .WithOptional(e => e.Picture)
                .HasForeignKey(e => e.PictureId);



            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.ActivityRecords)
                .WithRequired(e => e.State)
                .HasForeignKey(e => e.StateId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.MaintenanceMaintenanceCategories)
                .WithRequired(e => e.MaintenanceCategory)
                .HasForeignKey(e => e.MaintenanceCategoryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.MaintenanceStates)
                .WithRequired(e => e.State)
                .HasForeignKey(e => e.StateId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Maintenance>()
                .HasMany(e => e.ActivityRecords)
                .WithRequired(e => e.MaintenanceTarget)
                .HasForeignKey(e => e.TargetId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Maintenance>()
                .HasMany(e => e.Purchases)
                .WithRequired(e => e.Maintenance)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ActivityRecords)
                .WithRequired(e => e.Publisher)
                .HasForeignKey(e => e.PublisherId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.MaintenanceUsers)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.ApproverId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.MaintenanceApprovers)
                .WithRequired(e => e.Approver)
                .HasForeignKey(e => e.OperatorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.MaintenanceOperators)
                .WithRequired(e => e.Operator)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.PurchaseCurrentOperators)
                .WithOptional(e => e.CurrentOperator)
                .HasForeignKey(e => e.ApproverId);

            modelBuilder.Entity<User>()
                .HasMany(e => e.PurchaseApprovers)
                .WithRequired(e => e.Approver)
                .HasForeignKey(e => e.CurrentOperatorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Building>()
                .HasMany(e => e.Maintenances)
                .WithRequired(e => e.Building)
                .WillCascadeOnDelete(false);

            #endregion
            #region 0924 by iefhong for AirConditions

            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.Groups)
                .WithRequired(e => e.GroupType)
                .HasForeignKey(e => e.GroupTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.LogInfos)
                .WithOptional(e => e.Dictionary)
                .HasForeignKey(e => e.ActionId);

            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.SceneModes)
                .WithRequired(e => e.Dictionary)
                .HasForeignKey(e => e.ConfigTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.RatedParameters)
                .WithOptional(e => e.Dictionary)
                .HasForeignKey(e => e.RatedParameterTypeId);

            modelBuilder.Entity<MonitoringConfig>()
                .HasMany(e => e.ConfigCycleSettings)
                .WithRequired(e => e.MonitoringConfig)
                .HasForeignKey(e => e.ConfigId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MonitoringConfig>()
                .HasMany(e => e.SceneModeConfigs)
                .WithRequired(e => e.MonitoringConfig)
                .HasForeignKey(e => e.ConfigId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SceneMode>()
                .HasMany(e => e.SceneModeConfig)
                .WithRequired(e => e.SceneMode)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SceneMode>()
                .HasMany(e => e.SceneModeMeter)
                .WithRequired(e => e.SceneMode)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Group>()
                .HasMany(e => e.MeterGroups)
                .WithRequired(e => e.Group)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RatedParameter>()
                .Property(e => e.MinValue)
                .HasPrecision(12, 4);
            modelBuilder.Entity<RatedParameter>()
                .Property(e => e.MinValue)
                .HasPrecision(12, 4);
            modelBuilder.Entity<RatedParameter>()
                            .Property(e => e.PPFMax)
                            .HasPrecision(12, 4);
            modelBuilder.Entity<RatedParameter>()
                            .Property(e => e.PPFMin)
                            .HasPrecision(12, 4);
            modelBuilder.Entity<RatedParameter>()
                            .Property(e => e.RPFMax)
                            .HasPrecision(12, 4);
            modelBuilder.Entity<RatedParameter>()
                           .Property(e => e.RPFMin)
                           .HasPrecision(12, 4);
            modelBuilder.Entity<RatedParameter>()
                           .Property(e => e.RPF)
                           .HasPrecision(12, 4);
            modelBuilder.Entity<RatedParameter>()
                           .Property(e => e.PPF)
                           .HasPrecision(12, 4);
            modelBuilder.Entity<RatedParameter>()
                .Property(e => e.MaxValue)
                .HasPrecision(12, 4);

            modelBuilder.Entity<User>()
                .HasMany(e => e.LogInfos)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.OperatorId);

            modelBuilder.Entity<User>()
                .HasMany(e => e.MonitoringConfigs)
                .WithOptional(e => e.Regenerator)
                .HasForeignKey(e => e.RegeneratorId);

            modelBuilder.Entity<User>()
                .HasMany(e => e.SceneModeMeters)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.OperatorId);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Groups)
                .WithOptional(e => e.Regenerator)
                .HasForeignKey(e => e.RegeneratorId);

            modelBuilder.Entity<User>()
                .HasMany(e => e.RatedParameterDetails)
                .WithOptional(e => e.Operator)
                .HasForeignKey(e => e.OperatorId);

            modelBuilder.Entity<Building>()
                .HasMany(e => e.BuildingMeterTypeUsers)
                .WithRequired(e => e.Building)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Building>()
                .HasMany(e => e.RequestForOvertimes)
                .WithRequired(e => e.Building)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.BuildingMeterTypeUsers)
                .WithRequired(e => e.MeterType)
                .HasForeignKey(e => e.MeterTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.RequestForOvertimeApplicantors)
                .WithRequired(e => e.Applicantor)
                .HasForeignKey(e => e.ApplicantId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.RequestForOvertimeApprovers)
                .WithOptional(e => e.Approver)
                .HasForeignKey(e => e.ApproverId);

            #endregion

            #region 0930 by iefhong for UserExtension、ConfigDetail

            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.UserExtensions)
                .WithRequired(e => e.ColumnType)
                .HasForeignKey(e => e.ColumnTypeId)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<MonitoringConfig>()
            //    .HasMany(e => e.ConfigDetails)
            //    .WithRequired(e => e.Template)
            //    .HasForeignKey(e => e.TemplateId)
            //    .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ConfigDetails)
                .WithOptional(e => e.OperatorUser)
                .HasForeignKey(e => e.OperatorId);

            modelBuilder.Entity<Dictionary>()
               .HasMany(e => e.ConfigDetailEngeryCategories)
               .WithOptional(e => e.EnergyCategory)
               .HasForeignKey(e => e.EnergyCategoryId);

            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.ConfigDetailBuildingCategories)
                .WithOptional(e => e.BuildingCategory)
                .HasForeignKey(e => e.BuildingCategoryId);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserExtensions)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserExtension>()
                .Property(e => e.Value)
                .HasPrecision(9, 2);

            modelBuilder.Entity<MonitoringConfig>()
             .HasMany(e => e.ConfigCycleSettings)
             .WithRequired(e => e.MonitoringConfig)
             .HasForeignKey(e => e.ConfigId)
             .WillCascadeOnDelete(false);

            //modelBuilder.Entity<MonitoringConfig>()
            //    .HasMany(e => e.ConfigDetails)
            //    .WithRequired(e => e.Template)
            //    .HasForeignKey(e => e.TemplateId)
            //    .WillCascadeOnDelete(false);
            #endregion

            #region 1029 by iefhong for ConfigDetail

            modelBuilder.Entity<MonitoringConfig>()
                .HasMany(e => e.ConfigDetailHolidayTimeControlTemplates)
                .WithOptional(e => e.HolidayTimeControlTemplate)
                .HasForeignKey(e => e.HolidayTimeControlTemplateId);

            modelBuilder.Entity<MonitoringConfig>()
                .HasMany(e => e.ConfigDetailPeacetimeTimeControlTemplates)
                .WithOptional(e => e.PeacetimeTimeControlTemplate)
                .HasForeignKey(e => e.PeacetimeTimeControlTemplateId);



            modelBuilder.Entity<MonitoringConfig>()
                .HasMany(e => e.ConfigDetailTemplates)
                .WithRequired(e => e.MonitoringConfigTemplate)
                .HasForeignKey(e => e.TemplateId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MonitoringConfig>()
                .HasMany(e => e.ConfigDetailVacationTimeControlTemplates)
                .WithOptional(e => e.VacationTimeControlTemplate)
                .HasForeignKey(e => e.VacationTimeControlTemplateId);

            modelBuilder.Entity<MonitoringConfig>()
                .HasMany(e => e.ConfigDetailWeekEndTimeControlTemplates)
                .WithOptional(e => e.WeekEndTimeControlTemplate)
                .HasForeignKey(e => e.WeekEndTimeControlTemplateId);

            #endregion

            #region 1031 by iefhong for Add Columns

            //historyBill能耗分类
            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.HitoryBillsByEnergyCategory)
                .WithOptional(e => e.EnergyCategory)
                .HasForeignKey(e => e.EnergyCategoryId);

            //user能好用户分类
            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.UserType)
                .WithOptional(e => e.EnergyUserType)
                .HasForeignKey(e => e.UserType);

            //meter安装模式
            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.MeterSetupMode)
                .WithOptional(e => e.DictionarySetupMode)
                .HasForeignKey(e => e.SetupMode);

            //building用途
            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.BuildingPurpose)
                .WithOptional(e => e.DictionaryEnergyUserType)
                .HasForeignKey(e => e.Purpose);

            //MetersAction设备
            modelBuilder.Entity<Meter>()
                .HasMany(e => e.MetersActions)
                .WithRequired(e => e.Meter)
                .WillCascadeOnDelete(false);

            //MetersAction动作id
            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.Actions)
                .WithRequired(e => e.Action)
                .WillCascadeOnDelete(false);

            //MetersAction父id
            modelBuilder.Entity<MetersAction>()
                .HasMany(e => e.Children)
                .WithOptional(e => e.Parent)
                .HasForeignKey(e => e.ParentId);

            //Maintenance PurchaseId
            modelBuilder.Entity<Maintenance>()
                .HasMany(e => e.PurchaseId)
                .WithRequired(e => e.MaintenancePurchase)
                .HasForeignKey(e => e.MaintenanceId)
                .WillCascadeOnDelete(false);

            //CommandQueue MetersAction
            modelBuilder.Entity<MetersAction>()
                .HasMany(e => e.CommandQueue)
                .WithRequired(e => e.MetersAction)
                .HasForeignKey(e => e.CommandGroup) //MeterActionId 修改为 CommandGroup
                .WillCascadeOnDelete(false);

            //MetersAction命令状态id
            modelBuilder.Entity<Dictionary>()
                .HasMany(e => e.CommandStatusDic)
                //.WithRequired(e => e.CommandStatusD)
                //.WillCascadeOnDelete(false);
                .WithOptional(e => e.CommandStatusDic)
                .HasForeignKey(e => e.CommandStatus);
            #endregion

            #region 0214 by iefhong for Add Columns for [Meter]

            modelBuilder.Entity<Meter>()
                .Property(e => e.ActivePrecise)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Meter>()
                .Property(e => e.ReactivePrecise)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Meter>()
                .Property(e => e.SpeedRate)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Meter>()
                .Property(e => e.PtRate)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Meter>()
                .Property(e => e.RangeRatio)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Meter>()
                .Property(e => e.Hydraulic)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Meter>()
                .Property(e => e.Flow)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Meter>()
                .Property(e => e.InitialValue)
                .HasPrecision(18, 4);

            #endregion


            modelBuilder.Entity<Setting>()
                .Property(e => e.ElectricityPrice)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Setting>()
                .Property(e => e.WaterPrice)
                .HasPrecision(18, 4);

        }


    }
}
