using System.Text;
using Blog.Data;
using Blogs;
using Blogs.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;//DEFINE O SCKEME
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;//PESQUISAR DEPOIS O QUE FAZ ISSO
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true, //validar a chave de assinatura
        IssuerSigningKey = new SymmetricSecurityKey(key), // aqui eu digo como ele valida essa chave
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


builder.Services.AddControllers();
builder.Services.AddDbContext<BlogDataContext>();
builder.Services.AddTransient<TokenService>(); // sempre cria um novo
// builder.Services.AddScoped(); // igual o transient só que é por transação, 
                              //se pedir duas vezes na mesma classe, vai ser chamado a mesma instancia
// builder.Services.AddSingleton(); // mesma intancia para a aplicação toda

var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthentication(); // quem você é
app.UseAuthorization(); // o que você pode fazer
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

//adicionar swagger 
//dotnet add package swashbuckle.aspnetcore
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
// app.UseSwagger();
// app.UseSwaggerUI();
