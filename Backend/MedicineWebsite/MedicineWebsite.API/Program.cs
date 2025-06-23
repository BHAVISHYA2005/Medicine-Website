using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MedicineWebsite.Application.Interfaces;
using MedicineWebsite.Application.Services;
using MedicineWebsite.Domain.Entities;
using MedicineWebsite.Persistence.Data;
using MedicineWebsite.Persistence.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Entity Framework with In-Memory Database for testing
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("MedicineWebsiteDb"));

// Configure Identity
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    // Password settings
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    
    // User settings
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;
    
    // Lockout settings
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? "YourSuperSecretKeyForDevelopment123!@#$%^&*()";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"] ?? "MedicineWebsiteAPI",
        ValidAudience = jwtSettings["Audience"] ?? "MedicineWebsiteUsers",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.Zero
    };
});

// Register repositories
builder.Services.AddScoped<IMedicineRepository, MedicineRepository>();
builder.Services.AddScoped<IPharmacyRepository, PharmacyRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Register application services
builder.Services.AddScoped<IMedicineService, MedicineService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Medicine Website API",
        Version = "v1",
        Description = "API for Medicine Website - Connect patients with nearby pharmacies"
    });
    
    // Configure JWT in Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Seed data for development
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    SeedData(context);
}

app.Run();

// Seed data method
static void SeedData(ApplicationDbContext context)
{
    if (!context.Medicines.Any())
    {        var medicines = new List<Medicine>
        {
            new Medicine
            {
                Name = "Paracetamol 500mg",
                GenericName = "Acetaminophen",
                Description = "Pain reliever and fever reducer",
                DrugClass = "Analgesic",
                Manufacturer = "Generic Pharma",
                Strength = "500mg",
                DosageForm = "Tablet",
                ActiveIngredients = "Acetaminophen 500mg",
                IsPrescriptionRequired = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new Medicine
            {
                Name = "Ibuprofen 400mg",
                GenericName = "Ibuprofen",
                Description = "Anti-inflammatory pain reliever",
                DrugClass = "NSAID",
                Manufacturer = "MediCare",
                Strength = "400mg",
                DosageForm = "Tablet",
                ActiveIngredients = "Ibuprofen 400mg",
                IsPrescriptionRequired = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new Medicine
            {
                Name = "Vitamin D3 1000IU",
                GenericName = "Cholecalciferol",
                Description = "Essential vitamin supplement",
                DrugClass = "Vitamin",
                Manufacturer = "Health Plus",
                Strength = "1000IU",
                DosageForm = "Capsule",
                ActiveIngredients = "Cholecalciferol 1000IU",
                IsPrescriptionRequired = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new Medicine
            {
                Name = "Amoxicillin 500mg",
                GenericName = "Amoxicillin",
                Description = "Antibiotic for bacterial infections",
                DrugClass = "Antibiotic",
                Manufacturer = "Generic Pharma",
                Strength = "500mg",
                DosageForm = "Capsule",
                ActiveIngredients = "Amoxicillin 500mg",
                IsPrescriptionRequired = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new Medicine
            {
                Name = "Aspirin 100mg",
                GenericName = "Acetylsalicylic Acid",
                Description = "Low-dose aspirin for heart health",
                DrugClass = "Antiplatelet",
                Manufacturer = "CardioMed",
                Strength = "100mg",
                DosageForm = "Tablet",
                ActiveIngredients = "Acetylsalicylic Acid 100mg",
                IsPrescriptionRequired = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            }
        };

        context.Medicines.AddRange(medicines);
        context.SaveChanges();

        // Add some pharmacies        var pharmacies = new List<Pharmacy>
        {
            new Pharmacy
            {
                Name = "HealthPlus Pharmacy",
                Address = "123 Main Street",
                City = "New York",
                State = "NY",
                PostalCode = "10001",
                Country = "USA",
                PhoneNumber = "+1 (555) 123-4567",
                Email = "info@healthplus.com",
                Latitude = 40.7128,
                Longitude = -74.0060,
                OpeningTime = new TimeSpan(8, 0, 0),
                ClosingTime = new TimeSpan(20, 0, 0),
                IsOpen24Hours = false,
                WorkingDays = "[\"Monday\",\"Tuesday\",\"Wednesday\",\"Thursday\",\"Friday\",\"Saturday\"]",
                DeliveryFee = 5.99m,
                MinimumOrderAmount = 25.00m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Pharmacy
            {
                Name = "MediCare Pharmacy",
                Address = "456 Oak Avenue",
                City = "New York",
                State = "NY",
                PostalCode = "10002",
                Country = "USA",
                PhoneNumber = "+1 (555) 987-6543",
                Email = "contact@medicare.com",
                Latitude = 40.7589,
                Longitude = -73.9851,
                OpeningTime = new TimeSpan(9, 0, 0),
                ClosingTime = new TimeSpan(22, 0, 0),
                IsOpen24Hours = false,
                WorkingDays = "[\"Monday\",\"Tuesday\",\"Wednesday\",\"Thursday\",\"Friday\",\"Saturday\",\"Sunday\"]",
                DeliveryFee = 4.99m,
                MinimumOrderAmount = 20.00m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        context.Pharmacies.AddRange(pharmacies);
        context.SaveChanges();

        // Add pharmacy medicines with prices
        var pharmacyMedicines = new List<PharmacyMedicine>();
        var medicinesList = context.Medicines.ToList();
        var pharmaciesList = context.Pharmacies.ToList();        foreach (var pharmacy in pharmaciesList)
        {
            foreach (var medicine in medicinesList)
            {
                pharmacyMedicines.Add(new PharmacyMedicine
                {
                    PharmacyId = pharmacy.Id,
                    MedicineId = medicine.Id,
                    Price = Random.Shared.Next(5, 100),
                    StockQuantity = Random.Shared.Next(10, 100),
                    IsAvailable = true,
                    ExpirationDate = DateTime.UtcNow.AddMonths(Random.Shared.Next(6, 24)),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }
        }

        context.PharmacyMedicines.AddRange(pharmacyMedicines);
        context.SaveChanges();
    }
}
