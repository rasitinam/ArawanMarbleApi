//using ArawanMarbleApi.Models;
using ArawanMarbleApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;  // Geliþtirme aþamasýnda https gerekmeyebilir
        options.SaveToken = true;  // Token'ý saklamayý aktif et
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "YourIssuer",  // Uygulama için geçerli issuer
            ValidAudience = "YourAudience",  // Uygulama için geçerli audience
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecretKey"))  // Gizli anahtar
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()    // Tüm origin'lere izin ver
              .AllowAnyMethod()    // Tüm HTTP metodlarýna izin ver (GET, POST, vb.)
              .AllowAnyHeader();   // Tüm header'lara izin ver
    });
});
builder.Services.AddEndpointsApiExplorer();  // API'nin uç noktalarýný keþfetmek için
builder.Services.AddSwaggerGen();  // Swagger'ý ekliyoruz
// Servisleri ekle
builder.Services.AddControllers();
builder.Services.AddDbContext<Ara56nmarblecomContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));  // SQLite baðlantý dizesi
var app = builder.Build();

// Middleware'leri doðru sýrayla ekleyin
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseCors("AllowAll");  // CORS politikasýný uygulamaya dahil et

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