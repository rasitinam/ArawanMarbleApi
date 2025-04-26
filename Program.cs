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
        options.RequireHttpsMetadata = false;  // Geli�tirme a�amas�nda https gerekmeyebilir
        options.SaveToken = true;  // Token'� saklamay� aktif et
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "YourIssuer",  // Uygulama i�in ge�erli issuer
            ValidAudience = "YourAudience",  // Uygulama i�in ge�erli audience
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
        policy.AllowAnyOrigin()    // T�m origin'lere izin ver
              .AllowAnyMethod()    // T�m HTTP metodlar�na izin ver (GET, POST, vb.)
              .AllowAnyHeader();   // T�m header'lara izin ver
    });
});
builder.Services.AddEndpointsApiExplorer();  // API'nin u� noktalar�n� ke�fetmek i�in
builder.Services.AddSwaggerGen();  // Swagger'� ekliyoruz
// Servisleri ekle
builder.Services.AddControllers();
builder.Services.AddDbContext<Ara56nmarblecomContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));  // SQLite ba�lant� dizesi
var app = builder.Build();

// Middleware'leri do�ru s�rayla ekleyin
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseCors("AllowAll");  // CORS politikas�n� uygulamaya dahil et

// Middleware'leri ekleyelim
app.UseRouting();
app.UseHttpsRedirection();

// �NCE UseDefaultFiles, SONRA UseStaticFiles
app.UseDefaultFiles(); // index.html, default.html, vs. arar
app.UseStaticFiles(); // wwwroot i�indeki dosyalar� sunar
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();  // Swagger'� kullanmaya ba�l�yoruz
    app.UseSwaggerUI();  // Swagger UI'yi aktif hale getiriyoruz
}
// 5. Middleware - Authentication ve Authorization'� Aktifle�tir
app.UseAuthentication();  // Authentication middleware'ini aktif et
app.UseAuthorization();
app.MapControllers();

// SPA i�in gerekirse (t�m route'lar� index.html'e y�nlendirir)
app.MapFallbackToFile("index.html");

app.Run();