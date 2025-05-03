using ArawanMarbleApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDistributedMemoryCache(); // Session i�in gerekli
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(1); // Oturum s�resi (�rne�in, 1 dakika)
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


builder.Services.AddEndpointsApiExplorer();  // API'nin u� noktalar�n� ke�fetmek i�in
builder.Services.AddSwaggerGen();  // Swagger'� ekliyoruz
// Servisleri ekle
builder.Services.AddControllers();
builder.Services.AddDbContext<Ara56nmarblecomContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));  // SQLite ba�lant� dizesi
var app = builder.Build();
app.UseSession();
// Middleware'leri do�ru s�rayla ekleyin
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseCors("AllowSpecificOrigins");  // CORS politikas�n� uygulamaya dahil et

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