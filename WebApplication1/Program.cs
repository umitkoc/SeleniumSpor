using Microsoft.AspNetCore.Mvc;
using WebApplication1;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
//
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapGet("/selenium-spor", ([FromBody]SeleniumSporRequest request) =>SporSelenium.Run(request));
app.Run();
