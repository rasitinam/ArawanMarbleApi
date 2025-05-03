using ArawanMarbleApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDistributedMemoryCache(); // Session için gerekli
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(1); // Oturum süresi (örneðin, 1 dakika)
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "ArawanAuthServer",
            ValidAudience = "ArawanMarbleUsers",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MyUltraSecureSuperLongSecretKey123456"))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin() // Herhangi bir origin'e izin verir
               .AllowAnyMethod() // GET, POST, PUT, DELETE vs.
               .AllowAnyHeader(); // Herhangi bir header'a izin verir
    });
});


builder.Services.AddEndpointsApiExplorer();  // API'nin uç noktalarýný keþfetmek için
builder.Services.AddSwaggerGen();  // Swagger'ý ekliyoruz
// Servisleri ekle
builder.Services.AddControllers();
builder.Services.AddDbContext<Ara56nmarblecomContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));  // SQLite baðlantý dizesi
var app = builder.Build();
app.UseSession();
// Middleware'leri doðru sýrayla ekleyin
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseCors("AllowSpecificOrigins");  // CORS politikasýný uygulamaya dahil et

// Middleware'leri ekleyelim
app.UseRouting();
app.UseHttpsRedirection();

// ÖNCE UseDefaultFiles, SONRA UseStaticFiles
app.UseDefaultFiles(); // index.html, default.html, vs. arar
app.UseStaticFiles(); // wwwroot içindeki dosyalarý sunar
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();  // Swagger'ý kullanmaya baþlýyoruz
    app.UseSwaggerUI();  // Swagger UI'yi aktif hale getiriyoruz
}
// 5. Middleware - Authentication ve Authorization'ý Aktifleþtir
app.UseAuthentication();  // Authentication middleware'ini aktif et
app.UseAuthorization();
app.MapControllers();

// SPA için gerekirse (tüm route'larý index.html'e yönlendirir)
app.MapFallbackToFile("index.html");

app.Run();