using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CosmosManagementApi.Models;

public partial class CosmosManagementDbContext : DbContext
{
    public CosmosManagementDbContext()
    {
    }

    public CosmosManagementDbContext(DbContextOptions<CosmosManagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bill> Bills { get; set; }

    public virtual DbSet<Card> Cards { get; set; }

    public virtual DbSet<CardBill> CardBills { get; set; }

    public virtual DbSet<Cprecord> Cprecords { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerProductBill> CustomerProductBills { get; set; }

    public virtual DbSet<CustomerProject> CustomerProjects { get; set; }

    public virtual DbSet<CustomerProjectBill> CustomerProjectBills { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Postion> Postions { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<ProductClass> ProductClasses { get; set; }

    public virtual DbSet<ProductStorageBill> ProductStorageBills { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectCategory> ProjectCategories { get; set; }

    public virtual DbSet<PurchaseCategory> PurchaseCategories { get; set; }

    public virtual DbSet<Staff> Staffs { get; set; }

    public virtual DbSet<StaffAccount> StaffAccounts { get; set; }

    public virtual DbSet<StaffProductBill> StaffProductBills { get; set; }

    public virtual DbSet<StaffProjectBill> StaffProjectBills { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=43.155.94.131,1433;Database=CosmosManagementDb;User ID=Callus;Password=18873939977Smart;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Bills__3214EC073A42260E");

            entity.Property(e => e.Id).HasComment("Primary key for bills");
            entity.Property(e => e.BillId)
                .HasMaxLength(50)
                .HasComment("Bill id in company")
                .HasColumnName("Bill_id");
            entity.Property(e => e.Comment)
                .HasMaxLength(255)
                .HasComment("Comments");
            entity.Property(e => e.Discount)
                .HasMaxLength(100)
                .HasComment("Discount for this bill");
            entity.Property(e => e.FinalPrice)
                .HasMaxLength(100)
                .HasComment("Final price of bill")
                .HasColumnName("Final_price");
            entity.Property(e => e.OriPrice)
                .HasMaxLength(100)
                .HasComment("Original price")
                .HasColumnName("Ori_price");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(255)
                .HasComment("PaymentMehtods")
                .HasColumnName("Payment_method");
            entity.Property(e => e.Time)
                .HasComment("When made")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Card>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Card__3214EC076D16B35F");

            entity.HasIndex(e => new { e.CustomerId, e.CardNo }, "Customer Card");

            entity.Property(e => e.Id).HasComment("Primary key for Customer Card");
            entity.Property(e => e.CardNo)
                .HasMaxLength(50)
                .HasComment("Card number")
                .HasColumnName("Card_no");
            entity.Property(e => e.CustomerId)
                .HasComment("ReferenceKey")
                .HasColumnName("Customer_id");
            entity.Property(e => e.Topped).HasColumnType("decimal(12, 2)");

            entity.HasOne(d => d.Customer).WithMany(p => p.Cards)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Card__Customer_i__05D8E0BE");
        });

        modelBuilder.Entity<CardBill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CardBill__3214EC0754EACEC0");

            entity.HasIndex(e => new { e.CardId, e.BillId }, "[Card_id,Bill_id]");

            entity.Property(e => e.Id).HasComment("Primary Key for Card Bills");
            entity.Property(e => e.BillId)
                .HasComment("Reference key to bill")
                .HasColumnName("Bill_id");
            entity.Property(e => e.CardId)
                .HasComment("ReferenceKey to Card")
                .HasColumnName("Card_id");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(150)
                .HasComment("How paid")
                .HasColumnName("Payment_method");
            entity.Property(e => e.StaffId).HasColumnName("Staff_id");

            entity.HasOne(d => d.Bill).WithMany(p => p.CardBills)
                .HasForeignKey(d => d.BillId)
                .HasConstraintName("FK__CardBills__Bill___09A971A2");

            entity.HasOne(d => d.Card).WithMany(p => p.CardBills)
                .HasForeignKey(d => d.CardId)
                .HasConstraintName("FK__CardBills__Card___08B54D69");
        });

        modelBuilder.Entity<Cprecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CPrecord__3214EC07440CC85B");

            entity.ToTable("CPrecords");

            entity.Property(e => e.Id).HasComment("Primary key CPrecords");
            entity.Property(e => e.Behaviour).HasComment("Behaviour for Action");
            entity.Property(e => e.CpId)
                .HasComment("Reference key to Customer Projects")
                .HasColumnName("CP_id");
            entity.Property(e => e.PicSrc)
                .HasMaxLength(255)
                .HasComment("Path to picture")
                .HasColumnName("Pic_src");
            entity.Property(e => e.Time)
                .HasComment("When actions happend")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Cp).WithMany(p => p.Cprecords)
                .HasForeignKey(d => d.CpId)
                .HasConstraintName("FK__CPrecords__CP_id__30C33EC3");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC07EFAC491C");

            entity.Property(e => e.Id).HasComment("Primary key for the Customer");
            entity.Property(e => e.Age)
                .HasMaxLength(125)
                .HasComment("User Age in brief");
            entity.Property(e => e.Birthday)
                .HasComment("Customer birthday")
                .HasColumnType("date");
            entity.Property(e => e.Comment)
                .HasMaxLength(255)
                .HasComment("Special comment of the user");
            entity.Property(e => e.CustomerId)
                .HasMaxLength(50)
                .HasComment("Customer number in systen")
                .HasColumnName("Customer_id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasComment("Customer Email address");
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .HasComment("Customer gender");
            entity.Property(e => e.Img)
                .HasMaxLength(255)
                .HasComment("path to customer img");
            entity.Property(e => e.IsDeleted).HasComment("0 for not 1 for Deleted");
            entity.Property(e => e.Kind)
                .HasMaxLength(125)
                .HasComment("What kind of User is it");
            entity.Property(e => e.KnoingMethod)
                .HasMaxLength(255)
                .HasComment("How Knows This Company")
                .HasColumnName("Knoing_method");
            entity.Property(e => e.Level)
                .HasMaxLength(50)
                .HasComment("Customer Level");
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .HasComment("Customer living location");
            entity.Property(e => e.NameCn)
                .HasMaxLength(100)
                .HasComment("Customer chinese name")
                .HasColumnName("Name_cn");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasComment("Cusomer Phone number")
                .HasColumnName("Phone ");
            entity.Property(e => e.Point).HasComment("Customer points");
            entity.Property(e => e.Rating)
                .HasMaxLength(100)
                .HasComment("Customer Rating");
            entity.Property(e => e.RegDate)
                .HasComment("When customer registered in system")
                .HasColumnType("datetime")
                .HasColumnName("Reg_date");
        });

        modelBuilder.Entity<CustomerProductBill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC0710415D4A");

            entity.HasIndex(e => new { e.CustomerId, e.ProductId, e.BillId }, "CustomerProductBillid").IsUnique();

            entity.HasIndex(e => new { e.CustomerId, e.ProductId, e.BillId }, "CustomerProductBillidUnique").IsUnique();

            entity.Property(e => e.Id).HasComment("Primary key");
            entity.Property(e => e.BigCategory)
                .HasMaxLength(255)
                .HasComment("The biggest category used for reconciliation")
                .HasColumnName("Big_category");
            entity.Property(e => e.BillId)
                .HasComment("Bill id")
                .HasColumnName("Bill_id");
            entity.Property(e => e.Comment)
                .HasMaxLength(255)
                .HasComment("Comment of this bill");
            entity.Property(e => e.CustomerId)
                .HasComment("Customer id")
                .HasColumnName("Customer_id");
            entity.Property(e => e.Discount)
                .HasMaxLength(100)
                .HasComment("Discount");
            entity.Property(e => e.NetPrice)
                .HasMaxLength(100)
                .HasComment("Net price calculated by unit price")
                .HasColumnName("Net_price");
            entity.Property(e => e.Number).HasComment("How many product bought");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(255)
                .HasComment("how is it paid")
                .HasColumnName("Payment_method");
            entity.Property(e => e.ProductId)
                .HasComment("Product id")
                .HasColumnName("Product_id");
            entity.Property(e => e.StaffId)
                .HasComment("Staff who made this purchase")
                .HasColumnName("Staff_id");
            entity.Property(e => e.UnitPrice)
                .HasMaxLength(100)
                .HasComment("Unit price ")
                .HasColumnName("Unit_price");

            entity.HasOne(d => d.Bill).WithMany(p => p.CustomerProductBills)
                .HasForeignKey(d => d.BillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerP__Bill___5EBF139D");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerProductBills)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerP__Custo__5CD6CB2B");

            entity.HasOne(d => d.Product).WithMany(p => p.CustomerProductBills)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerP__Produ__5DCAEF64");
        });

        modelBuilder.Entity<CustomerProject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC07602F7BAF");

            entity.HasIndex(e => new { e.CustomerId, e.ProjectId, e.Id }, "C").IsUnique();

            entity.Property(e => e.Id).HasComment("Primary key for CustoemrProjects");
            entity.Property(e => e.CustomerId)
                .HasComment("Reference key to customer")
                .HasColumnName("Customer_id");
            entity.Property(e => e.Number).HasComment("project number left");
            entity.Property(e => e.ProjectId)
                .HasComment("Reference key to project")
                .HasColumnName("Project_id");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerProjects)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerP__Custo__1332DBDC");

            entity.HasOne(d => d.Project).WithMany(p => p.CustomerProjects)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerP__Proje__14270015");
        });

        modelBuilder.Entity<CustomerProjectBill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC0710415D4A_copy1");

            entity.HasIndex(e => new { e.CustomerId, e.ProjectId, e.BillId }, "CustomerProjectBillid_copy1").IsUnique();

            entity.HasIndex(e => new { e.CustomerId, e.ProjectId, e.BillId }, "UQ__Customer__3FFFD2383B292408").IsUnique();

            entity.Property(e => e.Id).HasComment("Primary key");
            entity.Property(e => e.BigCategory)
                .HasMaxLength(255)
                .HasComment("The biggest category")
                .HasColumnName("Big_category");
            entity.Property(e => e.BillId)
                .HasComment("Bill id")
                .HasColumnName("Bill_id");
            entity.Property(e => e.Category)
                .HasMaxLength(255)
                .HasComment("which category this bill belongs to");
            entity.Property(e => e.Comment)
                .HasMaxLength(255)
                .HasComment("Comment of this bill");
            entity.Property(e => e.CustomerId)
                .HasComment("Customer id")
                .HasColumnName("Customer_id");
            entity.Property(e => e.Discount)
                .HasMaxLength(100)
                .HasComment("Discount used to calculate net price");
            entity.Property(e => e.NetPrice)
                .HasMaxLength(100)
                .HasComment("Sum of unit price")
                .HasColumnName("Net_price");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(255)
                .HasComment("how is it paid")
                .HasColumnName("Payment_method");
            entity.Property(e => e.ProjectId)
                .HasComment("Product id")
                .HasColumnName("Project_id");
            entity.Property(e => e.ProjectNumber)
                .HasComment("How many times the Project will go through")
                .HasColumnName("Project_Number");
            entity.Property(e => e.Staffid).HasComment("Staff who made this purchase");
            entity.Property(e => e.UnitPrice)
                .HasMaxLength(100)
                .HasComment("Unit price for a project")
                .HasColumnName("Unit_price");

            entity.HasOne(d => d.Bill).WithMany(p => p.CustomerProjectBills)
                .HasForeignKey(d => d.BillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerP__Bill___59FA5E80");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerProjectBills)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerP__Custo__5812160E");

            entity.HasOne(d => d.Project).WithMany(p => p.CustomerProjectBills)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerP__Proje__59063A47");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3214EC076DB1C675");

            entity.Property(e => e.Id).HasComment("Primary Key For Department");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(150)
                .HasComment("Department Name")
                .HasColumnName("Department_name");
            entity.Property(e => e.DepartmentNameEn)
                .HasMaxLength(150)
                .HasComment("Department English Name")
                .HasColumnName("Department_name_en");
            entity.Property(e => e.DepartmentNo)
                .HasMaxLength(150)
                .HasComment("Department Number in Company")
                .HasColumnName("Department_no");
            entity.Property(e => e.RegisteredNo)
                .HasMaxLength(150)
                .HasComment("Department Registered Nmuber")
                .HasColumnName("Registered_no");
        });

        modelBuilder.Entity<Postion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Postions__3214EC07BDC4585A");

            entity.Property(e => e.Id).HasComment("Primary key for Position");
            entity.Property(e => e.PositionName)
                .HasMaxLength(150)
                .HasComment("Postion name")
                .HasColumnName("Position_name");
            entity.Property(e => e.PositionNo)
                .HasMaxLength(150)
                .HasComment("Position number in company")
                .HasColumnName("Position_no");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product__3214EC07F6C21D94");

            entity.Property(e => e.Id).HasComment("Primary key for product");
            entity.Property(e => e.IfSelled)
                .HasComment("If product is selled")
                .HasColumnName("If_selled");
            entity.Property(e => e.ProductDate)
                .HasComment("Product purchse date")
                .HasColumnType("date")
                .HasColumnName("Product_date");
            entity.Property(e => e.ProductEndDate)
                .HasComment("Product end date")
                .HasColumnType("date")
                .HasColumnName("Product_end_date");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductC__3214EC07D4239062");

            entity.ToTable("ProductCategory");

            entity.Property(e => e.Id).HasComment("Primary key for Product Category");
            entity.Property(e => e.ClassId).HasColumnName("Class_id");
            entity.Property(e => e.InStorageTime)
                .HasColumnType("date")
                .HasColumnName("In_storage_time");
            entity.Property(e => e.ProductId).HasColumnName("Product_id");

            entity.HasOne(d => d.Class).WithMany(p => p.ProductCategories)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK__ProductCa__Class__73BA3083");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductCategories)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__ProductCa__Produ__72C60C4A");
        });

        modelBuilder.Entity<ProductClass>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product__3214EC07F6C21D94_copy1");

            entity.Property(e => e.Id).HasComment("Primary key for product");
            entity.Property(e => e.BigCategory)
                .HasMaxLength(255)
                .HasComment("Used for Bills")
                .HasColumnName("Big_category");
            entity.Property(e => e.BuyingPrice)
                .HasMaxLength(75)
                .HasComment("Buying price of product")
                .HasColumnName("Buying_price");
            entity.Property(e => e.Category)
                .HasMaxLength(125)
                .HasComment("ProductClassCategory");
            entity.Property(e => e.Img)
                .HasMaxLength(125)
                .HasComment("Path to img directory");
            entity.Property(e => e.Introduction)
                .HasMaxLength(255)
                .HasComment("Product introduction");
            entity.Property(e => e.ProductId)
                .HasMaxLength(75)
                .HasComment("Product Company id")
                .HasColumnName("Product_id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasComment("Product name")
                .HasColumnName("Product_name");
            entity.Property(e => e.ProductVolume)
                .HasMaxLength(100)
                .HasComment("Product size volume")
                .HasColumnName("Product_volume");
            entity.Property(e => e.SellingPrice)
                .HasMaxLength(75)
                .HasComment("Recommened selling price")
                .HasColumnName("Selling_price");
            entity.Property(e => e.Storage).HasComment("How many item in storage");
        });

        modelBuilder.Entity<ProductStorageBill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductS__3214EC07E86652EF");

            entity.Property(e => e.Id).HasComment("ProductStorage Primary id");
            entity.Property(e => e.BillId).HasColumnName("Bill_id");
            entity.Property(e => e.ProductId).HasColumnName("Product_id");

            entity.HasOne(d => d.Bill).WithMany(p => p.ProductStorageBills)
                .HasForeignKey(d => d.BillId)
                .HasConstraintName("FK__ProductSt__Bill___60A75C0F");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductStorageBills)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__ProductSt__Produ__5FB337D6");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Project__3214EC07858353BC");

            entity.Property(e => e.Id).HasComment("Primary key for project");
            entity.Property(e => e.BigCategory)
                .HasMaxLength(456)
                .HasComment("Big category for Projects")
                .HasColumnName("Big_category");
            entity.Property(e => e.Category)
                .HasMaxLength(100)
                .HasComment("Category of the project");
            entity.Property(e => e.Department).HasComment("Reference key to department");
            entity.Property(e => e.Introduction)
                .HasMaxLength(255)
                .HasComment("Project introduction");
            entity.Property(e => e.IsDeleted)
                .HasComment("0 for not deleted")
                .HasColumnName("Is_deleted");
            entity.Property(e => e.Period)
                .HasMaxLength(50)
                .HasComment("Project period");
            entity.Property(e => e.ProjectName)
                .HasMaxLength(255)
                .HasComment("Project name")
                .HasColumnName("Project_name");
            entity.Property(e => e.ProjectPrice)
                .HasMaxLength(125)
                .HasComment("Price for the project")
                .HasColumnName("Project_price");
            entity.Property(e => e.ProjectTimes)
                .HasComment("How many times the project will do in furture")
                .HasColumnName("Project_times");

            entity.HasOne(d => d.DepartmentNavigation).WithMany(p => p.Projects)
                .HasForeignKey(d => d.Department)
                .HasConstraintName("FK__Projects__Depart__628FA481");
        });

        modelBuilder.Entity<ProjectCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC0747CC92F0");

            entity.Property(e => e.Category).HasMaxLength(255);
            entity.Property(e => e.IsDeleted)
                .HasComment("0 is not deleted")
                .HasColumnName("Is_Deleted");
            entity.Property(e => e.WritingTime)
                .HasComment("When Input TIme")
                .HasColumnType("datetime")
                .HasColumnName("Writing_Time");
        });

        modelBuilder.Entity<PurchaseCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Purchase__3214EC07A4465011");

            entity.ToTable("PurchaseCategory");

            entity.Property(e => e.Id).HasComment("PrimaryKey");
            entity.Property(e => e.Category)
                .HasMaxLength(255)
                .HasComment("Purchase category after topup");
            entity.Property(e => e.IsDeleted).HasComment("1 for deleted");
            entity.Property(e => e.WritingTime)
                .HasComment("when record")
                .HasColumnType("date")
                .HasColumnName("Writing_time");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Staffs__3214EC071D3A8EBF");

            entity.Property(e => e.Id).HasComment("Primary key for Staff");
            entity.Property(e => e.Birthday)
                .HasComment("Staff birthday")
                .HasColumnType("date");
            entity.Property(e => e.DepartmentId)
                .HasComment("reference key to departments")
                .HasColumnName("Department_id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasComment("Email address")
                .HasColumnName("Email ");
            entity.Property(e => e.Gender)
                .HasMaxLength(25)
                .HasComment("Gender");
            entity.Property(e => e.IdCard)
                .HasMaxLength(50)
                .HasComment("Staff id card number")
                .HasColumnName("Id_card");
            entity.Property(e => e.IfOnboard)
                .HasComment("if staff is still on board")
                .HasColumnName("If_onboard");
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .HasComment("Where the staff lives");
            entity.Property(e => e.OnboardDate)
                .HasComment("When staff onboard")
                .HasColumnType("date")
                .HasColumnName("Onboard_date");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasComment("Phone number of staff");
            entity.Property(e => e.PositionId)
                .HasComment("reference key to positions")
                .HasColumnName("Position_id");
            entity.Property(e => e.StaffId)
                .HasMaxLength(50)
                .HasComment("Staff id for staff")
                .HasColumnName("Staff_id");
            entity.Property(e => e.StaffName)
                .HasMaxLength(100)
                .HasComment("StaffName")
                .HasColumnName("Staff_name");
            entity.Property(e => e.StaffNameEn)
                .HasMaxLength(100)
                .HasComment("Staff English name")
                .HasColumnName("Staff_name_en");

            entity.HasOne(d => d.Department).WithMany(p => p.Staff)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK__Staffs__Departme__656C112C");

            entity.HasOne(d => d.Position).WithMany(p => p.Staff)
                .HasForeignKey(d => d.PositionId)
                .HasConstraintName("FK__Staffs__Position__5BE2A6F2");
        });

        modelBuilder.Entity<StaffAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StaffAcc__3214EC0736885957");

            entity.Property(e => e.Id).HasComment("Account primary id");
            entity.Property(e => e.AccountId)
                .HasMaxLength(50)
                .HasComment("Account Id")
                .HasColumnName("Account_id");
            entity.Property(e => e.AccountName)
                .HasMaxLength(50)
                .HasComment("Account Name")
                .HasColumnName("Account_name");
            entity.Property(e => e.IsActive)
                .HasComment("1 for Active")
                .HasColumnName("Is_Active");
            entity.Property(e => e.IsLock)
                .HasComment("1 for locked")
                .HasColumnName("Is_lock");
            entity.Property(e => e.LastLogInTime)
                .HasComment("Last login time")
                .HasColumnType("datetime")
                .HasColumnName("Last_log_in_time");
            entity.Property(e => e.Level).HasComment("5 for max level");
            entity.Property(e => e.LogInFailedTime)
                .HasColumnType("datetime")
                .HasColumnName("Log_in_failed_time");
            entity.Property(e => e.LogInFailedTimes).HasColumnName("Log_in_failed_times");
            entity.Property(e => e.LsatLogInIp)
                .HasMaxLength(100)
                .HasComment("Last login Ip")
                .HasColumnName("Lsat_log_in_Ip");
            entity.Property(e => e.Pwd)
                .HasMaxLength(125)
                .HasComment("Password");
            entity.Property(e => e.PwdHash)
                .HasMaxLength(255)
                .HasComment("hashed password")
                .HasColumnName("Pwd_hash");
            entity.Property(e => e.RegTime)
                .HasComment("Registry TIME")
                .HasColumnType("datetime")
                .HasColumnName("Reg_time");
            entity.Property(e => e.StaffId)
                .HasComment("ReferenceKey to staff")
                .HasColumnName("Staff_id");

            entity.HasOne(d => d.Staff).WithMany(p => p.StaffAccounts)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK__StaffAcco__Staff__6383C8BA");
        });

        modelBuilder.Entity<StaffProductBill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC0710415D4A_copy2");

            entity.HasIndex(e => new { e.StaffId, e.ProductId, e.BillId }, "StaffProductBillid_copy2").IsUnique();

            entity.HasIndex(e => new { e.StaffId, e.ProductId, e.BillId }, "UQ__Staff__3FFFD2389E43FC3E").IsUnique();

            entity.Property(e => e.Id).HasComment("Primary key");
            entity.Property(e => e.BillId)
                .HasComment("Bill id")
                .HasColumnName("Bill_id");
            entity.Property(e => e.Number).HasComment("How many product bought");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .HasComment("how is it paid")
                .HasColumnName("Payment_method");
            entity.Property(e => e.ProductId)
                .HasComment("Product id")
                .HasColumnName("Product_id");
            entity.Property(e => e.StaffId)
                .HasComment("Customer id")
                .HasColumnName("Staff_id");

            entity.HasOne(d => d.Bill).WithMany(p => p.StaffProductBills)
                .HasForeignKey(d => d.BillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StaffProd__Bill___68487DD7");

            entity.HasOne(d => d.Product).WithMany(p => p.StaffProductBills)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StaffProd__Produ__6754599E");

            entity.HasOne(d => d.Staff).WithMany(p => p.StaffProductBills)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StaffProd__Staff__66603565");
        });

        modelBuilder.Entity<StaffProjectBill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC0710415D4A_copy3");

            entity.HasIndex(e => new { e.StaffId, e.ProjectId, e.BillId }, "StaffProjectBillid").IsUnique();

            entity.HasIndex(e => new { e.StaffId, e.ProjectId, e.BillId }, "UQ__Staff__3FFFD238A59E841B").IsUnique();

            entity.Property(e => e.Id).HasComment("Primary key");
            entity.Property(e => e.BillId)
                .HasComment("Bill id")
                .HasColumnName("Bill_id");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .HasComment("how is it paid")
                .HasColumnName("Payment_method");
            entity.Property(e => e.ProjectId)
                .HasComment("Product id")
                .HasColumnName("Project_id");
            entity.Property(e => e.ProjectNumber)
                .HasComment("How many times project will do")
                .HasColumnName("Project_Number");
            entity.Property(e => e.StaffId)
                .HasComment("Customer id")
                .HasColumnName("Staff_id");

            entity.HasOne(d => d.Bill).WithMany(p => p.StaffProjectBills)
                .HasForeignKey(d => d.BillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StaffProj__Bill___6D0D32F4");

            entity.HasOne(d => d.Project).WithMany(p => p.StaffProjectBills)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StaffProj__Proje__6C190EBB");

            entity.HasOne(d => d.Staff).WithMany(p => p.StaffProjectBills)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StaffProj__Staff__6B24EA82");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
